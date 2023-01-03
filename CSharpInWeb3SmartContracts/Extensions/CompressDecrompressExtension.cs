using System.IO.Compression;

namespace WebApi.Extensions
{
    public static class CompressDecrompressExtension
    {
        public static async Task<byte[]> CompressAsync(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var brotliStream = new BrotliStream(memoryStream, CompressionLevel.Optimal))
                {
                    await brotliStream.WriteAsync(bytes, 0, bytes.Length);
                }
                return memoryStream.ToArray();
            }
        }

        public static byte[] Decompress(byte[] bytes)
        {
            using var memoryStream = new MemoryStream(bytes);
            using (var outputStream = new MemoryStream())
            {
                using (var decompressStream = new BrotliStream(memoryStream, CompressionMode.Decompress))
                {
                    decompressStream.CopyTo(outputStream);
                }
                return outputStream.ToArray();
            }
        }

    }
}
