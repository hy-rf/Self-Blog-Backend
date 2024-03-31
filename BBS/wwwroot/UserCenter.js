
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
document.querySelector("label[for='updateAvatarBtn']").addEventListener("click", (e) => {
    if (e.target.nextElementSibling.disabled == true) {
        document.querySelector("label[for='avatar']").animate([
            { borderColor: "red" },
            { transition: "border-color 0.5s"}
        ],{
            duration: 1000,
        })
    }
});

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
            "Accept": "applicatoin/json",
            "Content-Type": "application/json"
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

// Get 2fQR
document.getElementById("generate2FQRCode").addEventListener("click", async (e) => {
    e.preventDefault();
    let res = await fetch("/api/2fv", {
        method: "POST",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json",
        },
    }).then(res => {
        return res.json();
    });
    if (res.success) {
        document.getElementById("TwoFactorQRCode").src = res.payload["qrCodeUrl"];
    }
});
// Validate 2fQR
document.getElementById("submit2FC").addEventListener("click", async (e) => {
    let code = document.getElementById("2FCode").value;
    var res = await fetch("/apt/2fv/vali", {
        method: "POST",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            "code": code
        })
    }).then(res => {
        return res.json();
    });
    if (res.success) {
        e.target.innerText = "Success!"
    }
    else {
        e.target.innerText = "Failed!"
    }
})
