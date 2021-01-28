using Rmon.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;

namespace Rmon.DataAccess
{
    /// <summary>
    /// This class is a helper class to audit model instances.
    /// </summary>
    public class EntityAuditAdapter
    {
        private static readonly string Unknown = nameof(Unknown);
        /// <summary>
        /// Marks user and timestamp information on entities and generates
        /// the audit log.
        /// </summary>
        /// <param name="currentUser">The <see cref="ClaimsPrincipal"/> logged in.</param>
        /// <param name="context">The <see cref="RmonContext"/> to use.</param>
        /// <param name="saveChangesAsync">A delegate to save the changes.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public async Task<int> ProcessEntityChangesAsync(
            ClaimsPrincipal currentUser,
            RmonContext context,
            Func<Task<int>> saveChangesAsync)
        {
            var user = Unknown;

            // grab user identifier
            if (currentUser != null)
            {
                var name = currentUser.Claims.FirstOrDefault(
                    c => c.Type == ClaimTypes.NameIdentifier);

                if (name != null)
                {
                    user = name.Value;
                }
                else if (!string.IsNullOrWhiteSpace(currentUser.Identity.Name))
                {
                    user = currentUser.Identity.Name;
                }
            }

            var audits = new List<EntityAudit>();

            // audit entities.
            foreach (var item in context.ChangeTracker.Entries<Entity>())
            {
                if (item.State == EntityState.Modified ||
                    item.State == EntityState.Added ||
                    item.State == EntityState.Deleted)
                {
                    // set created information for new item
                    if (item.State == EntityState.Added)
                    {
                        item.Property(RmonContext.CreatedBy).CurrentValue = user;
                        item.Property(RmonContext.CreatedOn).CurrentValue = DateTimeOffset.UtcNow;
                    }
        
                    object dbVal = null;

                    // set modified information for modified item
                    if (item.State == EntityState.Modified)
                    {
                        var db = await item.GetDatabaseValuesAsync();
                        dbVal = db.ToObject() as Entity;
                        item.Property(RmonContext.ModifiedBy).CurrentValue = user;
                        item.Property(RmonContext.ModifiedOn).CurrentValue = DateTimeOffset.UtcNow;
                    }

                    // parse the changes
                    var compObjects = new CompareLogic();
                    compObjects.Config.MaxDifferences = 99;
                    var compResult = compObjects.Compare(dbVal, item);

                    var auditDeltas = new List<AuditDelta>();

                    foreach (var change in compResult.Differences)
                    {
                        if (change.PropertyName.Substring(0,1) == ".")
                        {
                            var delta = new AuditDelta();
                            delta.FieldName = change.PropertyName.Substring(1, change.PropertyName.Length - 1);
                            delta.ValueBefore = change.Object1Value;
                            delta.ValueAfter = change.Object2Value;
                            auditDeltas.Add(delta);
                        }
                    }

                    var audit = new EntityAudit
                    {
                        EntityId = item.Entity.Id,
                        EntityType = item.GetType().Name,
                        Action = item.State.ToString(),
                        User = user,
                        Changes = auditDeltas
                    };

                    audits.Add(audit);
                }
            }

            if (audits.Count > 0)
            {
                // save
                context.EntityAudits.AddRange(audits);
            }

            var result = await saveChangesAsync();

            // need a second round to update newly generated keys
            var secondSave = false;

            // attach ids for add operations
            foreach (var audit in audits.Where(a => a.EntityId == 0).ToList())
            {
                secondSave = true;
                audit.EntityId = audit.EntityRef.Id;
                context.Entry(audit).State = EntityState.Modified;
            }

            if (secondSave)
            {
                await saveChangesAsync();
            }

            return result;
        }
    }
}
