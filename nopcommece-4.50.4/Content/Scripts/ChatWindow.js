"use strict";

class ChatWindow  {
    
    constructor(containerId, fromUserId)
    {    
        this.toUserId = "Admin";
        this.fromUserId = fromUserId;
        this.IsAdmin = fromUserId == "Admin";

        this.connection = new signalR.HubConnectionBuilder().withUrl("/notify?userId=" + fromUserId).build();

        this.container = document.getElementById(containerId);
        this.messages = this.container.querySelector('.messages');
        this.disconnectedMessage = this.container.querySelector('.disconnected-message');
        this.headText = this.container.querySelector('.head-text');
        this.inputText = this.container.querySelector('textarea');
        this.sendButton = this.container.querySelector('.send-btn');
        this.closeButton = this.container.querySelector('.close-btn');

        this.inputText.addEventListener('keydown', (e) => {
            if (e.keyCode === 13 && !e.shiftKey) {
                e.preventDefault();
                this.SendMessage();
            }
        });

        if (this.closeButton) {
            this.closeButton.addEventListener('click', () => {
                this.container.style.display = 'none';
            });
        }
        

        // Agregar un evento 'click' al botón de enviar
        this.sendButton.addEventListener('click', () => {
            this.SendMessage();
        });

        this.connection.on("ChatMessages", (messages) => {
            console.log("Receive chat messages");
            messages.reverse();
            for (var i = 0; i < messages.length; i++) {
                var obj = messages[i];
                this.AddMessage(obj.message, obj.fromUserId == this.fromUserId);
            }
        });

        
        this.connection.on("Message", (message, senderId) => {
            console.log("Receive mensage from: " + senderId);
            if (this.fromUserId == "Admin" && this.toUserId != senderId)
                return;

            this.AddMessage(message, false);
        });

        if (this.IsAdmin) {
            this.connection.on("Disconected", (chatId) => {
                console.log("Disconected: " + chatId);
                this.disconnectedMessage.style.display = 'block';    
            });

            this.connection.on("ImAlive", (chatId) => {
                console.log("Receive i am alive: " + chatId);
                this.disconnectedMessage.style.display = 'none';    
            });

            this.connection.on("RefreshMessageList", () => {
                updateTable('#chat-points-grid');
            });
        } else {
            this.connection.on("AreYouAlive", () => {
                console.log("Receive is alive");
                if (this.fromUserId != "Admin")
                    this.connection.invoke("ImAlive");
            });
        }
    }

    SendMessage() {
        
        var messageText = this.inputText.value;
        
        if (messageText == "" || this.fromUserId == this.toUserId)
            return;

        console.log("SendMessage From:'" + this.fromUserId + "' To: '" + this.toUserId + "' - " + messageText);

        if (this.IsAdmin)
            this.connection.invoke("AdminSendMessage", messageText, this.toUserId);
        else
            this.connection.invoke("UserSendMessage", messageText);

        this.AddMessage(messageText, true);
    }

    async SetToUserId(toUserId) {
        this.Clear();
        console.log("SetToUserId: " + toUserId);
        this.toUserId = toUserId;
        if (this.disconnectedMessage)
            this.disconnectedMessage.style.display = 'block';
        if (this.headText)
            this.headText.innerHTML = this.toUserId;

        await this.TryConnect();

        if (this.IsAdmin) {
            this.connection.invoke("GetChatMessages", this.toUserId);
            this.connection.invoke("AreYouAlive", this.toUserId);
        }
        else
            this.connection.invoke("GetChatMessages", this.fromUserId);
    }

    async TryConnect() {
        if (this.connection.state == signalR.HubConnectionState.Disconnected) {
            await this.connection.start();
            console.log("ChatWindow SignalR Connected.");
        }
    }

    Clear() {
        this.messages.innerText = "";
        this.inputText.value = "";
    }

    AddMessage(message, fromMe) {
        // Obtener el mensaje del input
        let messageObj = message;

        // Crear un nuevo elemento de mensaje
        let obj = document.createElement('div');
        if (fromMe)
            obj.className = "message from-me"
        else
            obj.className = "message from-others"
        obj.innerText = messageObj;

        // Agregar el mensaje a la lista de mensajes
        this.messages.appendChild(obj);

        this.messages.scrollTop = this.messages.scrollHeight;

        // Limpiar el input
        this.inputText.value = '';
    }
}



