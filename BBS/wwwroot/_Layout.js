
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
var chatRoomWindow = document.createElement("div");
chatRoomWindow.className = "ChatWindow";
chatRoomWindow.innerHTML = `
<link rel="stylesheet" href="/ChatRoom.css">
<div id="chatroomList">
    <ul>
    </ul>
</div>`;


document.getElementsByTagName("main")[0].addEventListener("mouseover", (e) => {
    if (e.target.classList.contains("ChatWindow")) {
        e.target.style.opacity = 1;
        disableScroll();
    }
});
document.getElementsByTagName("main")[0].addEventListener("mouseleave", (e) => {
    if (e.target.classList.contains("ChatWindow")) {
        e.target.style.opacity = 0.5;
        enableScroll();
    }
}, true);
document.getElementsByTagName("main")[0].addEventListener("click", async (e) => {
    if (e.target.tagName == "LI" && !chatWindow.activeChatRoom) {
        chatWindow["activeChatRoom"] = e.target.id.split("_")[1];
    }
    else if (e.target.id == "backtoChatRoomListBtn") {
        chatWindow["isVisible"] = true;
        chatWindow["activeChatRoom"] = 0;
    }
}, true);
document.getElementById("user").addEventListener("click", (e) => {
    if (e.target.id == "Chat") {
        chatWindow["isVisible"] = !chatWindow.isVisible;
        e.preventDefault();
    }
});
disableScroll = () => {
    document.querySelector(":root").style.overflow = "hidden";
}
enableScroll = () => {
    document.querySelector(":root").style.overflow = "auto";
}


var chatWindow = new Proxy({
    isVisible: false,
    activeChatRoom: 0
}, {
    get: (target, prop) => {
        return target[prop];
    },
    set: async (target, prop, value) => {
        target[prop] = value;
        console.log(target);
        // When chat room window is visible do this
        if (target.isVisible) {
            if (document.getElementsByTagName("main")[0].getElementsByClassName("ChatWindow").length == 0) {
                await document.getElementsByTagName("main")[0].appendChild(chatRoomWindow);
            }
            chatRoomWindow.innerHTML = `
            <link rel="stylesheet" href="/ChatRoom.css">
            <div id="chatroomList">
                <ul>
                </ul>
            </div>`;
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
                document.querySelector("#chatroomList ul").innerText = "";
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
        }
        else {
            document.getElementsByTagName("main")[0].removeChild(chatRoomWindow);
        }
        // When there is active chat room do this
        if (target.activeChatRoom) {
            chatRoomWindow.innerHTML = `
            <link rel="stylesheet" href="/ChatRoom.css">
            <button id="backtoChatRoomListBtn">back</button>
            <div id="chatContent">
                <ul>
                </ul>
            </div>
            <div id="chatInput">
            </div>`;
            var res = await fetch("/api/GetChatRoomMessages", {
                method: "POST",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    chatRoomId: target.activeChatRoom
                })
            }).then(res => {
                return res.json();
            });
            if (res.success) {
                document.getElementById("chatInput").innerHTML = `
                                <input type="text" id="chatInputBox">
                                <button id="sendButton">Send</button>`;
                document.querySelector("#chatContent ul").innerText = "";
                res.payload.forEach((element) => {
                    var li = document.createElement("li");
                    li.innerText = `${element.user.name} says ${element.message}`;
                    document.querySelector("#chatContent ul").appendChild(li);
                });
                chatting(chatWindow.activeChatRoom.toString());
            }
            else {
                document.querySelector("#chatContent ul").innerText = "No Message";
                document.getElementById("chatInput").innerHTML = "";
            }
        }
        return true;
    }
});


function chatting(RoomId) {

    "use strict";
    var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

    //Disable the send button until connection is established.
    document.getElementById("sendButton").disabled = true;

    connection.on("ReceiveMessage", function (roomid, userid, user, message) {

        if (user != document.getElementById("userlink").innerText.split(" ")[1] || true) {
            var li = document.createElement("li");
            document.querySelector("#chatContent>ul").appendChild(li);
            li.textContent = `${user} says ${message}`;
            li.scrollIntoView();
        }
    });

    connection.start().then(function () {
        connection.invoke("Join", RoomId);
        document.getElementById("sendButton").disabled = false;
    }).catch(function (err) {
        return console.error(err.toString());
    });

    document.getElementById("sendButton").addEventListener("click", function (event) {
        var roomid = chatWindow.activeChatRoom;
        var userid = document.getElementById("userlink").classList[0];
        var user = document.getElementById("userlink").innerText.split(" ")[1];
        var message = document.getElementById("chatInputBox").value;
        connection.invoke("SendMessage", roomid, userid, user, message).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });
    document.getElementById("backtoChatRoomListBtn").addEventListener("click", () => {
        connection.stop();
    })
}





// readJson = (object) => {
//     return fetch("/test", {
//         method: "POST",
//         headers: {
//             "Accept": "application/json",
//             "Content-Type": "application/json"
//         },
//         body: JSON.stringify(object),
//     }).then((response) => {
//         return response.json();
//     });
// }