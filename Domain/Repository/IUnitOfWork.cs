namespace Domain.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        ITicketRepository TicketRepository { get; } 
        IDiscussionRepository DiscussionRepository { get; } 

        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task<int> SaveChanges();
        Task<bool> SaveChangesReturnBool();
    }
}
