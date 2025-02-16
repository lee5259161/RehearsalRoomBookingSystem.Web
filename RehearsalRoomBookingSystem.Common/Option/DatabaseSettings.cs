namespace RehearsalRoomBookingSystem.Common.Option
{

    /// <summary>
    /// Represents the settings for the database.
    /// </summary>
    public class DatabaseSettings
    {
        /// <summary>
        /// Gets or sets the path to the SQLite database file.
        /// </summary>
        public string SqlitePath { get; set; }

        /// <summary>
        /// Gets or sets the connection string for the database.
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
