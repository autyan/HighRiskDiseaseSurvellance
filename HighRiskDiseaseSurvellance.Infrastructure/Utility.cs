using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighRiskDiseaseSurvellance.Infrastructure
{
    public static class Utility
    {
        private static readonly uint[] Lookup32 = Utility.CreateLookup32();

        public static string ToHexString(this byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof (bytes));
            uint[] lookup32 = Utility.Lookup32;
            char[] chArray  = new char[bytes.Length * 2];
            for (int index = 0; index < bytes.Length; ++index)
            {
                uint num = lookup32[(int) bytes[index]];
                chArray[2 * index]     = (char) num;
                chArray[2 * index + 1] = (char) (num >> 16);
            }
            return new string(chArray);
        }

        public static string ToBase36String(
            this byte[]  bytes,
            EndianFormat bytesEndian            = EndianFormat.Little,
            bool         includeProceedingZeros = true)
        {
            return new RadixEncoding("0123456789abcdefghijklmnopqrstuvwxyz", bytesEndian, includeProceedingZeros).Encode(bytes);
        }

        private static uint[] CreateLookup32()
        {
            uint[] lookup32 = new uint[256];
            for (int index = 0; index < 256; ++index)
            {
                string str = index.ToString("X2");
                lookup32[index] = (uint) str[0] + ((uint) str[1] << 16);
            }
            return lookup32;
        }

        public static string[] SplitStringRangeThree(this string source)
        {
            var store  = new List<string>();
            var len    = source.Length /3;
            var range0 = source[..len];
            store.Add(range0);
            var range1 = source[len..^len];
            store.Add(range1);
            var len2   = len *2 +1;
            var range2 = source[len2..];
            store.Add(range2);
            return store.ToArray();
        }

        public static string CreateRandomSalt(int size)
        {
            var rand  = System.Security.Cryptography.RandomNumberGenerator.Create();
            var bytes = new byte[size];
            rand.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
