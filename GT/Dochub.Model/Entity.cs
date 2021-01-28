using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rmon.Model
{
    public abstract class Entity : IEntity
    {
        /// <summary>
        /// Primary key for base entity.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Entity Name
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Entity Created Date
        /// </summary>
        private DateTime? createdDate;
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate
        {
            get { return createdDate ?? DateTime.UtcNow; }
            set { createdDate = value; }
        }

        /// <summary>
        /// Entity Modified Date
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Entity Created By
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Entity Modified By
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Entity Version
        /// </summary>
        [Timestamp]
        public byte[] Version { get; set; }
    }
}
