
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
            <p></p>
            <button id="submitSignupBtn" type="submit">sign up</button>
         </div>`;
});

//document.getElementById("loginBtn").addEventListener("click", () => {
//    document.getElementById("togglePanel").innerHTML = document.getElementsByTagName("template")[0].innerHTML;
//});

//document.getElementById("signupBtn").addEventListener("click", () => {
//    document.getElementById("togglePanel").innerHTML = document.getElementsByTagName("template")[1].innerHTML;
//});






document.getElementById("togglePanel").addEventListener("click", async (e) => {
    if (e.target.id == "submitLoginBtn") {
        e.target.previousElementSibling.innerText = ".";
        var logging = true;
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
        }
    }
    else if (e.target.id == "submitSignupBtn") {
        e.target.previousElementSibling.innerText = ".";
        var signing = true;
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
                Pwd: document.getElementById("Pwd").value
            })
        }
        ).then(response => {
            return response.json();
        });
        signing = false;
        if (res.success) {
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
});