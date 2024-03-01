
var PostForm = document.createElement("form");
PostForm.setAttribute("method", "POST");
PostForm.setAttribute("action", "/Post/CreatePost");
PostForm.innerHTML = `<input name="Title"><input name="Content"><input name="Tags"><button>Post</button>`;

document.getElementById("Post").addEventListener("click", () => {
    document.querySelector("main").appendChild(PostForm);
});

function showEditPostPanel(Id) {
    var EditPostForm = document.createElement("form");
    EditPostForm.setAttribute("method", "POST");
    EditPostForm.setAttribute("action", `/Post/EditPost/${Id}`);
    EditPostForm.innerHTML = `<input name="Title"><input name="Content"><input name="Tags"><button>EditPost</button>`;
    document.getElementById(`PostUnit${Id}`).appendChild(EditPostForm);
}