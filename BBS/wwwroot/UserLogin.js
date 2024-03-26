

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
})