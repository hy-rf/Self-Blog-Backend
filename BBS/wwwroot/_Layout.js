
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
    }).catch((err) => {
        console.log(err);
    });
}




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
    set: (target, prop, value) => {
        target[prop] = value;
        if (value) {
            document.getElementsByTagName("main")[0].appendChild(document.createElement("chat-window"));
        }
        else {
            document.getElementsByTagName("main")[0].removeChild(document.getElementsByTagName("chat-window")[0]);
        }
        return true;
    }
});

class chatWindow extends HTMLElement {
    constructor() {
        super();
        this.attachShadow({ mode: "open" });
        this.shadowRoot.innerHTML =
            `<link rel="stylesheet" href="/ChatRoom.css">
            <div class="ChatWindow">
            <div class="ChatRoom">
                <div id="chatroomMemberList">
                    <ul>
                        <li>
                            <span>1</span>
                            <button>kick</button>
                        </li>
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
                        <li>
                            <span>1</span>
                            <span>1</span>
                        </li>
                    </ul>
                </div>
                <div id="chatInput">
                    <input type="text" id="chatInputBox">
                    <button>Send</button>
                </div>
            </div>
        </div>
        <script src="/ChatRoom.js"></script>`;
    }
}

customElements.define("chat-window", chatWindow);