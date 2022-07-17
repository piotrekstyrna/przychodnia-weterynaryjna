using System;

namespace Weterynarz.Entities
{
    /// <summary>
    /// Encja zwierzęcia
    /// </summary>
    public class Animal : EntityBase
    {
        /// <summary>
        /// Imię zwierzęcia
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Płeć zwierzęcia
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Data urodzenia zwierzęcia
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Gatunek zwierzęcia
        /// </summary>
        public string Specie { get; set; }

        /// <summary>
        /// Rasa zwierzęcia
        /// </summary>
        public string Race { get; set; }
    }
}