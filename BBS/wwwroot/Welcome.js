
document.getElementById("loginBtn").addEventListener("click", () => {
    document.getElementById("togglePanel").innerHTML =
        `<div>
            <label for="Name">Name</label>
            <input id="Name" name="Name" />
            <label for="Pwd">Pwd</label>
            <input id="Pwd" name="Pwd" type="password" />
            <button id="submitLoginBtn" type="submit">login</button>
        </div>`;
});

document.getElementById("signupBtn").addEventListener("click", () => {
    document.getElementById("togglePanel").innerHTML =
        `<form method="POST" action="/Signup">
            <label for="Name">Name</label>
            <input id="Name" name="Name" required />
            <label for="Pwd">Pwd</label>
            <input id="Pwd" name="Pwd" type="password" required />
            <button id="submitSignupBtn" type="submit">sign up</button>
         </form>`;
});

//document.getElementById("loginBtn").addEventListener("click", () => {
//    document.getElementById("togglePanel").innerHTML = document.getElementsByTagName("template")[0].innerHTML;
//});

//document.getElementById("signupBtn").addEventListener("click", () => {
//    document.getElementById("togglePanel").innerHTML = document.getElementsByTagName("template")[1].innerHTML;
//});


document.getElementById("submitSignupBtn").addEventListener("click", async (e) => {
    console.log("submitSignupBtn clicked");
});



document.getElementById("welcomeWrapper").addEventListener("click", async (e) => {
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
            window.location.href = "/UserCenter";
        }
        else {
            document.getElementById("submitLoginBtn").innerText = "Login Failed";
        }
    }
});