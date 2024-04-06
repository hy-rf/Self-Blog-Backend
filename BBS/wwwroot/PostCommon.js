


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
            if (res.message == "Post liked") {
                e.target.src = "/img/heartfill.png";
            }
            else {
                e.target.src = "/img/heartvac.png";
            }
        }
        else {
            e.target.innerText = res.message;
        }
    }
});

document.getElementById("Tag").addEventListener("keydown", (e) => {
    if (e.key === " " || e.key === "Enter") {
        var tagArray = e.target.value.split(" ").filter((item) => item != "");
        e.target.value = tagArray.map(tag => tag.startsWith("#") ? tag : "#" + tag).join(" ");
    }
    else if (e.key === "#") {
        e.target.value = e.target.value.slice(0, -1);
    }
})