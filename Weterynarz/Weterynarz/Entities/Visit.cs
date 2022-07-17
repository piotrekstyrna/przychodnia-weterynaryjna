using System;

namespace Weterynarz.Entities
{
    /// <summary>
    /// Encja wizyty
    /// </summary>
    public class Visit : EntityBase
    {
        /// <summary>
        /// Data wizyty
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Czas trwania wizyty
        /// </summary>
        public TimeSpan Time { get; set; }

        /// <summary>
        /// Opis czasu trwania wizyty
        /// </summary>
        public string TimeDescription => $"{Time.TotalMinutes} [minut]";

        /// <summary>
        /// Koszt wizyty
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Właściciel na wizycie
        /// </summary>
        public virtual int OwnerId { get; set; }
        public virtual Owner Owner { get; set; }

        /// <summary>
        /// Dolegliwość zwierzęcia ustalona po wizycie
        /// </summary>
        public virtual int DisorderId { get; set; }
        public virtual Disorder Disorder { get; set; }
    }
}