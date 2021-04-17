using OreoRPS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OreoRPS.Handlers
{
    /// <summary>
    /// Handles Game-specific logic.
    /// </summary>
    public static class GameHandler
    {
        /// <summary>
        /// Determines who the winner is if one exists.
        /// </summary>
        /// <param name="user1">The first user.</param>
        /// <param name="user2">The second user.</param>
        /// <returns>A tuple of the User who wins (or null) and a bool if someone won.</returns>
        public static (User, bool) DetermineWinner(User user1, User user2)
        {
            if (user1.Move == user2.Move)
                return (null, false);

            return user1.Move switch
            {
                "rock" => user2.Move switch
                {
                    "paper" => (user2, true),
                    _ => (user1, true)
                },
                "paper" => user2.Move switch
                {
                    "scissors" => (user2, true),
                    _ => (user1, true)
                },
                "scissors" => user2.Move switch
                {
                    "rock" => (user2, true),
                    _ => (user1, true)
                },
                _ => (null, false),
            };
        }
    }
}
