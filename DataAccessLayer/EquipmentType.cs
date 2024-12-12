using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer
{
	public class EquipmentType
	{
		[Key]
		public string Code { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public int Area { get; set; }

		public List<Facility> Facilities { get; } = new();
		public List<Contract> Contracts { get; } = new();
	}
}
