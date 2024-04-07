document.getElementById("Detail").addEventListener("click", (e) => {
    if (e.target.id == "sendFriendRequest") {
        fetch(`/Friend/${document.getElementById("Id").innerText}`, {
            method: "POST"
        }).then(response => {
            return response.json();
        }).then(ret => {
            if (ret.success) {
                e.target.innerText = "Request Sent";
            }
            else {
                e.target.innerText = "Request Failed";
            }
        });
    }
});