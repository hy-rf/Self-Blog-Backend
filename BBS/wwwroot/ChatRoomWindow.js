

document.querySelector("main").addEventListener("change", (e) => {
    if (e.target.id == "chatroomList") {
        var res = fetch("/apt/GetJoinedChatRoom", {
            method: "POST",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
        }).then(response => {
            return response.json();
        }).then(ret => {
            return ret.payload;
        });
        if (res.success) {
            res.payload.forEach((element) => {
                var li = document.createElement("li");
                li.innerText = element.Id + element.Name;
                document.querySelector("#chatroomList ul").appendChild(li);
            });
        }
        else {
            document.querySelector("#chatroomList ul").innerText = "No Chat Room";
        }
        e.stopPropagation();
    }
})