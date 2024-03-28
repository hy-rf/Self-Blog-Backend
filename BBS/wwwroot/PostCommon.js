


document.querySelector("main").addEventListener("click", (e) => {
    if (e.target.classList.contains("LikeBtn")) {
        fetch("/Like/Post", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                PostId: e.target.parentElement.href.split("/")[4]
            })
        }).then(response => {
            console.log(response);
        });
    }
});