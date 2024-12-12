using DataAccessLayer;
using DataAccessLayer.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using ProductionApi.Services;

namespace ProductionApi.Tests
{
	public class Tests
	{
		private Mock<IContractRepository> _contractRepositoryMock;
		private Mock<IFacilityRepository> _facilityRepositoryMock;
		private Mock<IEquipmentTypeRepository> _equipmentTypeRepositoryMock;
		private Mock<ILogger<BackgroundProcessingService>> _backgroundServiceLoggerMock;
		private Mock<ILogger<ContractService>> _loggerMock;
		private Mock<BackgroundProcessingService> _backgroundServiceMock;
		private ContractService _service;

		private const string FACILITY_NOT_FOUND_ERROR_MESSAGE = "Production facility was not found.";
		private const string EQUIPMENT_TYPE_NOT_FOUND_ERROR_MESSAGE = "Equipment type was not found.";
		private const string NOT_ENOUGH_SPACE_ERROR_MESSAGE = "Not enough space in the production facility for the equipment.";


		[SetUp]
		public void Setup()
		{
			_contractRepositoryMock = new Mock<IContractRepository>();
			_facilityRepositoryMock = new Mock<IFacilityRepository>();
			_equipmentTypeRepositoryMock = new Mock<IEquipmentTypeRepository>();
			_backgroundServiceLoggerMock = new Mock<ILogger<BackgroundProcessingService>>(); // Mock the logger for BackgroundProcessingService
			_loggerMock = new Mock<ILogger<ContractService>>();
			_backgroundServiceMock = new Mock<BackgroundProcessingService>(_backgroundServiceLoggerMock.Object);

			_service = new ContractService(
				_contractRepositoryMock.Object,
				_facilityRepositoryMock.Object,
				_equipmentTypeRepositoryMock.Object,
				_backgroundServiceMock.Object,
				_loggerMock.Object);
		}

		[Test]
		public async Task AddContract_ValidContract_SavesContractAndQueuesBackgroundTask()
		{
			// Arrange
			var contract = new Contract
			{
				Code = "con1",
				FacilityCode = "fac1",
				EquipmentTypeCode = "eqp1",
				Amount = 5
			};

			var facility = new Facility
			{
				Code = "fac1",
				Name = "facility1",
				StandartArea = 100
			};

			var equipmentType = new EquipmentType
			{
				Code = "eqp1",
				Area = 10
			};

			_facilityRepositoryMock
				.Setup(repo => repo.GetByIdAsync(contract.FacilityCode))
				.ReturnsAsync(facility);

			_equipmentTypeRepositoryMock
				.Setup(repo => repo.GetByIdAsync(contract.EquipmentTypeCode))
				.ReturnsAsync(equipmentType);

			_contractRepositoryMock
				.Setup(repo => repo.GetOccupiedAreaByFacilityCodeAsync(contract.FacilityCode))
				.ReturnsAsync(30);

			// Act
			await _service.AddContract(contract);

			// Assert
			_contractRepositoryMock.Verify(repo => repo.SaveAsync(It.Is<Contract>(c =>
				c.FacilityCode == contract.FacilityCode &&
				c.EquipmentTypeCode == contract.EquipmentTypeCode &&
				c.Amount == contract.Amount &&
				c.Code == contract.Code)), Times.Once);
		}

		[Test]
		public void AddContract_FacilityNotFound_ThrowsInvalidOperationException()
		{
			// Arrange
			var contract = new Contract { FacilityCode = "fac1" };

			_facilityRepositoryMock
				.Setup(repo => repo.GetByIdAsync(contract.FacilityCode))
				.ReturnsAsync((Facility)null);

			var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
				_service.AddContract(contract));

			Assert.That(ex.Message, Is.EqualTo(FACILITY_NOT_FOUND_ERROR_MESSAGE));
		}

		[Test]
		public void AddContract_EquipmentTypeNotFound_ThrowsInvalidOperationException()
		{
			var contract = new Contract
			{
				FacilityCode = "fac1",
				EquipmentTypeCode = "eqp1"
			};

			var facility = new Facility { Code = "fac1", StandartArea = 100 };

			_facilityRepositoryMock
				.Setup(repo => repo.GetByIdAsync(contract.FacilityCode))
				.ReturnsAsync(facility);

			_equipmentTypeRepositoryMock
				.Setup(repo => repo.GetByIdAsync(contract.EquipmentTypeCode))
				.ReturnsAsync((EquipmentType)null);

			var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
				_service.AddContract(contract));

			Assert.That(ex.Message, Is.EqualTo(EQUIPMENT_TYPE_NOT_FOUND_ERROR_MESSAGE));
		}

		[Test]
		public void AddContract_NotEnoughSpace_ThrowsInvalidOperationException()
		{
			var contract = new Contract
			{
				FacilityCode = "fac1",
				EquipmentTypeCode = "eqp1",
				Amount = 10
			};

			var facility = new Facility
			{
				Code = "fac1",
				StandartArea = 100
			};

			var equipmentType = new EquipmentType
			{
				Code = "eqp1",
				Area = 10
			};

			_facilityRepositoryMock
				.Setup(repo => repo.GetByIdAsync(contract.FacilityCode))
				.ReturnsAsync(facility);

			_equipmentTypeRepositoryMock
				.Setup(repo => repo.GetByIdAsync(contract.EquipmentTypeCode))
				.ReturnsAsync(equipmentType);

			_contractRepositoryMock
				.Setup(repo => repo.GetOccupiedAreaByFacilityCodeAsync(contract.FacilityCode))
				.ReturnsAsync(50);

			var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
				_service.AddContract(contract));

			Assert.That(ex.Message, Is.EqualTo(NOT_ENOUGH_SPACE_ERROR_MESSAGE));
		}
	}
}