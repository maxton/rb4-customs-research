﻿using System;
using System.IO;
using System.Text;

namespace LibForge.Extensions
{
  internal static class StreamExtensions
  {
    /// <summary>
    /// Read a signed 8-bit integer from the stream.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static sbyte ReadInt8(this Stream s) => unchecked((sbyte)s.ReadUInt8());

    /// <summary>
    /// Read an unsigned 8-bit integer from the stream.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static byte ReadUInt8(this Stream s)
    {
      byte ret;
      byte[] tmp = new byte[1];
      s.Read(tmp, 0, 1);
      ret = tmp[0];
      return ret;
    }

    /// <summary>
    /// Read an unsigned 16-bit little-endian integer from the stream.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static ushort ReadUInt16LE(this Stream s) => unchecked((ushort)s.ReadInt16LE());

    /// <summary>
    /// Read a signed 16-bit little-endian integer from the stream.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static short ReadInt16LE(this Stream s)
    {
      int ret;
      byte[] tmp = new byte[2];
      s.Read(tmp, 0, 2);
      ret = tmp[0] & 0x00FF;
      ret |= (tmp[1] << 8) & 0xFF00;
      return (short)ret;
    }

    public static void WriteInt16LE(this Stream s, short i)
    {
      s.WriteUInt16LE(unchecked((ushort)i));
    }

    public static void WriteUInt16LE(this Stream s, ushort i)
    {
      byte[] tmp = new byte[2];
      tmp[0] = (byte)(i & 0xFF);
      tmp[1] = (byte)((i >> 8) & 0xFF);
      s.Write(tmp, 0, 2);
    }

    /// <summary>
    /// Read an unsigned 16-bit Big-endian integer from the stream.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static ushort ReadUInt16BE(this Stream s) => unchecked((ushort)s.ReadInt16BE());

    /// <summary>
    /// Read a signed 16-bit Big-endian integer from the stream.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static short ReadInt16BE(this Stream s)
    {
      int ret;
      byte[] tmp = new byte[2];
      s.Read(tmp, 0, 2);
      ret = (tmp[0] << 8) & 0xFF00;
      ret |= tmp[1] & 0x00FF;
      return (short)ret;
    }

    public static void WriteUInt24LE(this Stream s, uint i)
    {
      byte[] tmp = new byte[3];
      tmp[0] = (byte)(i & 0xFF);
      tmp[1] = (byte)((i >> 8) & 0xFF);
      tmp[2] = (byte)((i >> 16) & 0xFF);
      s.Write(tmp, 0, 3);
    }
    public static void WriteUInt24BE(this Stream s, uint i)
    {
      byte[] tmp = new byte[3];
      tmp[2] = (byte)(i & 0xFF);
      tmp[1] = (byte)((i >> 8) & 0xFF);
      tmp[0] = (byte)((i >> 16) & 0xFF);
      s.Write(tmp, 0, 3);
    }

    /// <summary>
    /// Read an unsigned 24-bit little-endian integer from the stream.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static uint ReadUInt24LE(this Stream s)
    {
      int ret;
      byte[] tmp = new byte[3];
      s.Read(tmp, 0, 3);
      ret = tmp[0] & 0x0000FF;
      ret |= (tmp[1] << 8) & 0x00FF00;
      ret |= (tmp[2] << 16) & 0xFF0000;
      return unchecked((uint)ret);
    }

    /// <summary>
    /// Read a signed 24-bit little-endian integer from the stream.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static int ReadInt24LE(this Stream s)
    {
      int ret;
      byte[] tmp = new byte[3];
      s.Read(tmp, 0, 3);
      ret = tmp[0] & 0x0000FF;
      ret |= (tmp[1] << 8) & 0x00FF00;
      ret |= (tmp[2] << 16) & 0xFF0000;
      if ((tmp[2] & 0x80) == 0x80)
      {
        ret |= 0xFF << 24;
      }
      return ret;
    }

    /// <summary>
    /// Read an unsigned 24-bit Big-endian integer from the stream.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static uint ReadUInt24BE(this Stream s)
    {
      int ret;
      byte[] tmp = new byte[3];
      s.Read(tmp, 0, 3);
      ret = tmp[2] & 0x0000FF;
      ret |= (tmp[1] << 8) & 0x00FF00;
      ret |= (tmp[0] << 16) & 0xFF0000;
      return (uint)ret;
    }

    /// <summary>
    /// Read a signed 24-bit Big-endian integer from the stream.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static int ReadInt24BE(this Stream s)
    {
      int ret;
      byte[] tmp = new byte[3];
      s.Read(tmp, 0, 3);
      ret = tmp[2] & 0x0000FF;
      ret |= (tmp[1] << 8) & 0x00FF00;
      ret |= (tmp[0] << 16) & 0xFF0000;
      if ((tmp[0] & 0x80) == 0x80)
      {
        ret |= 0xFF << 24; // sign-extend
      }
      return ret;
    }

    /// <summary>
    /// Read an unsigned 32-bit little-endian integer from the stream.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static uint ReadUInt32LE(this Stream s) => unchecked((uint)s.ReadInt32LE());

    /// <summary>
    /// Read a signed 32-bit little-endian integer from the stream.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static int ReadInt32LE(this Stream s)
    {
      int ret;
      byte[] tmp = new byte[4];
      s.Read(tmp, 0, 4);
      ret = tmp[0] & 0x000000FF;
      ret |= (tmp[1] << 8) & 0x0000FF00;
      ret |= (tmp[2] << 16) & 0x00FF0000;
      ret |= (tmp[3] << 24);
      return ret;
    }

    public static void WriteInt32LE(this Stream s, int i)
    {
      s.WriteUInt32LE(unchecked((uint)i));
    }

    public static void WriteUInt32LE(this Stream s, uint i)
    {
      byte[] tmp = new byte[4];
      tmp[0] = (byte)(i & 0xFF);
      tmp[1] = (byte)((i >> 8) & 0xFF);
      tmp[2] = (byte)((i >> 16) & 0xFF);
      tmp[3] = (byte)((i >> 24) & 0xFF);
      s.Write(tmp, 0, 4);
    }

    /// <summary>
    /// Read an unsigned 32-bit Big-endian integer from the stream.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static uint ReadUInt32BE(this Stream s) => unchecked((uint)s.ReadInt32BE());

    /// <summary>
    /// Read a signed 32-bit Big-endian integer from the stream.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static int ReadInt32BE(this Stream s)
    {
      int ret;
      byte[] tmp = new byte[4];
      s.Read(tmp, 0, 4);
      ret = (tmp[0] << 24);
      ret |= (tmp[1] << 16) & 0x00FF0000;
      ret |= (tmp[2] << 8) & 0x0000FF00;
      ret |= tmp[3] & 0x000000FF;
      return ret;
    }

    /// <summary>
    /// Read an unsigned 64-bit little-endian integer from the stream.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static ulong ReadUInt64LE(this Stream s) => unchecked((ulong)s.ReadInt64LE());

    /// <summary>
    /// Read a signed 64-bit little-endian integer from the stream.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static long ReadInt64LE(this Stream s)
    {
      long ret;
      byte[] tmp = new byte[8];
      s.Read(tmp, 0, 8);
      ret = tmp[4] & 0x000000FFL;
      ret |= (tmp[5] << 8) & 0x0000FF00L;
      ret |= (tmp[6] << 16) & 0x00FF0000L;
      ret |= (tmp[7] << 24) & 0xFF000000L;
      ret <<= 32;
      ret |= tmp[0] & 0x000000FFL;
      ret |= (tmp[1] << 8) & 0x0000FF00L;
      ret |= (tmp[2] << 16) & 0x00FF0000L;
      ret |= (tmp[3] << 24) & 0xFF000000L;
      return ret;
    }

    public static void WriteInt64LE(this Stream s, long i)
    {
      s.WriteUInt64LE(unchecked((ulong)i));
    }

    public static void WriteUInt64LE(this Stream s, ulong i)
    {
      byte[] tmp = new byte[8];
      tmp[0] = (byte)(i & 0xFF);
      tmp[1] = (byte)((i >> 8) & 0xFF);
      tmp[2] = (byte)((i >> 16) & 0xFF);
      tmp[3] = (byte)((i >> 24) & 0xFF);
      i >>= 32;
      tmp[4] = (byte)(i & 0xFF);
      tmp[5] = (byte)((i >> 8) & 0xFF);
      tmp[6] = (byte)((i >> 16) & 0xFF);
      tmp[7] = (byte)((i >> 24) & 0xFF);
      s.Write(tmp, 0, 8);
    }

    /// <summary>
    /// Read an unsigned 64-bit big-endian integer from the stream.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static ulong ReadUInt64BE(this Stream s) => unchecked((ulong)s.ReadInt64BE());

    /// <summary>
    /// Read a signed 64-bit big-endian integer from the stream.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static long ReadInt64BE(this Stream s)
    {
      long ret;
      byte[] tmp = new byte[8];
      s.Read(tmp, 0, 8);
      ret = tmp[3] & 0x000000FFL;
      ret |= (tmp[2] << 8) & 0x0000FF00L;
      ret |= (tmp[1] << 16) & 0x00FF0000L;
      ret |= (tmp[0] << 24) & 0xFF000000L;
      ret <<= 32;
      ret |= tmp[7] & 0x000000FFL;
      ret |= (tmp[6] << 8) & 0x0000FF00L;
      ret |= (tmp[5] << 16) & 0x00FF0000L;
      ret |= (tmp[4] << 24) & 0xFF000000L;
      return ret;
    }

    /// <summary>
    /// Reads a multibyte value of the specified length from the stream.
    /// </summary>
    /// <param name="s">The stream</param>
    /// <param name="bytes">Must be less than or equal to 8</param>
    /// <returns></returns>
    public static long ReadMultibyteBE(this Stream s, byte bytes)
    {
      if (bytes > 8) return 0;
      long ret = 0;
      var b = s.ReadBytes(bytes);
      for (uint i = 0; i < b.Length; i++)
      {
        ret <<= 8;
        ret |= b[i];
      }
      return ret;
    }

    /// <summary>
    /// Read a single-precision (4-byte) floating-point value from the stream.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static float ReadFloat(this Stream s)
    {
      byte[] tmp = new byte[4];
      s.Read(tmp, 0, 4);
      return BitConverter.ToSingle(tmp, 0);
    }

    /// <summary>
    /// Read a half-precision (2-byte) floating point value from the stream.
    /// Return value is aliased to a single precision float because C# does not support half floats.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static float ReadHalfFloat(this Stream s) => ParseHalfFloat(s.ReadUInt16LE());

    unsafe public static float ParseHalfFloat(int half)
    {
      int sign = half >> 15;
      int exponent = ((half >> 10) & 0x1F);
      int mantissa = half & 0x03FF;
      int single;
      if (exponent == 0)
      {
        // Subnormal
        if (mantissa == 0) return 0;
        int exp = -15;
        int mask = 0x3FF;
        // Find the first leading 1.
        while (mantissa == (mantissa & mask))
        {
          mask >>= 1;
          exp--;
        }
        // AND the mantissa with the mask because the SP float is *not* subnormal and has an implied "1."
        single = (sign << 31) | (((128 + exp) & 0xFF) << 23) | ((mantissa & mask) << (30 + exp));
      }
      else
      {
        single = exponent == 31 ?
          // Infinity
          (mantissa == 0 ? (sign << 31) | (0xFF << 23)
          // NaN
          : (sign << 31) | (0xFF << 23) | 1)
          // Normal
          : (sign << 31) | (((exponent + 112) & 0xFF) << 23) | (mantissa << 13);
      }
      // TODO: Any other option besides unsafe code or allocating an unnecessary byte array?
      return *(float*)(&single);
      /* Eek, unsafe, but BitConverter.ToSingle is also unsafe according to reference source,
         and it requires allocating another byte array and multiple method calls... */
      // return BitConverter.ToSingle(BitConverter.GetBytes(single), 0);
    }


    /// <summary>
    /// Read a null-terminated ASCII string from the given stream.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string ReadASCIINullTerminated(this Stream s, int limit = -1)
    {
      StringBuilder sb = new StringBuilder(255);
      char cur;
      while ((limit == -1 || sb.Length < limit) && (cur = (char)s.ReadByte()) != 0)
      {
        sb.Append(cur);
      }
      return sb.ToString();
    }

    /// <summary>
    /// Read a null-terminated ASCII string from the given stream.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string ReadFixedLengthNullTerminatedString(this Stream s, int length)
    {
      StringBuilder sb = new StringBuilder(length);
      char cur;
      while (--length > 0 && (cur = (char)s.ReadByte()) != 0)
      {
        sb.Append(cur);
      }
      s.Position += length;
      return sb.ToString();
    }

    /// <summary>
    /// Read a length-prefixed string of the specified encoding type from the file.
    /// The length is a 32-bit little endian integer.
    /// </summary>
    /// <param name="s"></param>
    /// <param name="e">The encoding to use to decode the string.</param>
    /// <returns></returns>
    public static string ReadLengthPrefixedString(this Stream s, Encoding e, bool bigEdn = false)
    {
      int length = bigEdn ? s.ReadInt32BE() : s.ReadInt32LE();
      byte[] chars = new byte[length];
      s.Read(chars, 0, length);
      return e.GetString(chars);
    }

    /// <summary>
    /// Read a length-prefixed UTF-8 string from the given stream.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string ReadLengthUTF8(this Stream s, bool bigEdn = false)
    {
      return s.ReadLengthPrefixedString(Encoding.UTF8, bigEdn);
    }

    /// <summary>
    /// Read a given number of bytes from a stream into a new byte array.
    /// </summary>
    /// <param name="s"></param>
    /// <param name="count">Number of bytes to read (maximum)</param>
    /// <returns>New byte array of size &lt;=count.</returns>
    public static byte[] ReadBytes(this Stream s, int count)
    {
      // Size of returned array at most count, at least difference between position and length.
      int realCount = (int)((s.Position + count > s.Length) ? (s.Length - s.Position) : count);
      byte[] ret = new byte[realCount];
      s.Read(ret, 0, realCount);
      return ret;
    }

    /// <summary>
    /// Read a variable-length integral value as found in MIDI messages.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static uint ReadMidiMultiByte(this Stream s)
    {
      int ret = 0;
      byte b = (byte)(s.ReadByte());
      ret += b & 0x7f;
      if (0x80 == (b & 0x80))
      {
        ret <<= 7;
        b = (byte)(s.ReadByte());
        ret += b & 0x7f;
        if (0x80 == (b & 0x80))
        {
          ret <<= 7;
          b = (byte)(s.ReadByte());
          ret += b & 0x7f;
          if (0x80 == (b & 0x80))
          {
            ret <<= 7;
            b = (byte)(s.ReadByte());
            ret += b & 0x7f;
            if (0x80 == (b & 0x80))
              throw new InvalidDataException("Variable-length MIDI number > 4 bytes");
          }
        }
      }
      return (uint)ret;
    }

    public static void WriteMidiMultiByte(this Stream s, uint i)
    {
      if (i > 0x7FU)
      {
        int max = 7;
        while ((i >> max) > 0x7FU) max += 7;
        while (max > 0)
        {
          s.WriteByte((byte)(((i >> max) & 0x7FU) | 0x80));
          max -= 7;
        }
      }
      s.WriteByte((byte)(i & 0x7FU));
    }

    public static void WriteLE(this Stream s, ushort i) => s.WriteUInt16LE(i);
    public static void WriteLE(this Stream s, uint i) => s.WriteUInt32LE(i);
    public static void WriteLE(this Stream s, ulong i) => s.WriteUInt64LE(i);
    public static void WriteLE(this Stream s, short i) => s.WriteInt16LE(i);
    public static void WriteLE(this Stream s, int i) => s.WriteInt32LE(i);
    public static void WriteLE(this Stream s, long i) => s.WriteInt64LE(i);
    public static ushort FlipEndian(this ushort i) => (ushort)((i & 0xFFU) << 8 | (i & 0xFF00U) >> 8);
    public static uint FlipEndian(this uint i) => (i & 0x000000FFU) << 24 | (i & 0x0000FF00U) << 8 |
                                                  (i & 0x00FF0000U) >> 8 | (i & 0xFF000000U) >> 24;
    public static ulong FlipEndian(this ulong i) => (i & 0x00000000000000FFUL) << 56 | (i & 0x000000000000FF00UL) << 40 |
                                                     (i & 0x0000000000FF0000UL) << 24 | (i & 0x00000000FF000000UL) << 8 |
                                                     (i & 0x000000FF00000000UL) >> 8 | (i & 0x0000FF0000000000UL) >> 24 |
                                                     (i & 0x00FF000000000000UL) >> 40 | (i & 0xFF00000000000000UL) >> 56;
    public static short FlipEndian(this short i) => unchecked((short)((ushort)i).FlipEndian());
    public static int FlipEndian(this int i) => unchecked((int)((uint)i).FlipEndian());
    public static long FlipEndian(this long i) => unchecked((long)((ulong)i).FlipEndian());
    public static void WriteBE(this Stream s, ushort i) => s.WriteUInt16LE(i.FlipEndian());
    public static void WriteBE(this Stream s, uint i) => s.WriteUInt32LE(i.FlipEndian());
    public static void WriteBE(this Stream s, ulong i) => s.WriteUInt64LE(i.FlipEndian());
    public static void WriteBE(this Stream s, short i) => s.WriteInt16LE(i.FlipEndian());
    public static void WriteBE(this Stream s, int i) => s.WriteInt32LE(i.FlipEndian());
    public static void WriteBE(this Stream s, long i) => s.WriteInt64LE(i.FlipEndian());

  }
}
