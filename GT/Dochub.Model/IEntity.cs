using System;
using System.Collections.Generic;
using System.Text;

namespace Rmon.Model
{
    /// <summary>
    /// Base entity interface to contain all common properties.
    /// </summary>
    public interface IEntity : IModifiableEntity
    {
        /// <summary>
        /// Entity Id
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Entity Creation Date
        /// </summary>
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// Entity Modified Date
        /// </summary>
        DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Entity Created By
        /// </summary>
        string CreatedBy { get; set; }

        /// <summary>
        /// Entity Modified By
        /// </summary>
        string ModifiedBy { get; set; }

        /// <summary>
        /// Entity Version
        /// </summary>
        byte[] Version { get; set; }
    }
}
