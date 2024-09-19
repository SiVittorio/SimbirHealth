using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimbirHealth.Data.Models._Base
{
    /// <summary>
    /// Базовая сущность
    /// </summary>
    public abstract class BaseEntity : IBaseEntity
    {
        public BaseEntity()
        {
            DateCreate = DateTime.UtcNow;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        [Key]
        public Guid Guid { get; set; } = Guid.NewGuid();   
        
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        [ReadOnly(true)]
        public DateTime DateCreate { get; set; }
    }
}
