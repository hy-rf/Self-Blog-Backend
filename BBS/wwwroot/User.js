
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
    fetch(`/User/EditName/${Id}`, {
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
    }).catch(error => {
        console.log(error);
    });
}

getChatRoomList = () => {
    return fetch(`/ChatRoomList/${document.getElementById("Id").innerText.split(":")[1].toString()}`).then(response => {
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
        friendlist += `<p>${ret[i].name}</p>
        <p>${ret[i].created}</p>
        <img src="data:image/png;base64, ${ret[i].avatar}" width="64" height="64">
        <button id="">chat with ${ret[i].name} </button>`;
    }
    ele.innerHTML = friendlist;
    document.querySelector("#User").appendChild(ele);


    // Grid F
    var ret2 = await getChatRoomList();
    var chatroomlist = "";
    var ele2 = document.createElement("div");
    ele2.setAttribute("id", "chatrooms");
    ele2.setAttribute("style", "position:relativel;grid-area:F;");
}