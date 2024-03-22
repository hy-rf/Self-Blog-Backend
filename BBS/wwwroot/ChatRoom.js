
document.getElementById("addUsertoChatRoom").addEventListener("click", (e) => {
    if (e.target.innerText == "kick") {
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
            window.location.reload();
        });
    }
})
document.getElementById("chatroomMemberList").addEventListener("click", (e) => {
    if (e.target.tagName == "BUTTON") {
        var userid = e.target.parentNode.firstElementChild.innerText;
        fetch("/Chat/KickChatRoomMember", {
            method: "DELETE",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                UserId: userid,
                ChatRoomId: window.location.href.split("/")[4]
            }),
        }).then(response => {
            e.target.innerText = "success"
            //window.location.reload();
        });
    }
    e.stopPropagation();
})