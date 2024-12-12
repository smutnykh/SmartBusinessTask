using System.ComponentModel.DataAnnotations;

namespace ProductionApi.DTOs
{
	public class ContractDTO
	{
		[Required]
		public string Code { get; set; }

		[Required]
		public string FacilityCode { get; set; }

		[Required]
		public string EquipmentTypeCode { get; set; }

		[Required]
		public int Amount { get; set; }
	}
}
