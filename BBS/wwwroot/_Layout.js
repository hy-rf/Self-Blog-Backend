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

document.getElementById("searchOptions").addEventListener("change", (e) => {
    document.getElementById("searchbox").setAttribute("placeholder", e.target.value);
});

var floating = false;
document.querySelector("main").addEventListener("mousedown", async (e) => {

    if (e.target.classList.contains("UserInfo")) {
        var id = parseInt(e.target.firstElementChild.innerText);
        response = () => {
            return fetch(`/User/${id}`, {
                method: "GET",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json"
                },
            }).then(response => {
                return response.json();
            });
        }
        response().then(res => {
            var ele = document.createElement("div");
            ele.innerHTML = `<p>${res["id"]}</p><p>${res["name"]}</p><a href="/User/UserPage/${res["id"]}">go to its page</a><img src="data:image/png;base64, ${res["avatar"]}" width="64" height="64"><button>close</button>`;
            e.target.appendChild(ele);
            ele.lastChild.addEventListener("click", () => {
                ele.remove();
            })
        });

    }
});

document.querySelector("#friendRequestsReceived").addEventListener("click", (e) => {
    if (e.target.tagName == "BUTTON") {
        alert(e.target.parentNode.firstElementChild.innerText);
        //send approve request and add friend to database
        fetch("/Friend", {
            method: "POST",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                Id: parseInt(e.target.parentNode.firstElementChild.innerText)
            })
        }).then(response => {
            e.target.innerText = "success"
        }).catch(error => {
            e.target.innerText = "fail"
        });
    }
});