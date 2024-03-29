

//document.querySelector("main").addEventListener("change", async (e) => {
//    console.log(e.target.id);
//    if (e.target.id == "chatroomList") {
//        var res = await fetch("/api/GetJoinedChatRoom", {
//            method: "POST",
//            headers: {
//                "Accept": "application/json",
//                "Content-Type": "application/json"
//            },
//        }).then(response => {
//            return response.json();
//        });
//        if (res.success) {
//            res.payload.forEach((element) => {
//                var li = document.createElement("li");
//                li.innerText = element.Id + element.Name;
//                document.querySelector("#chatroomList ul").appendChild(li);
//            });
//        }
//        else {
//            document.querySelector("#chatroomList ul").innerText = "No Chat Room";
//        }
//        e.stopPropagation();
//    }
//});