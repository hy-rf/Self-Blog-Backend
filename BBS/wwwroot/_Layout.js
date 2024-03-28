
// This is function for dynamically visible header
var prepos = window.scrollY;
window.onscroll = () => {
    var curpos = window.scrollY;
    if (prepos >= curpos) {
        document.querySelector("header").classList.remove("header_hide");
    }
    else {
        document.querySelector("header").classList.add("header_hide");
    }
    prepos = curpos;
}

readJson = (object) => {
    return fetch("/test", {
        method: "POST",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        body: JSON.stringify(object),
    }).then((response) => {
        return response.json();
    });
}

// This is function for dynamically visible chat room window
var chatRoomWindow = document.createElement("div");
chatRoomWindow.className = "ChatWindow";
chatRoomWindow.innerHTML = `    <link rel="stylesheet" href="/ChatRoom.css">
<div class="ChatWindow">
    <div class="ChatRoom">
        <div id="chatroomList">
            <ul>
            </ul>
        </div>
        <div id="chatroomMemberList">
            <ul>
            </ul>
        </div>
        <div id="addUsertoChatRoom">
            <input type="text" id="UserId" placeholder="User Id">
                <button>Add</button>
        </div>
    </div>
    <div class="Chat">
        <div id="chatContent">
            <ul>
            </ul>
        </div>
        <div id="chatInput">
            <input type="text" id="chatInputBox">
                <button id="sendButton">Send</button>
        </div>
    </div>
</div>`



document.getElementById("Chat").addEventListener("click", (e) => {
    if (toggleWindow["isVisible"]) {
        toggleWindow["isVisible"] = false;
    }
    else {
        toggleWindow["isVisible"] = true;
    }
    e.preventDefault();
});
var activeChatRoom = null;
var toggleWindow = new Proxy({
    isVisible: false
}, {
    get: (target, prop) => {
        return target[prop];
    },
    set: async (target, prop, value) => {
        target[prop] = value;
        if (value) {
            await document.getElementsByTagName("main")[0].appendChild(chatRoomWindow);
            var res = await fetch("/api/GetJoinedChatRoom", {
                method: "POST",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json"
                },
            }).then(response => {
                return response.json();
            });
            if (res.success) {
                res.payload.forEach((element) => {
                    var li = document.createElement("li");
                    li.innerText = parseInt(element.id) + element.name;
                    li.id = `chatroom_${element.id}`;
                    document.querySelector("#chatroomList ul").appendChild(li);
                });
            }
            else {
                document.querySelector("#chatroomList ul").innerText = "No Chat Room";
            }

            document.getElementById("chatroomList").addEventListener("click", async (e) => {
                if (e.target.tagName == "LI") {
                    var res = await fetch("/api/GetChatRoomMessages", {
                        method: "POST",
                        headers: {
                            "Accept": "application/json",
                            "Content-Type": "application/json"
                        },
                        body: JSON.stringify({
                            chatRoomId: e.target.id.split("_")[1]
                        })
                    }).then(res => {
                        return res.json();
                    });
                    if (res.success) {
                        activeChatRoom = e.target.id.split("_")[1];
                        document.querySelector("#chatContent ul").innerText = "";
                        res.payload.forEach((element) => {
                            var li = document.createElement("li");
                            li.innerText = `${element.user.name} says ${element.message}`;
                            document.querySelector("#chatContent ul").appendChild(li);
                        });
                        chatting();

                    }
                    else {
                        document.querySelector("#chatContent ul").innerText = "No Message";
                    }
                }
        });
        }
        else {
            document.getElementsByTagName("main")[0].removeChild(chatRoomWindow);
        }

        return true;
    }
});


function chatting() {

    "use strict";
    var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

    //Disable the send button until connection is established.
    document.getElementById("sendButton").disabled = true;

    connection.on("ReceiveMessage", function (roomid, userid, user, message) {
        var li = document.createElement("li");
        document.querySelector("#chatContent>ul").appendChild(li);
        // We can assign user-supplied strings to an element's textContent because it
        // is not interpreted as markup. If you're assigning in any other way, you
        // should be aware of possible script injection concerns.
        li.textContent = `${user} says ${message}`;
        li.scrollIntoView();
    });

    connection.start().then(function () {
        document.getElementById("sendButton").disabled = false;
    }).catch(function (err) {
        return console.error(err.toString());
    });

    document.getElementById("sendButton").addEventListener("click", function (event) {
        var roomid = activeChatRoom;
        var userid = document.getElementById("userlink").classList[0];
        var user = document.getElementById("userlink").innerText.split(" ")[1];
        var message = document.getElementById("chatInputBox").value;
        connection.invoke("SendMessage", roomid, userid, user, message).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });
}