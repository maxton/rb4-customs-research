﻿using DtxCS;
using DtxCS.DataTypes;
using MidiCS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibForge.RBSong
{
  static class RBSongConverter
  {
    static StructType vec3 = StructType.FromData(DTX.FromDtaString("define vec3 (props (x float) (y float) (z float))"));
    static StructType xfm_type = StructType.FromData(DTX.FromDtaString(
      @"define xfm
            (props
              (basis_x vec3)
              (basis_y vec3)
              (basis_z vec3)
              (translate vec3))"));
    static Component editorComponent = new Component
    {
      Name1 = "Editor",
      Name2 = "Editor",
      Rev = 3,
      Unknown2 = 2,
      Props = new[]
        {
          new Property("capabilities", new UIntValue(50))
        }
    };

    static Component entityHeaderComponent = new Component
    {
      Name1 = "EntityHeader",
      Name2 = "EntityHeader",
      Rev = 3,
      Unknown2 = 1,
      Props = new[]
        {
          new Property("copy_on_instance", new BoolValue(true)),
          new Property("drives_parent", new BoolValue(false)),
          new Property("static", new BoolValue(false)),
          new Property("instance_polling_mode", new IntValue(0)),
          new Property("num_instances", new IntValue(0)),
          new Property("num_meshes", new IntValue(0)),
          new Property("num_particles", new IntValue(0)),
          new Property("num_propanims", new IntValue(0)),
          new Property("num_lights", new IntValue(0)),
          new Property("num_verts", new IntValue(0)),
          new Property("num_faces", new IntValue(0)),
          new Property("changelist", new IntValue(0)),
          new Property("icon_cam_initialized", new BoolValue(false)),
          new Property("icon_cam_near", new FloatValue(0.1f)),
          new Property("icon_cam_far", new FloatValue(1000f)),
          new Property("icon_cam_xfm", StructValue.FromData(xfm_type, DTX.FromDtaString(
            @"(basis_x ((x 1.0) (y 0.0) (z 0.0)))
              (basis_y ((x 0.0) (y 1.0) (z 0.0)))
              (basis_z ((x 0.0) (y 0.0) (z 1.0)))
              (translate ((x 0.0) (y 0.0) (z 0.0)))"))),
          new Property("icon_data",
            new ArrayValue(new ArrayType{ElementType = PrimitiveType.Byte, InternalType = DataType.Uint8 | DataType.Array }, new Value[] { }))
        }
    };

    public static RBSongResource MakeRBSong(DataArray array, MidiFile mf)
    {
      var drumBank = array.Array("drum_bank")?.Any(1)
        .Replace("sfx", "fusion/patches")
        .Replace("_bank.milo", ".fusion")
        ?? "fusion/patches/kit01.fusion";
      var animations = ExtractVenuePropAnimsFromMidi(mf);
      return new RBSongResource
      {
        Version = 0xE,
        Entity = new Entity
        {
          Version = 0x14,
          Layers = new[] {
            new EntityLayer
            {
              Version = 0x14,
              fileSlotIndex = 0,
              TotalObjectLayers = 1,
              Objects = new GameObject[]
              {
                new GameObject
                {
                  Id = new GameObjectId { Index = 0, Layer = 0 },
                  Rev = 2,
                  Name = "root",
                  Components = new Component[] {
                    editorComponent,
                    entityHeaderComponent,
                    new Component
                    {
                      Rev = 3,
                      Name1 = "RBSongMetadata",
                      Name2 = "RBSongMetadata",
                      Unknown2 = 4,
                      Props = new[]
                      {
                        new Property("tempo", new SymbolValue("medium")),
                        new Property("vocal_tonic_note", new LongValue(array.Array("vocal_tonic_note")?.Int(1) ?? 0)),
                        new Property("vocal_track_scroll_duration_ms", new LongValue(array.Array("song_scroll_speed")?.Int(1) ?? 2300)),
                        new Property("global_tuning_offset", new FloatValue(array.Array("tuning_offset_cents")?.Number(1) ?? 0)),
                        new Property("band_fail_sound_event", new SymbolValue("")),
                        new Property("vocal_percussion_patch", new ResourcePathValue("fusion/patches/vox_perc_tambourine.fusion")),
                        new Property("drum_kit_patch", new ResourcePathValue(drumBank)),
                        new Property("improv_solo_patch", new SymbolValue("gtrsolo_amer_03")),
                        new Property("dynamic_drum_fill_override", new IntValue(10)),
                        new Property("improv_solo_volume_db", new FloatValue(-9))
                      }
                    },
                    new Component
                    {
                      Rev = 3,
                      Name1 = "RBVenueAuthoring",
                      Name2 = "RBVenueAuthoring",
                      Unknown2 = 0,
                      Props = new[]
                      {
                        new Property("part2_instrument", new IntValue(2)),
                        new Property("part3_instrument", new IntValue(0)),
                        new Property("part4_instrument", new IntValue(1))
                      }
                    }
                  }
                }
              }
            },
          }
        },
        InlineLayerNames = new string[] { "" },
        InlineResourceLayers = new[] {
          new Resource[] {
            new PropAnimResource()
            {
              Path = "venue_authoring_data",
              Version = 0xE,
              InlineLayerNames = new string[] { "" },
              InlineResourceLayers = new Resource[][] { new Resource[] { } },
              Entity = new Entity
              {
                Version = 0x14,
                Layers = new[]
                {
                  new EntityLayer
                  {
                    Version = 0x14,
                    fileSlotIndex = 0,
                    TotalObjectLayers = (short)animations.Length,
                    Objects = animations,
                  }
                }
              }
            }
          }
        }
      };
    }
    static StructType propKeysType = StructType.FromData(DTX.FromDtaString(
        @"define propkeys
          (props
            (keys array (item struct (props (frame float) (value symbol))))
            (interpolation int)
            (driven_prop driven_prop))"));

    static GameObject[] ExtractVenuePropAnimsFromMidi(MidiFile mf)
    {
      var objs = new List<GameObject>();
      short index = 1;
      objs.Add(new GameObject
      {
        Id = new GameObjectId { Index = 0, Layer = 0 },
        Rev = 2,
        Name = "root",
        Components = new[]
        {
          editorComponent,
          entityHeaderComponent,
          new Component
          {
            Rev = 3,
            Name1 = "PropAnim",
            Name2 = "PropAnim",
            Unknown2 = 0,
            Props = StructValue.FromData(
                      StructType.FromData(DTX.FromDtaString(
                        @"(props 
                            (frame_range_start float)
                            (frame_range_end float)
                            (time_units int)
                            (is_looping bool))")),
                      DTX.FromDtaString(
                        @"(frame_range_start 3.402823E+38)
                          (frame_range_end -3.402823E+38)
                          (time_units 0)
                          (is_looping 0)")).Props

          }
        }
      });
      var tracks = new Midi.MidiHelper().ProcessTracks(mf);
      var partMap = new Dictionary<string, string> {
        { "PART BASS", "bass_intensity"},
        { "PART GUITAR", "guitar_intensity" },
        { "PART DRUMS", "drum_intensity" },
        { "PART VOCALS", "mic_intensity" },
        { "PART KEYS", "keyboard_intensity" },
      };
      var intensityMap = new Dictionary<string, string>
      {
        // All
        { "[idle_realtime]", "idle_realtime" },
        { "[idle]", "idle" },
        { "[idle_intense]", "idle_intense" },
        { "[play]", "play" },
        { "[mellow]", "mellow" },
        { "[intense]", "intense" },
        // Guitar/bass only
        { "[play_solo]", "play_solo" },
        // Vocal only
        { "[tambourine_start]", "tambourine_start" },
        { "[tambourine_end]", "tambourine_end" },
        { "[cowbell_start]", "cowbell_start" },
        { "[cowbell_end]", "cowbell_end" },
        { "[clap_start]", "clap_start" },
        { "[clap_end]", "clap_end" },
        // Drum only
        { "[ride_side_true]", "ride_side_true" },
        { "[ride_side_false]", "ride_side_false" },
      };
      foreach(var track in tracks)
      {
        if (!partMap.ContainsKey(track.Name))
          continue;
        var propkeys = DTX.FromDtaString(
          "(keys ()) " +
          "(interpolation 0) " +
          $"(driven_prop (0 0 RBVenueAuthoring 0 1 {partMap[track.Name]}))");
        var keyframes = propkeys.Array("keys").Array(1);
        foreach(var n in track.Items)
        {
          if(n is Midi.MidiText t)
          {
            if (intensityMap.ContainsKey(t.Text))
            {
              var keyframe = new DataArray();
              keyframe.AddNode(new DataAtom((float)n.StartTime));
              keyframe.AddNode(DataSymbol.Symbol(intensityMap[t.Text]));
              keyframes.AddNode(keyframe);
            }
          }
        }
        objs.Add(new GameObject
        {
          Id = new GameObjectId { Index = index, Layer = index++ },
          Rev = 2,
          Name = "Keys type 11",
          Components = new []
          {
            new Component
            {
              Rev = 3,
              Name1 = "Editor",
              Name2 = "Editor",
              Unknown2 = 2,
              Props = new[]
              {
                new Property("capabilities", new UIntValue(62))
              }
            },
            new Component
            {
              Rev = 3,
              Name1 = "PropKeysSymCom",
              Name2 = "PropKeys",
              Unknown2 = 0,
              Props = StructValue.FromData(propKeysType, propkeys).Props
            }
          }
        });
      }
      return objs.ToArray();
    }
  }
}
