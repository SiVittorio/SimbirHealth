using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimbirHealth.Data.Models._Base
{
    /// <summary>
    /// Интерфейс базовой сущности
    /// </summary>
    public interface IBaseEntity
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        [Key]
        public Guid Guid { get; set; }

        /// <summary>
        /// Дата создания сущности
        /// </summary>
        public DateTime DateCreate { get; set; }
    }
}
