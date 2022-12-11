using System.Text.Json;

namespace Domain.Models
{
    public class ErrorDetail
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ErrorDetail(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
