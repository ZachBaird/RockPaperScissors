using Microsoft.AspNetCore.SignalR;
using OreoRPS.Handlers;
using OreoRPS.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OreoRPS.Hubs
{
    /// <summary>
    /// SignalR hub for Rock Paper Scissors.
    /// </summary>
    public class GameHub : Hub
    {
        /// <summary>
        /// Override method to add and track the users connected to the hub.
        /// </summary>
        /// <returns>A task.</returns>
        public override Task OnConnectedAsync()
        {
            // We only want 2 players to be able to play.
            if (UserHandler.UserCount < 2)
            {
                UserHandler.ConnectedIds.Add(Context.ConnectionId);
                UserHandler.Users.Add(new User
                {
                    ConnectionId = Context.ConnectionId,
                    Move = ""
                });
            }

            return base.OnConnectedAsync();
        }

        /// <summary>
        /// Override method to remove added players.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns>A task.</returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            UserHandler.Users.Remove(UserHandler.Users.Find(u => u.ConnectionId == Context.ConnectionId));
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Receives a move from the client and emits the user choice down to the clients.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="choice">The user's choice.</param>
        /// <returns>A task.</returns>
        public async Task SendMove(string user, string choice)
        {
            if (UserHandler.Users.Count == 2)
            {
                if (UserHandler.ConnectedIds.Contains(Context.ConnectionId) &&
                    UserHandler.CheckHand())
                {
                    // Add the user's name to the class in case it isn't there.
                    UserHandler
                        .Users
                        .Find(u => u.ConnectionId == Context.ConnectionId)
                        .Name = user;

                    UserHandler
                        .Users
                        .Find(u => u.ConnectionId == Context.ConnectionId)
                        .Move = choice;

                    if (!UserHandler.CheckHand())
                    {
                        var (winner, winnerExists) = GameHandler.DetermineWinner(UserHandler.Users[0], UserHandler.Users[1]);

                        if (winnerExists)
                            await Clients.All.SendAsync("ReceiveMove", winner.Name, $"{winner.Name} has won with {winner.Move}!");
                        else
                        {
                            await Clients.All.SendAsync("ReceiveMove", user, "There was no winner. Try again!");
                        }
                    }
                    else
                    {
                        await Clients.All.SendAsync("ReceiveMove", user, "No winner. Try again!");
                    }
                }
            }
            else            
                await Clients.All.SendAsync("ReceiveMove", user, "Error: you played by yourself. Move stored.");            
        }
    }
}
