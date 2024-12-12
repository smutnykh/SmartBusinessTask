using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
	public class ContractRepository : Repository<Contract>, IContractRepository
	{
		public ContractRepository(ProductionDbContext dbContext) : base(dbContext)
		{
		}

		public async Task<int> GetOccupiedAreaByFacilityCodeAsync(string facilityCode)
		{
			return await _dbSet.Where(x => x.FacilityCode == facilityCode).SumAsync(c => c.Amount * c.EquipmentType.Area);
		}
	}
}
