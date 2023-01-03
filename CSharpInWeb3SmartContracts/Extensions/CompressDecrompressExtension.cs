using System.IO.Compression;

namespace WebApi.Extensions
{
    public static class CompressDecrompressExtension
    {
        public static async Task<byte[]> CompressAsync(byte[] bytes)
        {
            using var memoryStream = new MemoryStream();
            using (var brotliStream = new BrotliStream(memoryStream, CompressionLevel.Optimal))
            {
                await brotliStream.WriteAsync(bytes, default);
            }
            return memoryStream.ToArray();
        }

        public static async Task<byte[]> DecompressAsync(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
            {
                using (var outputStream = new MemoryStream())
                {
                    using (var brotliStream = new BrotliStream(memoryStream, CompressionMode.Decompress))
                    {
                        await brotliStream.CopyToAsync(outputStream);
                    }
                    return outputStream.ToArray();
                }
            }
        }

    }
}
