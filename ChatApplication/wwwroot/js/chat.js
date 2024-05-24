const user = document.getElementById('userInput');
const message = document.getElementById('messageInput');
//const receiver = document.getElementById('receiverInput');
const sendButton = document.getElementById('sendButton');
const messageList = document.getElementById('messages');

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub").build();
//.configureLogging(signalR.LogLevel.Information)
//.build();

//connection.on("ReceiveMessage", (user, message) => {

//    var span = document.createElement("span");
//    span.textContent = user;
//    document.getElementById("client").appendChild(span);


//    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
//    //var encodedMsg = user + " says " + msg;
//    //Append msg to li tag
//    //var li = document.createElement("li");
//    //li.textContent = encodedMsg;
//    //document.getElementById("messages").appendChild(li);

//    //Append msg to p tag
//    var encodedMsg = msg;
//    var p = document.createElement("p");
//    p.textContent = encodedMsg;
//    document.getElementById("messages").appendChild(p);

//});

connection.on("ReceiveMessage", (user, message) => {

    debugger;
    // Create a div element for author
    var authorDiv = document.createElement("div");
    authorDiv.classList.add("author");

    // Create a span element for the author
    var authorSpan = document.createElement("span");
    authorSpan.textContent = user;

    authorDiv.appendChild(authorSpan);

    // Create a div element for chat
    var chatDiv = document.createElement("div");
    chatDiv.classList.add("chat");

    // Create a p element for the chat message
    var messageP = document.createElement("p");
    messageP.textContent = message;

    chatDiv.appendChild(messageP);

    // Create a div element for the chat box
    var chatBox = document.createElement("div");
    chatBox.classList.add("chat__box", "bot__chat");

    // Append the author and message to the chat box
    chatBox.appendChild(authorDiv);
    chatBox.appendChild(chatDiv);

    // Append the chat box to the mymessages container
    document.getElementById("chat1").appendChild(chatBox);
});

connection.start().then(function () {
    //Pass logged in userId or username
    $.ajax({
        url: '/api/User/get-username',
        method: 'GET',
        success: function (username) {
            console.log('Logged-in username:', username);

            connection.invoke("GetConnectionId", username).then(function (id) {
                //Shows connectionId on View
                document.getElementById("connectionId").innerText = id;
                //document.getElementById("receiverId").innerText = id;
            });
            document.getElementById("sendButton").disabled = false;


        },
        error: function (xhr, status, error) {
            console.error('Error retrieving username:', error);
        }
    });
}).catch(function (err) {
    return console.error(err.toString());
});


document.getElementById("sendButton").addEventListener('click', event => {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;

    //connection.invoke("SendMessage", user, message).catch(function (err) {
    //    return console.error(err.toString());
    //});

    //event.preventDefault();

   // DisplayMessageOnSendersChat(user, message);

    debugger;

    fetch('/api/Chat/send-message-to-all', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            user: user,
            message: message
        })
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            console.log(response);
            console.log('Message sent successfully');
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });

});


//document.getElementById("sendToUser").addEventListener('click', event => {
//    var user = document.getElementById("userInput").value;
//    var receiverConnectionId = document.getElementById("receiverId").value;
//    var msg = document.getElementById("messageInput").value;
//    connection.invoke("SendToUser", user, receiverConnectionId, msg).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});

function DisplayMessageOnSendersChat(user, msg) {

    debugger;
    // Create a div element for author
    var authorDiv = document.createElement("div");
    authorDiv.classList.add("author");

    // Create a span element for the author
    var authorSpan = document.createElement("span");
    authorSpan.textContent = user;

    authorDiv.appendChild(authorSpan);

    // Create a div element for chat
    var chatDiv = document.createElement("div");
    chatDiv.classList.add("chat");

    // Create a p element for the chat message
    var messageP = document.createElement("p");
    messageP.textContent = msg;

    chatDiv.appendChild(messageP);

    // Create a div element for the chat box
    var chatBox = document.createElement("div");
    chatBox.classList.add("chat__box", "your__chat");

    // Append the author and message to the chat box
    chatBox.appendChild(authorDiv);
    chatBox.appendChild(chatDiv);

    // Append the chat box to the mymessages container
    document.getElementById("chat1").appendChild(chatBox);
}


//function DisplayMessageOnSendersChat(user, msg)
//{
//    debugger;

//    var span = document.createElement("span");
//    span.textContent = user;
//    document.getElementById("myusername").appendChild(span);

//    var p = document.createElement("p");
//    p.textContent = msg;
//    document.getElementById("mymessages").appendChild(p);   
//}

document.getElementById("sendToUser").addEventListener('click', event => {

    debugger;
    // Get the content of the <h3> tag with ID "clientName"
    var clientNameElement = document.getElementById("clientName");
    var clientName = clientNameElement.textContent.trim(); // Remove leading and trailing whitespace

    // Extract the value of @ViewBag.ClientName from the full text content
    var prefix = "Hello! Welcome to the Chat Room of ";
    if (clientName.startsWith(prefix)) {
        clientName = clientName.substring(prefix.length); // Remove the prefix
    }

    // Display the client name in the console
    console.log("Client Name:", clientName);
    //alert(clientName);



    var user = document.getElementById("userInput").value;
    var msg = document.getElementById("messageInput").value;

    DisplayMessageOnSendersChat(user, msg);

    //connection.invoke("GetUsernameAsync", user).catch(function (err) {
    //    return console.error(err.toString());
    //});

    fetch('/api/Chat/send-message', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            user: user,
            clientName: clientName,
            message: msg
        })
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            console.log(response);
            console.log('Message sent successfully');
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });

    event.preventDefault();
});
