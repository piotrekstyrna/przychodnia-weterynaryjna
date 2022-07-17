namespace Weterynarz.Classes
{

    /// <summary>
    /// Statyczna klasa z naszym kontekstem EF Corowym. Statyczna dlatego, żeby była tylko 1 instancja tego contextu
    /// </summary>
    public static class StaticContext
    {
        /// <summary>
        /// Context danych EF Core
        /// </summary>
        public static VetContext Context { get; } = new VetContext();

        /// <summary>
        /// Konstruktor klasy statycznej wykonuje się TYLKO 1 raz po 1 użyciu klasy statycznej
        /// </summary>
        static StaticContext()
        {
            Context.Database.EnsureCreated();
        }
    }
}