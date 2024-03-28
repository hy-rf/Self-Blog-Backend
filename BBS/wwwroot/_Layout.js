
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
                <button>Send</button>
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
            //document.getElementById("chatroomList").dispatchEvent(new Event("change"));
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
        return true;
    }
});

