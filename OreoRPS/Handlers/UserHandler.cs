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
        /// List of players.
        /// </summary>
        public static List<User> Players = new List<User>();

        /// <summary>
        /// List of users.
        /// </summary>
        public static List<User> Users = new List<User>();

        /// <summary>
        /// Convenience method to get the number of connected users.
        /// </summary>
        public static int PlayerCount => Players.Count;
        
        /// <summary>
        /// Convenience method to get the two players in the game.
        /// </summary>
        /// <returns></returns>
        public static (User, User) GetPlayers()
        {
            var player1 = Players[0];
            var player2 = Players[1];

            return (player1, player2);
        }

        /// <summary>
        /// Sets a user as a player.
        /// </summary>
        /// <param name="connectionId">The user's connection Id.</param>
        /// <param name="name">The user's name.</param>
        public static void SetUser(string connectionId, string name)
        {
            if (PlayerCount < 2)
            {
                var newPlayer = Users.Find(u => u.ConnectionId == connectionId);
                newPlayer.Name = name;

                Players.Add(newPlayer);
                Users.Remove(newPlayer);
            }
        }

        /// <summary>
        /// Handles primary logic when a user disconnects from the game.
        /// </summary>
        /// <param name="connectionId">The connection Id of the user.</param>
        public static void RemoveUser(string connectionId)
        {
            // Determine if it's a player disconnecting or a user. If it's a player, remove player
            //  and move a user in as a new player. If it's a user, just remove the user.
            if (Players.Select(p => p.ConnectionId).Contains(connectionId))
            {
                var player = Players.Find(p => p.ConnectionId == connectionId);
                Players.Remove(player);

                if (Users.Count > 0)
                {
                    var newPlayer = Users.FirstOrDefault();
                    Users.Remove(newPlayer);
                    Players.Add(newPlayer);
                }
            }
            else
            {
                var userToRemove = Users.Find(u => u.ConnectionId == connectionId);
                Users.Remove(userToRemove);
            }
        }

        /// <summary>
        /// Determines if a connection id is a specific player.
        /// </summary>
        /// <param name="connectionId">The connection id to test.</param>
        /// <returns>True if the player is in the game, false if not.</returns>
        public static bool PlayerIsInGame(string connectionId) => 
            Players.Select(p => p.ConnectionId).Contains(connectionId);

        public static bool CheckHand()
        {
            if (Players.Any(u => string.IsNullOrWhiteSpace(u.Move)))            
                return true;
            else
                return false;
        }

        /// <summary>
        /// Sets the <see cref="User" />'s hands to empty strings.
        /// </summary>
        public static void ResetHands()
        {
            foreach (var player in Players)
                player.Move = string.Empty;
        }
    }
}
