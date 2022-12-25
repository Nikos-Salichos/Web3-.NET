using Domain.Entities;
using Nethereum.Signer;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class SmartContract : IEntity
    {
        public string? Address { get; set; }
        public string? Bytecode { get; set; }
        public Chain Chain { get; set; } = new Chain();

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
        public List<object>? Parameters { get; set; }

        [Column("Parameters")]
        public string ParametersSerialized
        {
            get
            {
                return JsonConvert.SerializeObject(Parameters);
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Parameters = new List<object>();
                }
                else
                {
                    Parameters = JsonConvert.DeserializeObject<List<object>>(value);
                }
            }
        }



    }
}
