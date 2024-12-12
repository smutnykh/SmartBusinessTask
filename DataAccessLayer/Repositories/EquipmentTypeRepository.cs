using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Repositories
{
	public class EquipmentTypeRepository : Repository<EquipmentType>, IEquipmentTypeRepository
	{
		public EquipmentTypeRepository(ProductionDbContext dbContext) : base(dbContext)
		{
		}
	}
}
