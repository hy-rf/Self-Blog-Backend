
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
        if (res.success) {
            e.target.previousElementSibling.innerText = res.message;
            window.location.href = "/UserCenter";
        }
        else {
            e.target.previousElementSibling.innerText = res.message;
        }
    }
    // TODO : implement api at backend
    else if (e.target.id == "submitSignupBtn") {
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