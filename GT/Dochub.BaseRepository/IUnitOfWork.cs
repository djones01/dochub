using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rmon.BaseRepository
{
    public interface IUnitOfWork<TEntity> : IDisposable
    {
        /// <summary>
        /// The repository related to the unit of work.
        /// </summary>
        IBasicRepository<TEntity> Repo { get; }

        /// <summary>
        /// Sets the user for auditing.
        /// </summary>
        /// <param name="user">The <see cref="ClaimsPrincipal"/> for audits.</param>
        void SetUser(ClaimsPrincipal user);

        /// <summary>
        /// Commit the work
        /// </summary>
        /// <returns>An asynchronous <see cref="Task"/>.</returns>
        Task CommitAsync();
    }
}
