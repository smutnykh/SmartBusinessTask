using AutoMapper;
using DataAccessLayer;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ProductionApi.DTOs;
using ProductionApi.Services;
using System;

namespace ProductionApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ContractController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ContractService _contractService;

		public ContractController(IMapper mapper, IContractRepository contractRepository, ContractService contractService)
		{
			_mapper = mapper;
			_contractService = contractService;
		}

		[HttpPost]
		[Route("create")]
		public async Task<IActionResult> Create(CreateContractDTO dto)
		{
			try
			{
				var contract = _mapper.Map<Contract>(dto);
				contract.Code = Guid.NewGuid().ToString();

				await _contractService.AddContract(contract);

				return Ok();
			} 
			catch (InvalidOperationException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet]
		[Route("getAll")]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				var contracts = await _contractService.GetAll();
				return Ok(_mapper.Map<IList<Contract>, IList<ContractDTO>>(contracts));
			}
			catch
			{
				return StatusCode(500);
			}
		}
	}
}
