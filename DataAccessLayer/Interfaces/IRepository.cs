namespace DataAccessLayer.Interfaces
{
	public interface IRepository<TEntity> where TEntity : class
	{
		Task<TEntity> GetByIdAsync(string id);

		Task<IList<TEntity>> GetAllAsync();

		Task SaveAsync(TEntity entity);
	}
}
