
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
        if (/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}/.test(e.target.value)) {
            document.querySelectorAll("#pwdSecurityIndicator>div")[0].style.backgroundColor = "red";
            document.querySelectorAll("#pwdSecurityIndicator>div")[1].style.backgroundColor = "yellow";
            document.querySelectorAll("#pwdSecurityIndicator>div")[2].style.backgroundColor = "green";
        }
        else if (/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{6,}/.test(e.target.value)) {
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
}
function enableInput() {
    document.querySelectorAll("#togglePanel>div>input")[0].disabled = false;
    document.querySelectorAll("#togglePanel>div>input")[1].disabled = false;
    document.querySelector("#togglePanel>div>button").disabled = false;
}