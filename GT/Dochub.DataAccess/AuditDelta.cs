using System;
using System.Collections.Generic;
using System.Text;

namespace Rmon.DataAccess
{
    /// <summary>
    /// Stores field-level changes.
    /// </summary>
    public class AuditDelta
    {
        /// <summary>
        /// Name of the field with changes.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Old value of the field.
        /// </summary>
        public string ValueBefore { get; set; }

        /// <summary>
        /// New value of the field.
        /// </summary>
        public string ValueAfter { get; set; }
    }
}
