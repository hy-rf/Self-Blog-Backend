
var PostForm = document.createElement("form");
PostForm.setAttribute("method", "POST");
PostForm.setAttribute("action", "/Post/CreatePost");
PostForm.innerHTML = `<input name="Title"><input name="Content"><input name="Tags"><button>Post</button>`;

document.getElementById("Post").addEventListener("click", () => {
    document.querySelector("main").appendChild(PostForm);
});