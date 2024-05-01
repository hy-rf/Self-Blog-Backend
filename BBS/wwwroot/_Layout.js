
// This is function for dynamically visible header

var prepos = window.scrollY;

window.onscroll = () => {
    if (window.innerWidth > 800) {
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


// load notification
fetch("/Notifications").then(res => {
    return res.json();
}).then(ret => {
    if (ret.success) {
        notifications.data = ret.payload;
    }
});
// load avatar
fetch("/api/User/Avatar").then(res => {
    return res.json();
}).then(ret => {
    if (ret.success) {
        document.getElementById("userLink").src = `data:image/png;base64, ${ret.payload}`;
    }
});

var chatRoomWindow = document.createElement("div");
chatRoomWindow.className = "ChatWindow";





document.getElementsByTagName("main")[0].addEventListener("click", handleChatRoom);
document.querySelector("header").addEventListener("click", handleHeaderLinkActions, true);

// handle mouse in chat window scroll behavior

document.getElementsByTagName("main")[0].addEventListener("mouseover", (e) => {
    if (e.target.classList.contains("ChatWindow")) {
        disableScroll();
    }
});
// handle mouse out of chat window scroll behavior
document.getElementsByTagName("main")[0].addEventListener("mouseleave", (e) => {
    if (e.target.classList.contains("ChatWindow")) {
        enableScroll();
    }
}, true);

// handle routes in chatroom
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
        var isLoading = false;
        // When chat room window is visible do this
        if (target.isVisible) {
            if (document.getElementsByTagName("main")[0].getElementsByClassName("ChatWindow").length == 0) {
                await document.getElementsByTagName("main")[0].appendChild(chatRoomWindow);
            }
            chatRoomWindow.innerHTML = `
            <div id="topLinks">
                <p>Chatrooms</p>
            </div>
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
            <div id="topLinks">
                <button id="backtoChatRoomListBtn">Back</button>
                <button id="chatRoomMemberListBtn">Members</button>
            </div>
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
                document.querySelector("#chatContent ul").innerHTML = "";
                res.payload.forEach((element) => {
                    var li = document.createElement("li");
                    li.innerHTML = chatMessageUnit(element.user.name, element.message, element.created);
                    document.querySelector("#chatContent>ul").appendChild(li);
                });
                chatting(chatWindow.activeChatRoom.toString());
            }
            else {
                document.querySelector("#chatContent ul").innerText = "No Message";
                document.getElementById("chatInput").innerHTML = "";
            }
        }
        // if user is in chat room member list do this
        if (target.activeChatRoomMember) {
            document.querySelector("#chatContent ul").innerText = "";
            document.getElementById("chatRoomMemberListBtn").innerText = "Message";
            document.getElementById("chatInput").innerHTML = `
                                <input type="text" id="userId">
                                <button id="addMemberBtn">Add</button>`;
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
            document.getElementById("notificationList").innerHTML += notificationUnit(dt[i].url, dt[i].type);
        }
        return true;
    }
});


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

function chatting(RoomId) {

    "use strict";
    var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

    //Disable the send button until connection is established.
    document.getElementById("sendButton").disabled = true;

    connection.on("ReceiveMessage", function (roomid, userid, user, message, time) {

        var li = document.createElement("li");
        li.innerHTML = chatMessageUnit(user, message, time);
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

function notificationUnit(url, type) {
    return `<a href="${url}">${type}</a><br>`;
}

function chatMessageUnit(name, message, created) {
    return `${name} : ${message} <p class="timeIndicator">${new Date(created).toLocaleString('en-US', { timeZoneName: 'short' })}</p>`
}

function disableScroll() {
    document.querySelector(":root").style.overflow = "hidden";
}

function enableScroll() {
    document.querySelector(":root").style.overflow = "auto";
}

async function handleChatRoom(e) {
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
        // if add chatroom success refresh joined chatroom
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
}

async function handleHeaderLinkActions(e) {
    if (e.target.id === "chat") {
        chatWindow["isVisible"] = !chatWindow.isVisible;
        e.preventDefault();
    }
    if (e.target.id === "logoutBtn") {
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
    if (e.target.id === "userLink") {
        document.location.href = "/UserCenter";
    }
    if (e.target.id === "notification") {
        console.log("click");
        showNotificationList["isVisible"] = !showNotificationList.isVisible;
        e.target.style.color = "black";
        e.preventDefault();
    }
    e.stopPropagation();
}

function handleLoading(target) {
    if (isLoading) {
        target.innerText += ".";
        if (target.innerText.innerText.length >= 4) {
            target.innerText.innerText = ".";
        }
    }
}

if (document.location.href.split("/")[3] === "FriendRequests") {
    document.querySelector("#friendRequestsReceived").addEventListener("click", (e) => {
        if (e.target.tagName == "BUTTON") {
            fetch("/Friend", {
                method: "POST",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    Id: parseInt(e.target.parentNode.id)
                })
            }).then(response => {
                e.target.innerText = "success"
            }).catch(error => {
                e.target.innerText = "fail"
            });
        }
    });
}
if (document.location.href.split("/")[3] === "UserCenter") {
    (async () => {
        function EditName(e) {
            var newName = document.getElementById('inputName').value;
            fetch(`/User/EditName`, {
                method: "POST",
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    Name: newName
                }),
            }).then(response => {
                location.reload();
                return;
            });
        }
        function getFriendList() {
            return fetch(`/FriendList/${document.getElementById("Id").innerText.split(":")[1].toString()}`).then(response => {
                return response.json();
            }).then(ret => {
                return ret.payload;
            }).catch(error => {
                console.log(error);
            });
        }
        function getLikedPosts() {
            return fetch(`/Like/${document.getElementById("Id").innerText.split(":")[1].toString()}`).then(response => {
                return response.json();
            }).then(ret => {
                return ret.payload;
            }).catch(error => {
                console.log(error);
            });
        }


        var ret = await getFriendList();
        var friendlist = "";
        var ele = document.createElement("div");
        ele.setAttribute("id", "friends");
        ele.setAttribute("style", "grid-area:E;");
        for (i = 0; i < ret.length; i++) {
            friendlist += `<img src="data:image/png;base64, ${ret[i].friendUser.avatar}" width="64" height="64">
            <a href="/User/${ret[i].friendUser.id}">${ret[i].friendUser.name}</a>`;
        }
        ele.innerHTML = friendlist;
        document.querySelector("#User").appendChild(ele);

        var ret2 = await getLikedPosts();
        var likedPosts = "<h3>Liked Posts</h3>";
        var ele2 = document.createElement("div");
        //ele2.setAttribute("id", "chatrooms");
        ele2.setAttribute("style", "position:relativel;grid-area:F;");
        for (i = 0; i < ret2.length; i++) {
            likedPosts += `<a href="/Post/Detail/${ret2[i].postId}">${ret2[i].post.title}</a>`;
        }
        ele2.innerHTML = likedPosts;
        document.querySelector("#User").appendChild(ele2);


        document.querySelector('#name').addEventListener('click', () => {
            var ele = document.querySelector('#name');
            ele.outerHTML = `<input id="inputName" value="${ele.innerHTML}"></input><button id="changeNameButton">Update Name</button>`;
            document.getElementById("changeNameButton").addEventListener("click", EditName);
        });
        // warn when click on update avatar button while no file uploaded
        document.querySelector("label[for='updateAvatarBtn']").addEventListener("click", (e) => {
            if (e.target.nextElementSibling.disabled == true) {
                document.querySelector("label[for='avatar']").animate([
                    { borderColor: "red" },
                    { transition: "border-color 0.5s" }
                ], {
                    duration: 1000,
                })
            }
        });

        // preview avatar
        document.querySelector("#avatar").addEventListener("change", () => {
            const [file] = document.querySelector("#avatar").files;
            if (file) {
                document.querySelector('#Preview').src = URL.createObjectURL(file);
            }
            document.querySelector("form>button").removeAttribute("disabled");
        });
    })();
    async function UploadFile(FormEle) {
        const formData = new FormData(FormEle);
        try {
            const response = await fetch(FormEle.action, {
                method: 'POST',
                body: formData
            });
            location.reload();
        } catch (error) {
            console.error('Error:', error);
        }
    }
}


if (document.location.href.split("/")[3] === "User") {
    document.getElementById("Detail").addEventListener("click", (e) => {
        if (e.target.id == "sendFriendRequest") {
            fetch(`/Friend/${document.getElementById("Id").innerText.split(':')[1]}`, {
                method: "POST"
            }).then(response => {
                return response.json();
            }).then(ret => {
                if (ret.success) {
                    e.target.innerText = "Request Sent";
                }
                else {
                    e.target.innerText = "Request Failed";
                }
            });
        }
    });
}

if (document.location.href.split("/")[3] === "Welcome") {
    // click to view login or signup panel
    document.getElementById("loginBtn").addEventListener("click", () => {
        document.getElementById("togglePanel").innerHTML =
            `<div>
            <label for="Name">Name</label>
            <input id="Name" name="Name" />
            <label for="Pwd">Pwd</label>
            <input id="Pwd" name="Pwd" type="password" />
            <p></p>
            <button id="submitLoginBtn" type="submit">login</button>
        </div>`;
    });
    document.getElementById("signupBtn").addEventListener("click", () => {
        document.getElementById("togglePanel").innerHTML =
            `<div>
            <label for="Name">Name</label>
            <input id="Name" name="Name" required />
            <label for="Pwd">Pwd</label>
            <input id="Pwd" name="Pwd" type="password" required />
            <input id="RePwd" name="RePwd" type="password" required />
            <p>Password Security</p>
            <div id="pwdSecurityIndicator">
                <div></div><div></div><div></div>
            </div>
            <p></p>
            <button id="submitSignupBtn" type="submit">sign up</button>
         </div>`;
    });
    // submit login or signup
    document.getElementById("togglePanel").addEventListener("click", async (e) => {
        if (e.target.id == "submitLoginBtn") {
            e.target.previousElementSibling.innerText = ".";
            var logging = true;
            disableInput();
            setInterval(() => {
                if (logging) {
                    e.target.previousElementSibling.innerText += ".";
                    if (e.target.previousElementSibling.innerText.length >= 4) {
                        e.target.previousElementSibling.innerText = ".";
                    }
                }
            }, 100);
            var res = await fetch("/api/User/Login", {
                method: "POST",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    Name: document.getElementById("Name").value,
                    Pwd: document.getElementById("Pwd").value
                })
            }
            ).then(response => {
                return response.json();
            });
            logging = false;
            if (res.success) {
                e.target.previousElementSibling.innerText = res.message + " Redirecting";
                setInterval(() => {
                    e.target.previousElementSibling.innerText += ".";
                    if (e.target.previousElementSibling.innerText.length >= 30) {
                        e.target.previousElementSibling.innerText = res.message + " Redirecting";
                    }
                }, 100);
                window.location.href = "/UserCenter";
            }
            else {
                e.target.previousElementSibling.innerText = res.message;
                enableInput()
            }
        }
        else if (e.target.id == "submitSignupBtn") {
            e.target.previousElementSibling.innerText = ".";
            var signing = true;
            disableInput();
            setInterval(() => {
                if (signing) {
                    e.target.previousElementSibling.innerText += ".";
                    if (e.target.previousElementSibling.innerText.length >= 4) {
                        e.target.previousElementSibling.innerText = ".";
                    }
                }
            }, 100);
            var res = await fetch("/api/User/Signup", {
                method: "POST",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    Name: document.getElementById("Name").value,
                    Pwd: document.getElementById("Pwd").value,
                    RePwd: document.getElementById("RePwd").value
                })
            }
            ).then(response => {
                return response.json();
            });
            signing = false;
            if (res.success) {
                e.target.previousElementSibling.innerText = res.message + " Redirecting";
                setInterval(() => {
                    e.target.previousElementSibling.innerText += ".";
                    if (e.target.previousElementSibling.innerText.length >= res.message.length + 16) {
                        e.target.previousElementSibling.innerText = res.message + " Redirecting";
                    }
                }, 100);
                await fetch("/api/User/Login", {
                    method: "POST",
                    headers: {
                        "Accept": "application/json",
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({
                        Name: document.getElementById("Name").value,
                        Pwd: document.getElementById("Pwd").value
                    })
                });
                window.location.href = "/UserCenter";
            }
            else {
                e.target.previousElementSibling.innerText = res.message;
                enableInput()
            }
        }
    });
    // check if Name is used
    document.getElementById("togglePanel").addEventListener("keyup", async (e) => {
        if (e.target.id == "Name" && e.target.parentNode.querySelector("button").id == "submitSignupBtn") {
            var name = e.target.value;
            var nameAvailable = await fetch("/api/User/CheckDuplicatedName", {
                method: "POST",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    Name: name
                })
            }).then(response => {
                return response.json();
            }).then(ret => {
                return ret.success;
            });
            if (nameAvailable) {
                e.target.style.color = "green";
            }
            else {
                e.target.style.color = "red";
            }
        }
        else if (e.target.id == "Pwd" && e.target.parentNode.querySelector("button").id == "submitSignupBtn") {
            if (/^(.*[A-Z]){1,}/.test(e.target.value) && /^(.*[a-z]){1,}/.test(e.target.value) && /^(.*\d){1,}/.test(e.target.value) && /^(.*[~!@#$%^&*]){1,}/.test(e.target.value)) {
                document.querySelectorAll("#pwdSecurityIndicator>div")[0].style.backgroundColor = "red";
                document.querySelectorAll("#pwdSecurityIndicator>div")[1].style.backgroundColor = "yellow";
                document.querySelectorAll("#pwdSecurityIndicator>div")[2].style.backgroundColor = "green";
            }
            else if (/^(.*[A-Z]){1,}/.test(e.target.value) && /^(.*[a-z]){1,}/.test(e.target.value) && /^(.*\d){1,}/.test(e.target.value) || /^(.*[~!@#$%^&*]){1,}/.test(e.target.value)) {
                document.querySelectorAll("#pwdSecurityIndicator>div")[0].style.backgroundColor = "red";
                document.querySelectorAll("#pwdSecurityIndicator>div")[1].style.backgroundColor = "yellow";
                document.querySelectorAll("#pwdSecurityIndicator>div")[2].style.backgroundColor = "transparent";
            }
            else {
                document.querySelectorAll("#pwdSecurityIndicator>div")[0].style.backgroundColor = "red";
                document.querySelectorAll("#pwdSecurityIndicator>div")[1].style.backgroundColor = "transparent";
                document.querySelectorAll("#pwdSecurityIndicator>div")[2].style.backgroundColor = "transparent";
            }
        }
    });
    function disableInput() {
        document.querySelectorAll("#togglePanel>div>input")[0].disabled = true;
        document.querySelectorAll("#togglePanel>div>input")[1].disabled = true;
        document.querySelector("#togglePanel>div>button").disabled = true;
        document.getElementById("loginBtn").disabled = true;
        document.getElementById("signupBtn").disabled = true;
    }
    function enableInput() {
        document.querySelectorAll("#togglePanel>div>input")[0].disabled = false;
        document.querySelectorAll("#togglePanel>div>input")[1].disabled = false;
        document.querySelector("#togglePanel>div>button").disabled = false;
        document.getElementById("loginBtn").disabled = false;
        document.getElementById("signupBtn").disabled = false;
    }
}



if (document.location.href.split("/")[3] === "Post") {



    document.getElementById("PostList").addEventListener("click", async (e) => {
        if (e.target.classList.contains("LikeBtn")) {
            var res = await fetch("/Like/Post", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    PostId: parseInt(e.target.parentElement.firstElementChild.innerText)
                })
            }).then(response => {
                return response.json();
            });
            if (res.success) {
                console.log(e.target.getAttribute("fill"))
                if (e.target.getAttribute("fill") == "#000000") {
                    e.target.setAttribute("fill", "#ffffff");
                    e.target.children[2].children[0].firstElementChild.setAttribute("fill", "#ff0000");
                }
                else {
                    e.target.setAttribute("fill", "#000000");
                    e.target.children[2].children[0].firstElementChild.setAttribute("fill", "#808184");
                }
            }
            else {
                e.target.innerText = res.message;
            }
        }
        e.stopPropagation();
    }, true);

    document.getElementById("Tag").addEventListener("keydown", (e) => {
        if (e.key === " " || e.key === "Enter") {
            var tagArray = e.target.value.split(" ").filter((item) => item != "");
            e.target.value = tagArray.map(tag => tag.startsWith("#") ? tag : "#" + tag).join(" ");
        }
        else if (e.key === "#") {
            e.target.value = e.target.value.slice(0, -1);
        }
    })
    if (document.location.href.split("/")[4] === "Detail") {

        var PostContent;
        const quill = new Quill('#PostContent', {
            readOnly: true,
        });


        const quillEdit = new Quill('#EditPostForm', {
            modules: {
                toolbar: ['bold', 'italic', 'underline', 'strike', 'link', 'image', 'code-block', 'video', 'blockquote', 'clean']
            },
            placeholder: 'Compose an epic...',
            theme: 'snow'
        });

        document.getElementById("toggleEditPostBtn").addEventListener("click", () => {
            document.getElementById("PostFormWrapper").classList.toggle("hideEditPost");
        });

        // Get Post Content then set it to quill editors
        fetch(`/api/Post/${document.location.href.split("/")[5]}`).then(res => {
            return res.json();
        }).then(ret => {
            if (ret.success) {
                PostContent = JSON.parse(ret.payload)
                quill.setContents(PostContent);
                quillEdit.setContents(PostContent);
            }
        });



        // Set Tag and Title to the input fields in the edit form
        document.getElementById("Tag").value = (document.getElementById("Tags") != null) ? document.getElementById("Tags").innerText.split(" ").join("") : "";
        document.getElementById("Title").value = document.getElementsByClassName("Title")[0].innerText;

        document.getElementById("submitEditPost").addEventListener("click", (e) => {
            fetch(`/Post/EditPost`, {
                method: "POST",
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    PostId: parseInt(parseInt(document.location.href.split("/")[5])),
                    Title: document.getElementById("Title").value,
                    Tag: document.getElementById("Tag").value,
                    Content: quillEdit.getContents()
                }),
            }).then(response => {
                return response.json();
            }).then(ret => {
                if (ret.success) {
                    location.reload();
                }
                else {
                    alert(ret.message);
                }
            });
        });
    } else {

        const quillEdit = new Quill('#CreatePostForm', {
            modules: {
                toolbar: ['bold', 'italic', 'underline', 'strike', 'link', 'image', 'code-block', 'video', 'blockquote', 'clean']
            },
            placeholder: 'Compose an epic...',
            theme: 'snow'
        });
        document.getElementById("submitPost").addEventListener("click", (e) => {
            fetch(`/Post/CreatePost`, {
                method: "POST",
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    Title: document.getElementById("Title").value,
                    Tag: document.getElementById("Tag").value,
                    Content: quillEdit.getContents()
                }),
            }).then(response => {
                return response.json();
            }).then(ret => {
                if (ret.success) {
                    location.reload();
                }
                else {
                    alert(ret.message);
                }
            });
        });
        document.getElementById("toggleCreatePostBtn").addEventListener("click", () => {
            document.getElementById("PostFormWrapper").classList.toggle("showCreatePost");
        });


    }
}