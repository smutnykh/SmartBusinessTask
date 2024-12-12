using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer
{
	public class Facility
	{
		[Key]
		public string Code {  get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public int StandartArea { get; set; }

		public List<EquipmentType> EquipmentTypes { get; } = new();
		public List<Contract> Contracts { get; } = new();
	}
}
