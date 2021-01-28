using System;
using System.Collections.Generic;
using System.Text;

namespace Rmon.Model
{
    /// <summary>
    /// Base interface for use in <see cref="IEntity"/>.
    /// </summary>
    public interface IModifiableEntity
    {
        string EntityName { get; set; }
    }
}
