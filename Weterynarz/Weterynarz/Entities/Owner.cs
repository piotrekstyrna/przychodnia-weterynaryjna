using System.Collections.Generic;

namespace Weterynarz.Entities
{
    public class Owner : EntityBase
    {
        /// <summary>
        /// Imię właściciela
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Nazwisko właściciela
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Numer telefonu właściciela
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Zwierzę właściciela
        /// </summary>
        public virtual int? AnimalId { get; set; }
        public virtual Animal Animal { get; set; }
    }
}