using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Hashing;
using System.Buffers.Binary;

namespace DeadzoneTechEditor
{
    public static class ChecksumService
    {
        private const int ChecksumOffset = 0x720;
        private const int ChecksumDataOffset = 0x724;
        public static byte[] GetStoredChecksum(byte[] saveByteData)
        {

            byte[] checkSum = 
                [
                    saveByteData[ChecksumOffset], 
                    saveByteData[ChecksumOffset + 1], 
                    saveByteData[ChecksumOffset + 2], 
                    saveByteData[ChecksumOffset + 3]
                ];

            return checkSum;
        }

        public static uint CalculateChecksum(byte[] saveByteData)
        {
            return Crc32.HashToUInt32(saveByteData.AsSpan(ChecksumDataOffset));
        }

        public static bool IsChecksumValid(byte[] saveByteData)
        {
            byte[] localChecksum = GetStoredChecksum(saveByteData);

            return (CalculateChecksum(saveByteData) == BinaryPrimitives.ReadUInt32LittleEndian(localChecksum));
        }

        public static void WriteChecksum(byte[] saveByteData)
        {
            uint checksum = CalculateChecksum(saveByteData);

            BinaryPrimitives.WriteUInt32LittleEndian(saveByteData.AsSpan(ChecksumOffset, 4), checksum);
        }
    }
}
