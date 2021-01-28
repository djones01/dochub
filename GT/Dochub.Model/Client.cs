using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Rmon.Model
{
    public class Client : Entity
    {
        /// <summary>
        /// Client Name
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "Client name cannot exceed 100 characters.")]
        public string Name { get; set; }

        /// <summary>
        /// Client Description
        /// </summary>
        [StringLength(240, ErrorMessage = "Client description cannot exceed 240 characters.")]
        public string Description { get; set; }
    }
}
