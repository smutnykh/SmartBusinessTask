namespace DataAccessLayer.Interfaces
{
	public interface IContractRepository : IRepository<Contract>
	{
		Task<int> GetOccupiedAreaByFacilityCodeAsync(string facilityCode);
	}
}
