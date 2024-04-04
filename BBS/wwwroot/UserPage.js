document.getElementById("Detail").addEventListener("click", (e) => {
    if (e.target.id == "sendFriendRequest") {
        fetch(`/Friend/${document.getElementById("Id").innerText}`, {
            method: "POST"
        }).then(response => {
            e.target.innerText = "success"
        }).catch(response => {
            e.target.innerText = "fail"
        });
    }

});