
document.getElementById("addUsertoChatRoom").addEventListener("click", (e) => {
    if (e.target.tagName == "BUTTON") {
        fetch("/Chat/AddChatRoomMember", {
            method: "POST",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                UserId: document.getElementById("UserId").value,
                ChatRoomId: window.location.href.split("/")[4]
            }),
        }).then(response => {
            e.target.innerText = "success"
        });
    }
})