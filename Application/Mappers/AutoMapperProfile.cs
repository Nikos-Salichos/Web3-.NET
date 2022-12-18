using AutoMapper;
using Domain.DTOs;
using Domain.Models;

namespace Application.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SmartContract, SmartContractDTO>();
        }
    }
}
