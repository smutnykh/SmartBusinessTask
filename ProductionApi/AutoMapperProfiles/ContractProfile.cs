using AutoMapper;
using DataAccessLayer;
using ProductionApi.DTOs;

namespace ProductionApi.AutoMapperProfiles
{
	public class ContractProfile : Profile
	{
		public ContractProfile()
		{
			CreateMap<CreateContractDTO, Contract>();
			CreateMap<Contract, ContractDTO>();
		}
	}
}
