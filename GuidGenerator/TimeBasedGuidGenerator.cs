namespace GuidGenerator;
public static class TimeBasedGuidGenerator
{
    private static readonly Random Random;

    private const int ByteArraySize = 16;
    private const byte GuidClockSequenceByte = 8;
    private const byte NodeByte = 10;

    static TimeBasedGuidGenerator()
    {
        Random = new Random();
    }

    public static Guid GenerateTimeBasedGuid(DateTime dateTime)
    {
        long ticks = dateTime.Ticks - DateTime.MinValue.Ticks;

        byte[] guid = new byte[ByteArraySize];
        byte[] clockSequenceBytes = BitConverter.GetBytes(Convert.ToInt16(Environment.TickCount % Int16.MaxValue));
        byte[] timestamp = BitConverter.GetBytes(ticks);
        var node = new byte[6];

        Random.NextBytes(node);

        // copy node
        Array.Copy(node, 0, guid, NodeByte, node.Length);

        // copy clock sequence
        Array.Copy(clockSequenceBytes, 0, guid, GuidClockSequenceByte, clockSequenceBytes.Length);

        // copy timestamp
        Array.Copy(timestamp, 0, guid, 0, timestamp.Length);

        return new Guid(guid);
    }

    public static DateTime GetUtcDateTime(Guid guid)
    {
        return GetDateTimeOffset(guid).UtcDateTime;
    }

    private static DateTimeOffset GetDateTimeOffset(Guid guid)
    {
        byte[] bytes = guid.ToByteArray();


        byte[] timestampBytes = new byte[8];
        Array.Copy(bytes, 0, timestampBytes, 0, 8);

        long timestamp = BitConverter.ToInt64(timestampBytes, 0);
        long ticks = timestamp + DateTime.MinValue.Ticks;

        return new DateTimeOffset(ticks, TimeSpan.Zero);
    }

    /// <summary>
    /// Generates a random value for the node.
    /// </summary>
    /// <returns></returns>
    private static byte[] GenerateNodeBytes()
    {
        var node = new byte[6];

        Random.NextBytes(node);
        return node;
    }

    /// <summary>
    /// Generates a random clock sequence.
    /// </summary>
    private static byte[] GenerateClockSequenceBytes()
    {
        var bytes = new byte[2];
        Random.NextBytes(bytes);
        return bytes;
    }
}
