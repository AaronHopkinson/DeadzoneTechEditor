using System.Buffers.Binary;
using System.Text;

namespace DeadzoneTechEditor
{
    public static class TechService
    {
        // These are serialized stat records, not absolute file positions. Other
        // game versions and player saves can move them within the payload.
        private static readonly byte[] ValueHeader =
            [0x05, 0, 0, 0, (byte)'N', (byte)'o', (byte)'n', (byte)'e', 0,
             0x06, 0, 0, 0, (byte)'V', (byte)'a', (byte)'l', (byte)'u', (byte)'e', 0,
             0x0e, 0, 0, 0, (byte)'I', (byte)'n', (byte)'t', (byte)'6', (byte)'4',
             (byte)'P', (byte)'r', (byte)'o', (byte)'p', (byte)'e', (byte)'r', (byte)'t', (byte)'y', 0,
             0, 0, 0, 0, 0x08, 0, 0, 0, 0];

        private static int FindValue(byte[] data, string stat)
        {
            byte[] tag = Encoding.ASCII.GetBytes(stat + "\0");
            int search = 0;
            while (search <= data.Length - tag.Length)
            {
                int index = data.AsSpan(search).IndexOf(tag);
                if (index < 0) break;
                index += search;
                int header = index + tag.Length;
                if (header <= data.Length - ValueHeader.Length - sizeof(long) &&
                    data.AsSpan(header, ValueHeader.Length).SequenceEqual(ValueHeader))
                {
                    // The first matching record belongs to PlayerPersistenceData.
                    // Later records contain separate session statistics.
                    return header + ValueHeader.Length;
                }
                search = index + tag.Length;
            }
            throw new InvalidDataException($"Unsupported save: {stat} record missing.");
        }

        private static (int gained, int spent) GetOffsets(byte[] data)
        {
            int gained = FindValue(data, "Stats.Saved.TechGained");
            int spent = FindValue(data, "Stats.Saved.TechSpent");
            if (gained == spent) throw new InvalidDataException("Overlapping Tech records.");
            return (gained, spent);
        }

        public static long GetTech(byte[] data)
        {
            var (gainedOffset, spentOffset) = GetOffsets(data);
            long gained = BinaryPrimitives.ReadInt64LittleEndian(data.AsSpan(gainedOffset, 8));
            long spent = BinaryPrimitives.ReadInt64LittleEndian(data.AsSpan(spentOffset, 8));
            if (gained < 0 || spent < 0 || spent > gained)
                throw new InvalidDataException("Invalid Tech totals in save.");
            return gained - spent;
        }

        public static void SetTech(byte[] data, long newTech)
        {
            if (newTech < 0) throw new ArgumentOutOfRangeException(nameof(newTech));
            var (gainedOffset, spentOffset) = GetOffsets(data);
            long spent = BinaryPrimitives.ReadInt64LittleEndian(data.AsSpan(spentOffset, 8));
            if (spent < 0) throw new InvalidDataException("Invalid Tech spent in save.");
            long gained = checked(spent + newTech);
            BinaryPrimitives.WriteInt64LittleEndian(data.AsSpan(gainedOffset, 8), gained);
        }
    }
}
