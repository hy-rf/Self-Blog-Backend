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
    ele.setAttribute("style", "grid-area:E;")
    for (i = 0; i < ret.length; i++) {
        friendlist += `<a href="/User/${ret[i].friendUser.id}">${ret[i].friendUser.name}</a>
        <p>Joined at ${ret[i].friendUser.created}</p>
        <img src="data:image/png;base64, ${ret[i].friendUser.avatar}" width="64" height="64"><br>`;
    }
    ele.innerHTML = friendlist;
    document.querySelector("#User").appendChild(ele);

    var ret2 = await getLikedPosts();
    var likedPosts = "<h3>Liked Posts</h3>";
    var ele2 = document.createElement("div");
    //ele2.setAttribute("id", "chatrooms");
    ele2.setAttribute("style", "position:relativel;grid-area:F;");
    for (i = 0; i < ret2.length; i++) {
        likedPosts += `<a href="/Post/Detail/${ret2[i].post.id}">${ret2[i].post.title}</a>`;
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



    //////////////////////////////////////////////////////////////////////////////////////////////////// Experimental 2FV
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
})();


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



















