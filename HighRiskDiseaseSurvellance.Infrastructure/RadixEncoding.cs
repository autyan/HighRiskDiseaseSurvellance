using System;
using System.Collections.Generic;
using System.Numerics;

namespace HighRiskDiseaseSurvellance.Infrastructure
{
    public class RadixEncoding
  {
    private const int kByteBitCount = 8;
    private readonly double kBitsPerDigit;
    private readonly string kDigits;
    private readonly BigInteger kRadixBig;

    /// <summary>Create a radix encoder using the given characters as the digits in the radix</summary>
    /// <param name="digits">Digits to use for the radix-encoded string</param>
    /// <param name="bytesEndian">Endian ordering of bytes input to Encode and output by Decode</param>
    /// <param name="includeProceedingZeros">True if we want ending zero bytes to be encoded</param>
    public RadixEncoding(string digits, EndianFormat bytesEndian = EndianFormat.Little, bool includeProceedingZeros = false)
    {
      int length = digits.Length;
      this.kDigits = digits;
      this.kBitsPerDigit = Math.Log((double) length, 2.0);
      this.kRadixBig = new BigInteger(length);
      this.Endian = bytesEndian;
      this.IncludeProceedingZeros = includeProceedingZeros;
    }

    /// <summary>Numerial base of this encoding</summary>
    public int Radix => this.kDigits.Length;

    /// <summary>Endian ordering of bytes input to Encode and output by Decode</summary>
    public EndianFormat Endian { get; }

    /// <summary>True if we want ending zero bytes to be encoded</summary>
    public bool IncludeProceedingZeros { get; }

    public override string ToString() => string.Format("Base-{0} {1}", (object) this.Radix, (object) this.kDigits);

    private int EncodingCharsCount(int bytesLength) => (int) Math.Ceiling((double) (bytesLength * 8) / this.kBitsPerDigit);

    private int DecodingBytesCount(int charsCount) => (int) Math.Ceiling((double) charsCount * this.kBitsPerDigit / 8.0);

    /// <summary>Encode a byte array into a radix-encoded string</summary>
    /// <param name="bytes">byte array to encode</param>
    /// <returns>The bytes in encoded into a radix-encoded string</returns>
    /// <remarks>If <paramref name="bytes" /> is zero length, returns an empty string</remarks>
    public string Encode(byte[] bytes)
    {
      if (bytes.Length == 0)
        return string.Empty;
      List<char> charList = new List<char>(this.EncodingCharsCount(bytes.Length));
      if (((int) bytes[bytes.Length - 1] & 128) == 128)
        Array.Resize<byte>(ref bytes, bytes.Length + 1);
      BigInteger dividend = new BigInteger(bytes);
      while (!dividend.IsZero)
      {
        BigInteger remainder;
        dividend = BigInteger.DivRem(dividend, this.kRadixBig, out remainder);
        int index = Math.Abs((int) remainder);
        charList.Add(this.kDigits[index]);
      }
      if (this.IncludeProceedingZeros)
      {
        for (int count = charList.Count; count < charList.Capacity; ++count)
          charList.Add(this.kDigits[0]);
      }
      if (this.Endian == EndianFormat.Little)
        charList.Reverse();
      return new string(charList.ToArray());
    }

    private void DecodeImplPadResult(ref byte[] result, int padCount)
    {
      if (padCount <= 0)
        return;
      int newSize = result.Length + this.DecodingBytesCount(padCount);
      Array.Resize<byte>(ref result, newSize);
    }

    /// <summary>Decode a radix-encoded string into a byte array</summary>
    /// <param name="radixChars">radix string</param>
    /// <returns>The decoded bytes, or null if an invalid character is encountered</returns>
    /// <remarks>
    ///     If <paramref name="radixChars" /> is an empty string, returns a zero length array
    ///     Using <paramref>
    ///         <name>IncludeProceedingZeros</name>
    ///     </paramref>
    ///     has the potential to return a buffer with an
    ///     additional zero byte that wasn't in the input. So a 4 byte buffer was encoded, this could end up
    ///     returning a 5 byte buffer, with the extra byte being null.
    /// </remarks>
    public byte[] Decode(string radixChars) => this.Endian == EndianFormat.Big ? (!this.IncludeProceedingZeros ? this.DecodeImplReversed(radixChars) : this.DecodeImplReversedWithPadding(radixChars)) : (!this.IncludeProceedingZeros ? this.DecodeImpl(radixChars) : this.DecodeImplWithPadding(radixChars));

    private byte[] DecodeImpl(string chars, int startIndex = 0)
    {
      BigInteger bigInteger = new BigInteger();
      for (int index = startIndex; index < chars.Length; ++index)
      {
        int num = this.kDigits.IndexOf(chars[index]);
        if (num < 0)
          return (byte[]) null;
        bigInteger = bigInteger * this.kRadixBig + (BigInteger) num;
      }
      return bigInteger.ToByteArray();
    }

    private byte[] DecodeImplWithPadding(string chars)
    {
      int num = 0;
      int index = 0;
      while (index < chars.Length && (int) chars[index] == (int) this.kDigits[0])
      {
        ++index;
        ++num;
      }
      byte[] result = this.DecodeImpl(chars, num);
      this.DecodeImplPadResult(ref result, num);
      return result;
    }

    private byte[] DecodeImplReversed(string chars, int startIndex = 0)
    {
      BigInteger bigInteger = new BigInteger();
      for (int index = chars.Length - 1 - startIndex; index >= 0; --index)
      {
        int num = this.kDigits.IndexOf(chars[index]);
        if (num < 0)
          return (byte[]) null;
        bigInteger = bigInteger * this.kRadixBig + (BigInteger) num;
      }
      return bigInteger.ToByteArray();
    }

    private byte[] DecodeImplReversedWithPadding(string chars)
    {
      int num = 0;
      int index = chars.Length - 1;
      while (index >= 0 && (int) chars[index] == (int) this.kDigits[0])
      {
        --index;
        ++num;
      }
      byte[] result = this.DecodeImplReversed(chars, num);
      this.DecodeImplPadResult(ref result, num);
      return result;
    }
  }
}
