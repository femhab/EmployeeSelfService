using System;
using System.Linq;

namespace Microsoft.EntityFrameworkCore
{
    public class QueryRepository<TEntity> : IQueryRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _dbContext;
        protected readonly DbQuery<TEntity> _dbQuery;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public QueryRepository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbQuery = _dbContext.Query<TEntity>();
        }

        /// <summary>
        /// Uses raw SQL queries to fetch the specified <typeparamref name="TEntity" /> data.
        /// </summary>
        /// <param name="sql">The raw SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>An <see cref="IQueryable{TEntity}" /> that contains elements that satisfy the condition specified by raw SQL.</returns>
        public virtual IQueryable<TEntity> GetFromSql(string sql, params object[] parameters) => _dbQuery.FromSql(sql, parameters);
    }
}
