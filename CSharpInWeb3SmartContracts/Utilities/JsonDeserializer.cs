using Newtonsoft.Json;

namespace CSharpInWeb3SmartContracts.Utilities
{
    public static class JsonDeserializer<T>
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<T?> GetAndDeserializeGenericType(JsonSerializerSettings jsonSerializerSettings, string url)
        {
            try
            {
                HttpResponseMessage? response = await httpClient.GetAsync(new Uri(url));

                response.EnsureSuccessStatusCode();

                if (response is null)
                {
                    return default;
                }

                string? jsonContent = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrWhiteSpace(jsonContent))
                {
                    return JsonConvert.DeserializeObject<T>(jsonContent, jsonSerializerSettings);
                }
                return default;
            }
            catch (UriFormatException)
            {
                throw;
            }
            catch (JsonReaderException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
