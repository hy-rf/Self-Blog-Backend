

document.querySelector("#friendRequestsReceived").addEventListener("click", (e) => {
    if (e.target.tagName == "BUTTON") {
        fetch("/Friend", {
            method: "POST",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                Id: parseInt(e.target.parentNode.firstElementChild.children[1].innerText)
            })
        }).then(response => {
            e.target.innerText = "success"
        }).catch(error => {
            e.target.innerText = "fail"
        });
    }
});