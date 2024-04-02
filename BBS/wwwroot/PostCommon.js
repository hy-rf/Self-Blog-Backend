


document.getElementById("PostList").addEventListener("click", async (e) => {
    if (e.target.classList.contains("LikeBtn")) {
        var res = await fetch("/Like/Post", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                PostId: parseInt(e.target.parentElement.firstElementChild.innerText)
            })
        }).then(response => {
            return response.json();
        });
        if (res.success) {
            if(res.message == "Post liked"){
                e.target.src = "/img/heartfill.png";
            }
            else{
                e.target.src = "/img/heartvac.png";
            }
        }
        else {
            e.target.innerText = res.message;
        }
    }
});