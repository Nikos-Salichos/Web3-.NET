using System.IO.Compression;

namespace WebApi.Extensions
{
    public static class CompressDecrompressExtension
    {
        public static byte[] Compress(byte[] bytes)
        {
            using var memoryStream = new MemoryStream();
            using (var brotliStream = new BrotliStream(memoryStream, CompressionLevel.Optimal))
            {
                brotliStream.Write(bytes, 0, bytes.Length);
            }
            return memoryStream.ToArray();
        }

        public static byte[] Decompress(byte[] bytes)
        {
            using var memoryStream = new MemoryStream(bytes);
        }

    }
}
