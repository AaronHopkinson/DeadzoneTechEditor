using System;
using System.Collections.Generic;
using System.Text;
using System.Buffers.Binary;

namespace DeadzoneTechEditor
{
    public static class TechService
    {
        private const int TechOffset = 0x3D8D7;

        public static int GetTech(byte[] saveByteData)
        {
            return BinaryPrimitives.ReadInt32LittleEndian(saveByteData.AsSpan(TechOffset, 4));
        }

        public static void SetTech(byte[] saveByteData, int newTech)
        {
            BinaryPrimitives.WriteInt32LittleEndian(saveByteData.AsSpan(TechOffset, 4), newTech);
        }
    }
}
