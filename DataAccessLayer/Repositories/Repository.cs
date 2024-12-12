using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		protected readonly ProductionDbContext _dbContext;
		protected readonly DbSet<TEntity> _dbSet;

		public Repository(ProductionDbContext dbContext)
		{
			_dbContext = dbContext;
			_dbSet = dbContext.Set<TEntity>();
		}

		public async Task<TEntity> GetByIdAsync(string id) => await _dbSet.FindAsync(id);

		public async Task<IList<TEntity>> GetAllAsync() => await _dbSet.AsNoTracking().ToListAsync();

		public async Task SaveAsync(TEntity entity)
		{
			await _dbSet.AddAsync(entity);
			await _dbContext.SaveChangesAsync();
		}
	}
}
