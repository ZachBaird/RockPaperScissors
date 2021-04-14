'use strict';

const connection = new signalR.HubConnectionBuilder().withUrl('/gameHub').build();

const choices = [
    document.getElementById('rockChoice'),
    document.getElementById('paperChoice'),
    document.getElementById('scissorsChoice'),
]

const disableChoices = (choices) => {
    choices.forEach((choice) => choice.disabled = false);
};

const enableChoices = (choices) => {
    choices.forEach((choice) => choice.disabled = true);
};

// Disable choices until connection is established.
disableChoices(choices);

connection.on("ReceiveMove", (user, choice) => {
});

connection.start().then(() => {
    enableChoices(choices);
}).catch((err) => console.error(err.toString()));

