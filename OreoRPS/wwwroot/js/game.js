'use strict';

// Create a connection variable.
const connection = new signalR.HubConnectionBuilder().withUrl('/gameHub').build();

// DOM variables.
let playerName = '';
const scoreBoard = document.getElementById('scoreBoard');
const playerNameButton = document.getElementById('setPlayerName');
const playerNameInput = document.getElementById('playerName');
const chatSendButton = document.getElementById('sendButton');
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

// Handler for when someone sets their name.
playerNameButton.addEventListener('click', (e) => {
    e.preventDefault();

    if (playerNameInput.value != '') {
        playerName = playerNameInput.value;

        playerNameButton.disabled = true;
        playerNameInput.disabled = true;

        chatSendButton.disabled = false;
        enableChoices(choices);

        connection.invoke('SetUser', playerName);
    } else {
        alert('Must create an actual name to play.');
    }
});

// Disable choices until connection is established.
disableChoices(choices);

// Disable send button until connection is established.
chatSendButton.disabled = true;

connection.on("SetPlayer", (name, htmlId) => {
    document.getElementById(htmlId).innerText = `${name}: `;
});

connection.on('ReceiveMove', (user, message) => {
    scoreBoard.innerHTML = ``;
    const p = document.createElement('p');
    p.textContent = `${user}: ${message}`;
    scoreBoard.appendChild(p);
});

connection.on('UpdateMove', (user, htmlId) => {
    document.getElementById(htmlId).innerText = `${user}: played...`;
});

connection.on('ResetHands', (firstName, secondName) => {
    document.getElementById('firstUser').innerText = `${firstName}: `;
    document.getElementById('secondUser').innerText = `${secondName}: `;
});

connection.on('NoWinner', (message) => {
    scoreBoard.innerHTML = ``;
    const p = document.createElement('p');
    p.textContent = `${message}`;
    scoreBoard.appendChild(p);
});

// Game chat SignalR events.
connection.on('ReceiveMessage', (user, message) => {
    const msg = message.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
    const encodedMsg = `${user}: ${msg}`;
    const li = document.createElement('li');
    li.classList.add('list-member');
    li.textContent = encodedMsg;
    document.getElementById('messagesList').appendChild(li);
});

chatSendButton.addEventListener('click', (e) => {
    e.preventDefault();
    const message = document.getElementById('messageInput').value;
    document.getElementById('messageInput').value = '';

    connection.invoke('SendMessage', playerName, message).catch((err) => console.error(err.toString()));
});

connection.start().then(() => {
    console.log('Connection made to game hub..');
}).catch((err) => console.error(err.toString()));


choices[0].addEventListener('click', (e) => {
    e.preventDefault();
    connection.invoke('SendMove', playerName, 'rock');
});

choices[1].addEventListener('click', (e) => {
    e.preventDefault();
    connection.invoke('SendMove', playerName, 'paper');
});

choices[2].addEventListener('click', (e) => {
    e.preventDefault();
    connection.invoke('SendMove', playerName, 'scissors');
});
