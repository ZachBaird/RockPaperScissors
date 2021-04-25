# RockPaperScissors
SignalR, online implementation of RPS on .NET Core.

## Introduction

This app was a quick attempt at using SignalR to create an online, multiplayer rock-paper-scissors game in C# with SignalR. Two players can battle it out while additional users can spectate the match. If a player leaves, a new user is made a Player. Additionally, there is a chat feature where users can relax and converse.

## Notes

* The code could be cleaned up, but the most important parts live in `GameHub`, `GameHandler`, `UserHandler`, and `game.js`.
* I attempted to add a feature where the players and their move status "Zach has played" would display. It half works.
