using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// Represents a default generic repository implements the <see cref="IRepository{TEntity}"/> interface.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public static class RepositoryExtension
    {
        /// <summary>
        /// Gets the <see cref="IEnumerable{TEntity}"/> based on predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IEnumerable{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public static async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(this IRepository<TEntity> repository, Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string include = null, bool disableTracking = true)
             where TEntity : class
        {
#pragma warning disable CS0618 // Type or member is obsolete
            IQueryable<TEntity> query = repository.GetAll();
#pragma warning restore CS0618 // Type or member is obsolete

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (!string.IsNullOrEmpty(include))
            {
                foreach (var includeProperty in include.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = orderBy(query); //.ToListAsync();
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Gets the <see cref="IEnumerable{TEntity}"/> based on predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="disableTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>An <see cref="IEnumerable{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="predicate"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        public static IEnumerable<TEntity> GetAll<TEntity>(this IRepository<TEntity> repository, Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null, string include = null, bool disableTracking = true)
             where TEntity : class
        {
#pragma warning disable CS0618 // Type or member is obsolete
            IQueryable<TEntity> query = repository.GetAll();
#pragma warning restore CS0618 // Type or member is obsolete

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (!string.IsNullOrEmpty(include))
            {
                foreach (var includeProperty in include.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = orderBy(query); //.ToList();
            }

            return query.ToList();
        }

        /// <summary>
        /// Gets dbcontext to perform queryable action
        /// </summary>
        /// <returns>The <see cref="IQueryable{TEntity}"/>.</returns>
        public static IQueryable<TEntity> GetDbContext<TEntity>(this IRepository<TEntity> repository) where TEntity : class => repository.GetAll();
    }
}
