namespace KarateSemaphore
{
    /// <summary>
    /// Interface for display name settings.
    /// </summary>
    public interface IDisplayTextSettings
    {
        /// <summary>
        /// Gets the display name for the red competitor.
        /// </summary>
        string Aka { get; }

        /// <summary>
        /// Gets the display name for the blue competitor.
        /// </summary>
        string Ao { get; }
    }
}