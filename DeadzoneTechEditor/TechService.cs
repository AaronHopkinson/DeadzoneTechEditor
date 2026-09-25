using System.Buffers.Binary;

namespace DeadzoneTechEditor
{
    public static class TechService
    {
        // Serialized inventory ID marker shared by both tested saves.
        // The last 16 bytes identify the entry; the value follows as IntProperty.
        private static readonly byte[] TechInventoryId =
            [0x08, 0xfa, 0x10, 0xf5, 0x16, 0x64, 0x52, 0xb9,
             0x4d, 0x91, 0xbc, 0xb3, 0xf8, 0x68, 0x63, 0x6b, 0x91];

        private static readonly byte[] ValueHeader =
            [0x06, 0, 0, 0, (byte)'V', (byte)'a', (byte)'l', (byte)'u', (byte)'e', 0,
             0x0c, 0, 0, 0, (byte)'I', (byte)'n', (byte)'t', (byte)'P', (byte)'r',
             (byte)'o', (byte)'p', (byte)'e', (byte)'r', (byte)'t', (byte)'y', 0,
             0, 0, 0, 0, 0x04, 0, 0, 0, 0];

        private static int FindTechOffset(byte[] data)
        {
            int found = -1;
            int search = 0;
            while (search <= data.Length - TechInventoryId.Length)
            {
                int relative = data.AsSpan(search).IndexOf(TechInventoryId);
                if (relative < 0) break;
                int guid = search + relative;
                int header = guid + TechInventoryId.Length;
                if (guid >= 4 &&
                    BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(guid - 4, 4)) == 16 &&
                    header <= data.Length - ValueHeader.Length - sizeof(int) &&
                    data.AsSpan(header, ValueHeader.Length).SequenceEqual(ValueHeader))
                {
                    if (found >= 0)
                        throw new InvalidDataException("Ambiguous Tech inventory entry.");
                    found = header + ValueHeader.Length;
                }
                search = guid + TechInventoryId.Length;
            }
            if (found < 0)
                throw new InvalidDataException("Unsupported save: Tech inventory entry missing.");
            return found;
        }

        public static int GetTech(byte[] data)
        {
            int tech = BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(FindTechOffset(data), 4));
            if (tech < 0) throw new InvalidDataException("Invalid Tech amount in save.");
            return tech;
        }

        public static void SetTech(byte[] data, long newTech)
        {
            if (newTech < 0 || newTech > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(newTech), "Tech must be between 0 and 2,147,483,647.");
            int offset = FindTechOffset(data);
            if (BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(offset, 4)) < 0)
                throw new InvalidDataException("Invalid Tech amount in save.");
            BinaryPrimitives.WriteInt32LittleEndian(data.AsSpan(offset, 4), (int)newTech);
        }
    }
}
