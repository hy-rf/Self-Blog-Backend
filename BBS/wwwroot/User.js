
async function UploadFile(FormEle) {
    var resultElement = FormEle.elements.namedItem("result");
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

document.querySelector("#avatar").addEventListener("change", () => {
    const [file] = document.querySelector("#avatar").files;
    if (file) {
        document.querySelector('#Preview').src = URL.createObjectURL(file);
    }
    document.querySelector("form>button").removeAttribute("disabled");
});

BindEditNameEvent = () => {
    document.querySelector('#name').addEventListener('click', () => {
        var ele = document.querySelector('#name');
        ele.outerHTML = `<input id="inputName" value="${ele.innerHTML}"></input><button id="changeNameButton">Update Name</button>`;
        document.getElementById("changeNameButton").addEventListener("click", () => { EditName(document.querySelector('#Id').innerText.split(':')[1]) });
    });
}
BindEditNameEvent();



EditName = (Id) => {
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
        return;
    });
    var ele = document.querySelector('#inputName');
    ele.outerHTML = `<span id="name">${ele.value}</span>`;
    document.getElementById("changeNameButton").remove();
    BindEditNameEvent();
    document.getElementById('Title').innerText = `${ele.value}'s Info:`;
    document.getElementById('userlink').innerText = `Hello! ${ele.value}`;
}


getFriendList = () => {
    return fetch(`/FriendList/${document.getElementById("Id").innerText.split(":")[1].toString()}`).then(response => {
        return response.json();
    }).then(ret => {
        return ret.payload;
    }).catch(error => {
        console.log(error);
    });
}

getChatRoomList = () => {
    return fetch(`/Chat/GetChatRooms`, {
        method: "POST",
        headers: {
            "Accept":"applicatoin/json",
            "Content-Type":"application/json"
        },
        body: JSON.stringify({
            Id: document.getElementById("Id").innerText.split(":")[1].toString()
        })
    }).then(response => {
        return response.json();
    }).catch(error => {
        console.log(error);
    });
}


window.onload = async () => {
    // Grid E
    var ret = await getFriendList();
    console.log(ret[0]);
    var friendlist = "";
    var ele = document.createElement("div");
    ele.setAttribute("id", "friends");
    ele.setAttribute("style", "grid-area:E;")
    for (i = 0; i < ret.length; i++) {
        friendlist += `<a href="/User/${ret[i].friendUser.id}">${ret[i].friendUser.name}</a>
        <p>Joined at ${ret[i].friendUser.created}</p>
        <img src="data:image/png;base64, ${ret[i].friendUser.avatar}" width="64" height="64"><br>`;
    }
    ele.innerHTML = friendlist;
    document.querySelector("#User").appendChild(ele);


    // Grid F
    var ret2 = await getChatRoomList();
    console.log(ret2[0]);
    var chatroomlist = "";
    var ele2 = document.createElement("div");
    ele2.setAttribute("id", "chatrooms");
    ele2.setAttribute("style", "position:relativel;grid-area:F;");
    for (i = 0; i < ret2.length; i++) {
        chatroomlist += `<a href="/ChatRoom/${ret2[i].id}">go to ${ret2[i].name}</a>`;
    }
    ele2.innerHTML = chatroomlist;
    document.querySelector("#User").appendChild(ele2);
}