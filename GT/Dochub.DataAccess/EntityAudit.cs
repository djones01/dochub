using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Rmon.Model;

namespace Rmon.DataAccess
{
    /// <summary>
    /// Audit for all entities.
    /// </summary>
    public class EntityAudit
    {
        /// <summary>
        /// Audit key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Time of audit.
        /// </summary>
        public DateTimeOffset EventTime { get; set; }
            = DateTimeOffset.UtcNow;

        /// <summary>
        /// Id of the Entity being audited.
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// Type of Entity being audited.
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// Id of the user who made the change.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// What happened?
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// JSON serialized snapshot of before/after
        /// </summary>
        public List<AuditDelta> Changes { get; set; }

        public Entity EntityRef { get; set; }
    }
}
