using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class SmartContract
    {
        public string? Address { get; set; }
        public string? Bytecode { get; set; }

        [NotMapped]
        public object? Abi { get; set; }

        [Column("Abi")]
        public string AbiSerialized
        {
            get
            {
                return JsonConvert.SerializeObject(Abi);
            }
            set
            {
                Abi = string.IsNullOrEmpty(value)
                        ? new object()
                        : JsonConvert.DeserializeObject<object>(value);
            }
        }

        [NotMapped]
        public object? Parameters { get; set; }

        [Column("Parameters")]
        public string ParametersSerialized
        {
            get
            {
                return JsonConvert.SerializeObject(Parameters);
            }
            set
            {
                Parameters = string.IsNullOrEmpty(value)
                        ? new object()
                        : JsonConvert.DeserializeObject<object>(value);
            }
        }
    }
}
