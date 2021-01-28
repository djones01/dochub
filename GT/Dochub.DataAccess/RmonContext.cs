using Rmon.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Rmon.DataAccess
{
    public class RmonContext : DbContext, ISupportUser
    {
        /// <summary>
        /// Tracking lifetime of contexts.
        /// </summary>
        private readonly Guid _id;

        /// <summary>
        /// For audit info
        /// </summary>
        private readonly EntityAuditAdapter _adapter = new EntityAuditAdapter();

        /// <summary>
        /// The logged in <see cref="ClaimsPrincipal"/>.
        /// </summary>
        public ClaimsPrincipal User { get; set; }

        /// <summary>
        /// Magic string.
        /// </summary>
        public static readonly string RowVersion = nameof(RowVersion);

        /// <summary>
        /// Who created it?
        /// </summary>
        public static readonly string CreatedBy = nameof(CreatedBy);

        /// <summary>
        /// When was it created?
        /// </summary>
        public static readonly string CreatedOn = nameof(CreatedOn);

        /// <summary>
        /// Who last modified it?
        /// </summary>
        public static readonly string ModifiedBy = nameof(ModifiedBy);

        /// <summary>
        /// When was it last modified?
        /// </summary>
        public static readonly string ModifiedOn = nameof(ModifiedOn);

        /// <summary>
        /// Inject options.
        /// </summary>
        /// <param name="options">The <see cref="DbContextOptions{ContactContext}"/>
        /// for the context
        /// </param>
        public RmonContext(DbContextOptions<RmonContext> options)
            : base(options)
        {
            _id = Guid.NewGuid();
            Debug.WriteLine($"{_id} context created.");
        }

        /// <summary>
        /// Override the save operation to generate audit information.
        /// </summary>
        /// <param name="token">The <seealso cref="CancellationToken"/>.</param>
        /// <returns>The result.</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken token
            = default)
        {
            return await _adapter.ProcessEntityChangesAsync(
                User, this, async () => await base.SaveChangesAsync(token));
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<EntityAudit> EntityAudits { get; set; }

        /// <summary>
        /// Define the model.
        /// </summary>
        /// <param name="modelBuilder">The <see cref="ModelBuilder"/>.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Entity>();

            // this property isn't on the C# class
            // so we set it up as a "shadow" property and use it for concurrency
            entity.Property<byte[]>(RowVersion).IsRowVersion();

            // audit fields
            entity.Property<string>(ModifiedBy);
            entity.Property<DateTimeOffset>(ModifiedOn);
            entity.Property<string>(CreatedBy);
            entity.Property<DateTimeOffset>(CreatedOn);

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Dispose pattern.
        /// </summary>
        public override void Dispose()
        {
            Debug.WriteLine($"{_id} context disposed.");
            base.Dispose();
        }

        /// <summary>
        /// Dispose pattern.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/></returns>
        public override ValueTask DisposeAsync()
        {
            Debug.WriteLine($"{_id} context disposed async.");
            return base.DisposeAsync();
        }
    }
}
