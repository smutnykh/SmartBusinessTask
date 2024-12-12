using DataAccessLayer;
using DataAccessLayer.Interfaces;

namespace ProductionApi.Services
{
	public class ContractService
	{
		private readonly IFacilityRepository _facilityRepository;
		private readonly IEquipmentTypeRepository _equipmentTypeRepository;
		private readonly IContractRepository _contractRepository;

		private readonly BackgroundProcessingService _backgroundService;

		private readonly ILogger<ContractService> _logger;

		public ContractService(
			IContractRepository contractRepository, 
			IFacilityRepository facilityRepository,
			IEquipmentTypeRepository equipmentTypeRepository,
			BackgroundProcessingService backgroundProcessingService,
			ILogger<ContractService> logger)
		{
			_contractRepository = contractRepository;
			_facilityRepository = facilityRepository;
			_equipmentTypeRepository = equipmentTypeRepository;
			_backgroundService = backgroundProcessingService;
			_logger = logger;
		}
		
		public async Task<IList<Contract>> GetAll()
		{
			return await _contractRepository.GetAllAsync();
		}

		public async Task AddContract(Contract contract)
		{
			// Retrieve production facility
			var facility = await _facilityRepository.GetByIdAsync(contract.FacilityCode);
			if (facility == null)
			{
				throw new InvalidOperationException("Production facility was not found.");
			}

			var equipmentType = await _equipmentTypeRepository.GetByIdAsync(contract.EquipmentTypeCode);
			if (equipmentType == null)
			{
				throw new InvalidOperationException("Equipment type was not found.");
			}

			var requiredArea = contract.Amount * equipmentType.Area;

			var occupiedArea = await _contractRepository.GetOccupiedAreaByFacilityCodeAsync(contract.FacilityCode);

			if (facility.StandartArea - occupiedArea < requiredArea)
			{
				throw new InvalidOperationException("Not enough space in the production facility for the equipment.");
			}

			await _contractRepository.SaveAsync(contract);

			_backgroundService.QueueBackgroundWorkItem(async token =>
			{
				_logger.LogInformation($"Starting background processing for contract: {contract.Code}");
				await Task.Delay(5000, token); // Simulate background processing
				_logger.LogInformation($"Completed background processing for contract: {contract.Code}");
			});
		}

	}
}
