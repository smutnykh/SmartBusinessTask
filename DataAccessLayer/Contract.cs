using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer
{
	public class Contract
	{
		[Key]
		public string Code { get; set; }

		[Required]
		public string FacilityCode { get; set; }

		[Required]
		public string EquipmentTypeCode { get; set; }

		[Required]
		public int Amount { get; set; }

		public Facility Facility { get; set; } = null!;
		public EquipmentType EquipmentType { get; set; } = null!;
	}
}
