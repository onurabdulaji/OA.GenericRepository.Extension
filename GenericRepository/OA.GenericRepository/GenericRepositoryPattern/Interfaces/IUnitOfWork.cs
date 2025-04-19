namespace OA.GenericRepository.GenericRepositoryPattern.Interfaces;
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();
}
