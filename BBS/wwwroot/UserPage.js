document.getElementById("sendFriendRequest").addEventListener("click", (e) => {
    fetch(`/Friend/${document.getElementById("Id").innerText}`, {
        method: "POST"
    }).then(response => {
        e.target.innerText = "success"
    }).catch(response => {
        e.target.innerText = "fail"
    });
});