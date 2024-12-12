using System.ComponentModel.DataAnnotations;

namespace ProductionApi.DTOs
{
	public class CreateContractDTO
	{
		[Required]
		public string FacilityCode { get; set; }

		[Required]
		public string EquipmentTypeCode { get; set; }

		[Required]
		public int Amount { get; set; }
	}
}
