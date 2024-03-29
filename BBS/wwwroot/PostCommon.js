


document.getElementById("PostList").addEventListener("click", async (e) => {
    if (e.target.classList.contains("LikeBtn")) {
        console.log(e.target.parentElement.firstChild)
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
            e.target.outerHTML = "<p>Liked</p>";
        }
        else {
            e.target.innerText = res.message;
        }
    }
});