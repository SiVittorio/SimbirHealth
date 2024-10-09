using SimbirHealth.Data.Models._Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimbirHealth.Data.Models.Hospital
{
    /// <summary>
    /// Модель записи о больнице
    /// </summary>
    public class HospitalModel : BaseEntity, IDeleteable
    {
        public HospitalModel() { }

        public HospitalModel(string name, string address, string contactPhone, List<Room> rooms)
        {
            Name = name;
            Address = address;
            ContactPhone = contactPhone;
            Rooms = rooms;
        }

        /// <summary>
        /// Название больницы
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Адрес больницы
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Телефон больницы
        /// </summary>
        public string ContactPhone { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsDeleted { get; set; }

        [JsonIgnore]
        public List<Room> Rooms { get; set; }
    }
}
