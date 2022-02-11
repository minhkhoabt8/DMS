namespace Content.Infrastructure.Extensions;

public static class StreamExtensions
{
    public static byte[] ToBytes(this Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);
        using var memStream = new MemoryStream();
        stream.CopyTo(memStream);
        stream.Seek(0, SeekOrigin.Begin);

        return memStream.ToArray();
    }
}