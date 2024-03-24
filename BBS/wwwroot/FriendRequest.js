

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