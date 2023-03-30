"use strict";

class ChatWindow  {
    
    constructor(containerId, fromUserId)
    {    
        this.toUserId = "Admin";
        this.fromUserId = fromUserId;
        this.IsAdmin = fromUserId == "Admin";
        this.LimitMessages = 50;

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
                this.setCookie("NopChatActive", "false", 1);
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

    getCookie(nombre_de_la_cookie) {
        // Se crea una variable para almacenar el valor de la cookie
        var valor_cookie = "";

        // Se crea una cadena con todas las cookies
        var cookies = document.cookie;

        // Se busca la cookie específica
        var index = cookies.indexOf(nombre_de_la_cookie + "=");
        if (index !== -1) {
            // Se extrae el valor de la cookie
            index = index + nombre_de_la_cookie.length + 1;
            var end = cookies.indexOf(";", index);
            if (end === -1) {
                end = cookies.length;
            }
            valor_cookie = cookies.substring(index, end);
        }

        // Se devuelve el valor de la cookie
        return valor_cookie;
    }

    setCookie(c_name, value, exdays) {
        var exdate = new Date();
        exdate.setDate(exdate.getDate() + exdays);
        var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
        document.cookie = c_name + "=" + c_value;
    }

    NopChatActive() {
        var v = this.getCookie("NopChatActive");
        console.log(v);
        return v == "true";
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
        this.setCookie("NopChatActive", "true", 1);
        this.Clear();
        console.log("SetToUserId: " + toUserId);
        this.toUserId = toUserId;
        if (this.disconnectedMessage)
            this.disconnectedMessage.style.display = 'block';
        if (this.headText)
            this.headText.innerHTML = this.toUserId;

        await this.TryConnect();

        if (this.IsAdmin) {
            this.connection.invoke("GetChatMessages", this.toUserId, this.LimitMessages);
            this.connection.invoke("AreYouAlive", this.toUserId);
        }
        else
            this.connection.invoke("GetChatMessages", this.fromUserId, this.LimitMessages);
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



