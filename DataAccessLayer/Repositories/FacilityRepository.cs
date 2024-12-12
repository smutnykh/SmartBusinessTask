using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Repositories
{
	public class FacilityRepository : Repository<Facility>, IFacilityRepository
	{
		public FacilityRepository(ProductionDbContext dbContext) : base(dbContext)
		{
		}
	}
}
