using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
	public class ProductionDbContext : DbContext
	{
		public ProductionDbContext(DbContextOptions<ProductionDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Facility>()
				.HasMany(e => e.EquipmentTypes)
				.WithMany(e => e.Facilities)
				.UsingEntity<Contract>(
					l => l.HasOne(e => e.EquipmentType).WithMany(e => e.Contracts).HasForeignKey(e => e.EquipmentTypeCode),
					r => r.HasOne(e => e.Facility).WithMany(e => e.Contracts).HasForeignKey(e => e.FacilityCode)
				);

			//Seeding initial test data
			//var facility1 = new Facility
			//{
			//	Code = "123456a",
			//	Name = "Facility1",
			//	StandartArea = 100
			//};

			//var facility2 = new Facility
			//{
			//	Code = "123456b",
			//	Name = "Facility2",
			//	StandartArea = 200
			//};

			//var type1 = new EquipmentType
			//{
			//	Code = "123456c",
			//	Name = "ToolBox",
			//	Area = 1
			//};

			//var type2 = new EquipmentType
			//{
			//	Code = "123456d",
			//	Name = "Vehicle",
			//	Area = 50
			//};

			//var contract = new Contract
			//{
			//	Code = "123456z",
			//	FacilityCode = "123456a",
			//	EquipmentTypeCode = "123456d",
			//	Amount = 2
			//};

			//modelBuilder.Entity<Facility>().HasData(
			//	facility1,
			//	facility2
			//);

			//modelBuilder.Entity<EquipmentType>().HasData(
			//	type1,
			//	type2
			//);

			//modelBuilder.Entity<Contract>().HasData(contract);

			base.OnModelCreating(modelBuilder);
		}
	}
}
