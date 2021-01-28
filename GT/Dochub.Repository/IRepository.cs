using System;
using Rmon.BaseRepository;
using Rmon.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Rmon.Repository
{
    /// <summary>
    /// More fine-grained repository that exposes the data context. Used by
    /// the <see cref="UnitOfWork{TContext, TEntity}"/> implementation.
    /// </summary>
    /// <typeparam name="TEntity">The <see cref="TEntity"/> for the repo.</typeparam>
    /// <typeparam name="TContext">The <see cref="DbContext"/> type.</typeparam>
    public interface IRepository<TEntity, TContext> :
        IDisposable,
        IBasicRepository<TEntity> where TContext : DbContext
    {
        /// <summary>
        /// The <see cref="DbContext"/> instance.
        /// </summary>
        TContext PersistedContext { get; set; }
    }
}
