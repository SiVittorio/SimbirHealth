using SimbirHealth.Data.Models._Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimbirHealth.Data.Models.Hospital
{
    /// <summary>
    /// Кабинет в больнице
    /// </summary>
    public class Room : BaseEntity, IDeleteable
    {
        public Room() { }

        public Room(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Наименование кабинета
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Удалена ли запись
        /// </summary>
        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(Hospital))]
        public Guid HospitalGuid { get; set; }
        [JsonIgnore]
        public HospitalModel Hospital { get; set; }
    }
}
