namespace Weterynarz.Entities
{
    /// <summary>
    /// Encja dolegliwości
    /// </summary>
    public class Disorder : EntityBase
    {
        /// <summary>
        /// Nazwa dolegliwości
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Czy dolegliwość jest uleczalna
        /// </summary>
        public bool IsHealable { get; set; }

        /// <summary>
        /// Lek na dolegliwość
        /// </summary>
        public string Medicine { get; set; }
    }
}