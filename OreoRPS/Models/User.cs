namespace OreoRPS.Models
{
    /// <summary>
    /// Represens a current player in the game.
    /// </summary>
    public sealed class User
    {
        /// <summary>
        /// The connection Id of the user from SignalR context.
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// The move the player chose.
        /// </summary>
        public string Move { get; set; } = string.Empty;

        /// <summary>
        /// The name the player chose for themself.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
