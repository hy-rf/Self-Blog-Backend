


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
            console.log(e.target.getAttribute("fill"))
            if (e.target.getAttribute("fill") == "#000000") {
                e.target.setAttribute("fill", "#ffffff");
                e.target.children[2].children[0].firstElementChild.setAttribute("fill", "#ff0000");
            }
            else {
                e.target.setAttribute("fill", "#000000");
                e.target.children[2].children[0].firstElementChild.setAttribute("fill", "#808184");
            }
        }
        else {
            e.target.innerText = res.message;
        }
    }
    e.stopPropagation();
}, true);

document.getElementById("Tag").addEventListener("keydown", (e) => {
    if (e.key === " " || e.key === "Enter") {
        var tagArray = e.target.value.split(" ").filter((item) => item != "");
        e.target.value = tagArray.map(tag => tag.startsWith("#") ? tag : "#" + tag).join(" ");
    }
    else if (e.key === "#") {
        e.target.value = e.target.value.slice(0, -1);
    }
})