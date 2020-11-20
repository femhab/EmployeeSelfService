namespace Microsoft.EntityFrameworkCore
{
    public interface IQueryUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Gets the specified query repository (DbQuery) for the <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="hasCustomRepository"><c>True</c> if providing custom repositry</param>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>An instance of type inherited from <see cref="IQueryRepository{TEntity}"/> interface.</returns>
        IQueryRepository<TEntity> GetQueryRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class;
    }
}
