using SD.BuildingBlocks.Infrastructure;
using System.Threading.Tasks;

namespace AD.Demo.BookPublication.SQL.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookManagementDbContext dbContext;

        public UnitOfWork(BookManagementDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public int AffectedRows { get; private set; }

        public int Commit()
        {
            AffectedRows = dbContext.SaveChanges();
            return AffectedRows;
        }

        public async Task<int> CommitAsync()
        {
            try
            {
                AffectedRows = await dbContext.SaveChangesAsync();
                return AffectedRows;
            }
            catch
            {
                throw;
            }

        }
    }
}
