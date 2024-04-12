
// This is function for dynamically visible header
window.addEventListener("resize", handleWindowResize);
var pcWidth = false;
function handleWindowResize(){
    if (window.innerWidth > 800){
        pcWidth = true;
    }
}
if (pcWidth) {
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
}


handleNotification();
function handleNotification() {
    "use strict";
    var notificationConnection = new signalR.HubConnectionBuilder().withUrl("/notification").build();
    notificationConnection.start().then(function () {
    });
    notificationConnection.on("ReceiveNotification", function (msg) {
        console.log(notifications)
        var newData = notifications.data;
        newData.push(msg);
        console.log(newData);
        notifications.data = newData;
        // document.getElementById("notificationList").innerHTML += `<p>${msg}</p>`;
        // document.getElementById("notification").style.color = "red";
    });
}

var chatRoomWindow = document.createElement("div");
chatRoomWindow.className = "ChatWindow";
chatRoomWindow.innerHTML = `
<div id="chatroomList">
    <ul>
    </ul>
</div>`;
document.getElementById("navRight").addEventListener("click", (e) => {
    if (e.target.id == "userLink") {
        document.location.href = "/UserCenter";
    }
    else if (e.target.id == "notification") {
        showNotificationList["isVisible"] = !showNotificationList.isVisible;
        e.target.style.color = "black";
        e.preventDefault();
    }
});

var showNotificationList = new Proxy({
    isVisible: false
}, {
    get: (target, prop) => {
        return target[prop];
    },
    set: (target, prop, value) => {
        target[prop] = value;
        if (value) {
            document.getElementById("notificationList").style.display = "block";
        }
        else {
            document.getElementById("notificationList").style.display = "none";
        }
    }
});


document.getElementsByTagName("main")[0].addEventListener("mouseover", (e) => {
    if (e.target.classList.contains("ChatWindow")) {
        disableScroll();
    }
});
document.getElementsByTagName("main")[0].addEventListener("mouseleave", (e) => {
    if (e.target.classList.contains("ChatWindow")) {
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
    else if (e.target.id == "chatRoomMemberListBtn") {
        chatWindow["activeChatRoomMember"] = !chatWindow.activeChatRoomMember;
    }
}, true);
document.getElementsByTagName("main")[0].addEventListener("click", async (e) => {
    if (e.target.id == "addMemberBtn") {
        var res = await fetch("/api/ChatRoomMember", {
            method: "POST",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                ChatRoomId: chatWindow.activeChatRoom,
                userId: document.getElementById("userId").value
            })
        }).then(res => {
            return res.json();
        });
        if (res.success) {
            var res = await fetch(`/api/ChatRoomMember/${chatWindow.activeChatRoom}`, {
                method: "GET",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json"
                }
            }).then(res => {
                return res.json();
            });
            if (res.success) {
                document.getElementById("chatInput").innerHTML = `
                                <input type="text" id="userId">
                                <button id="addMemberBtn">Add</button>`;
                document.querySelector("#chatContent ul").innerText = "";
                res.payload.forEach((element) => {
                    var li = document.createElement("li");
                    li.innerText = `${element.user.id} ${element.user.name}`;
                    document.querySelector("#chatContent ul").appendChild(li);
                });
            }
        }
        else {
        }
    }
    if (e.target.id == "addChatRoom") {
        var res = await fetch("/api/ChatRoom", {
            method: "POST",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                name: document.getElementById("chatInputBox").value
            })
        }).then(res => {
            return res.json();
        });
        if (res.success) {
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
                document.getElementById("chatInput").innerHTML = `
                                <input type="text" id="chatInputBox">
                                <button id="addChatRoom">Add</button>`;
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
        }

    }
});
document.getElementById("navRight").addEventListener("click", async (e) => {
    if (e.target.id == "Chat") {
        chatWindow["isVisible"] = !chatWindow.isVisible;
        e.preventDefault();
    }
    else if (e.target.id === "logoutBtn") {
        e.preventDefault();
        var res = await fetch("/Logout", {
            method: "DELETE"
        }).then(res => {
            return res.json();
        }).then(ret => {
            if (ret.success) {
                document.location.href = "/Welcome"
            }
            else {
                e.target.innerText = "Fail!";
                setTimeout(() => {
                    e.target.innerText = "Logout"
                }, 3000);
                document.location.href = "/Welcome"
            }
        })
    }
    e.stopPropagation();
});
disableScroll = () => {
    document.querySelector(":root").style.overflow = "hidden";
}
enableScroll = () => {
    document.querySelector(":root").style.overflow = "auto";
}


var chatWindow = new Proxy({
    isVisible: false,
    activeChatRoom: 0,
    activeChatRoomMember: false
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
            <div id="chatroomList">
                <ul>
                </ul>
            </div>
            <div id="chatInput">
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
                document.getElementById("chatInput").innerHTML = `
                                <input type="text" id="chatInputBox">
                                <button id="addChatRoom">Add</button>`;
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
            <button id="backtoChatRoomListBtn">Back</button>
            <button id="chatRoomMemberListBtn">Members</button>
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
                    li.innerHTML = `${element.user.name} : ${element.message} <p class="timeIndicator">${new Date(element.created).toLocaleString('en-US', { timeZoneName: 'short' })}</p>`;
                    document.querySelector("#chatContent>ul").appendChild(li);
                });
                chatting(chatWindow.activeChatRoom.toString());
            }
            else {
                document.querySelector("#chatContent ul").innerText = "No Message";
                document.getElementById("chatInput").innerHTML = "";
            }
        }
        if (target.activeChatRoomMember) {
            document.getElementById("chatRoomMemberListBtn").innerText = "Message";
            var res = await fetch(`/api/ChatRoomMember/${chatWindow.activeChatRoom}`, {
                method: "GET",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json"
                }
            }).then(res => {
                return res.json();
            });
            if (res.success) {
                document.getElementById("chatInput").innerHTML = `
                                <input type="text" id="userId">
                                <button id="addMemberBtn">Add</button>`;
                document.querySelector("#chatContent ul").innerText = "";
                res.payload.forEach((element) => {
                    var li = document.createElement("li");
                    li.innerText = `${element.user.id} ${element.user.name}`;
                    document.querySelector("#chatContent>ul").appendChild(li);
                });
            }
            else {
                document.querySelector("#chatContent ul").innerText = "No Member";
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

    connection.on("ReceiveMessage", function (roomid, userid, user, message, time) {

        var li = document.createElement("li");
        li.innerHTML = `${user} : ${message} <p class="timeIndicator">${new Date(time).toLocaleString('en-US', { timeZoneName: 'short' })}</p>`;
        document.querySelector("#chatContent>ul").appendChild(li);
        li.scrollIntoView();

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
    });
    document.getElementById("chatRoomMemberListBtn").addEventListener("click", () => {
        connection.stop();
    });
}


// load avatar
fetch("/api/User/Avatar").then(res => {
    return res.json();
}).then(ret => {
    if (ret.success) {
        document.getElementById("userLink").src = `data:image/png;base64, ${ret.payload}`;
    }
});




// Fix This
// load user info popup
document.getElementById("navRight").addEventListener("mouseenter", async (e) => {
    if (e.target.id == "userLink") {
        var res = fetch("/api/User").then(res => {
            return res.text();
        }).then(async ret => {
            //    e.target.nextElementSibling.style.display = "block";
            e.target.nextElementSibling.innerHTML = await ret;

        }).then(() => {
            e.target.nextElementSibling.classList.add("userInfoShow");
        });
    }
});
document.getElementById("navRight").addEventListener("mouseleave", (e) => {
    if (e.target.id == "userLink") {
        e.target.nextElementSibling.classList.remove("userInfoShow");
        e.target.nextElementSibling.innerHTML = "";
    }

});

fetch("/Notifications").then(res => {
    return res.json();
}).then(ret => {
    if (ret.success) {
        notifications.data = ret.payload;
    }
});

var notifications = new Proxy({
    data: null
}, {
    get: function (target, prop) {
        return target[prop];
    },
    set: function (target, prop, value) {
        Reflect.set(target, prop, value);
        console.log("set");
        document.getElementById("notificationList").innerHTML = "<h4>notifications</h4>"
        var dt = target.data;
        for (i = 0; i < dt.length; i++) {
            document.getElementById("notificationList").innerHTML += `<a href="${dt[i].url}">${dt[i].type}</a><br>`;
        }
        return true;
    }
});


