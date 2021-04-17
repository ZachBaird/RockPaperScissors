using OreoRPS.Models;
using System.Collections.Generic;
using System.Linq;

namespace OreoRPS.Handlers
{
    /// <summary>
    /// User handler class to manage hub user data.
    /// </summary>
    public static class UserHandler
    {
        /// <summary>
        /// The first player's choice.
        /// </summary>
        public static string player1Choice = "";

        /// <summary>
        /// The second player's choice.
        /// </summary>
        public static string player2Choice = "";

        /// <summary>
        /// HashSet of the connected user ids.
        /// </summary>
        public static HashSet<string> ConnectedIds = new HashSet<string>();

        /// <summary>
        /// List of users.
        /// </summary>
        public static List<User> Users = new List<User>();

        /// <summary>
        /// Convenience method to get the number of connected users.
        /// </summary>
        public static int UserCount => ConnectedIds.Count;

        public static bool CheckHand()
        {
            if (Users.All(u => string.IsNullOrWhiteSpace(u.Move)) ||
                Users.Any(u => string.IsNullOrWhiteSpace(u.Move)))
            {
                return true;
            }

            return false;
        }
    }
}
