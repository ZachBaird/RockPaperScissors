using Microsoft.AspNetCore.SignalR;
using OreoRPS.Handlers;
using OreoRPS.Models;
using System;
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
        public async override Task OnConnectedAsync()
        {
            UserHandler.Users.Add(
                new User
                {
                    ConnectionId = Context.ConnectionId
                });

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Override method to remove added players.
        /// </summary>
        /// <param name="exception"></param>
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            UserHandler.RemoveUser(Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Sets a user up when they set their name.
        /// </summary>
        /// <param name="name">The name the user chose.</param>        
        public Task SetUser(string name)
        {
            UserHandler.SetUser(Context.ConnectionId, name);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Sends a chat message to the rest of the users.
        /// </summary>
        /// <param name="user">The name of the sender.</param>
        /// <param name="message">Their message.</param>        
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }


        /// <summary>
        /// Receives a move from the client and emits the user choice down to the clients.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="choice">The user's choice.</param>
        public async Task SendMove(string user, string choice)
        {
            if (UserHandler.Players.Count == 2)
            {
                if (UserHandler.PlayerIsInGame(Context.ConnectionId))
                {
                    UserHandler
                        .Players
                        .Find(u => u.ConnectionId == Context.ConnectionId)
                        .Move = choice;

                    UserHandler
                        .Players
                        .Find(u => u.ConnectionId == Context.ConnectionId)
                        .Name = user;

                    if (!UserHandler.CheckHand())
                    {
                        var (player1, player2) = UserHandler.GetPlayers();

                        var (winner, winnerExists) =
                            GameHandler.DetermineWinner(player1, player2);

                        if (winnerExists)
                        {
                            await Clients.All.SendAsync("ReceiveMove", winner.Name, $"Won with {winner.Move}!");
                            UserHandler.ResetHands();
                        }
                        else
                        {
                            await Clients.All.SendAsync("NoWinner", $"No winner with {player1.Move} & {player2.Move}. Try again!");
                            UserHandler.ResetHands();
                        }
                    }
                }
            }
            else            
                await Clients.All.SendAsync("ReceiveMove", user, "Error: you played by yourself. Move stored.");            
        }
    }
}
