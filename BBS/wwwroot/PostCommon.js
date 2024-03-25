


document.getElementById("LikeBtn").addEventListener("click", () => {
    fetch("/Like/Post", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            PostId: document.getElementById("PostId").value
        })
    }).then(response => {
        console.log(response);
    });