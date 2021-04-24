'use strict';

const connection = new signalR.HubConnectionBuilder().withUrl('/gameHub').build();

const choices = [
    document.getElementById('rock'),
    document.getElementById('paper'),
    document.getElementById('scissors'),
]

const disableChoices = (choices) => {
    choices.forEach((choice) => choice.disabled = true);
};

const enableChoices = (choices) => {
    choices.forEach((choice) => choice.disabled = false);
};

// Disable choices until connection is established.
disableChoices(choices);

connection.on('ReceiveMove', (user, message) => {
    const li = document.createElement('li');
    li.textContent = `${user}: ${message}`;
    li.classList.add('game-log');
    document.getElementById('scoreBoard').appendChild(li);
});

connection.on('NoWinner', (message) => {
    const li = document.createElement('li');
    li.textContent = `${message}`;
    document.getElementById('scoreBoard').appendChild(li);
});

connection.start().then(() => {
    console.log('Connection made to game hub..');
    console.log(choices);
    enableChoices(choices);
}).catch((err) => console.error(err.toString()));


choices[0].addEventListener('click', (e) => {
    e.preventDefault();
    const player = document.getElementById('playerName').value;
    connection.invoke('SendMove', player, 'rock');
});

choices[1].addEventListener('click', (e) => {
    e.preventDefault();
    const player = document.getElementById('playerName').value;
    connection.invoke('SendMove', player, 'paper');
});

choices[2].addEventListener('click', (e) => {
    e.preventDefault();
    const player = document.getElementById('playerName').value;
    connection.invoke('SendMove', player, 'scissors');
});
