
var PostForm = document.createElement("form");
PostForm.setAttribute("method", "POST");
PostForm.setAttribute("action", "/Post/CreatePost");
PostForm.innerHTML = `<input name="Title"><input name="Content"><input name="Tags"><button>Post</button>`;

document.getElementById("Post").addEventListener("click", () => {
    if (document.getElementById("Post").nextElementSibling.tagName == "FORM") {
        document.getElementById("Post").nextElementSibling.remove();
        return;
    }
    document.querySelector("main").insertBefore(PostForm, document.getElementById("Post").nextElementSibling);
    return;
});

function showEditPostPanel(Id) {
    var EditPostForm = document.createElement("form");
    EditPostForm.setAttribute("method", "POST");
    EditPostForm.setAttribute("action", `/Post/EditPost/${Id}`);
    EditPostForm.innerHTML = `<input name="Title"><input name="Content"><input name="Tags"><button>EditPost</button>`;
    document.getElementById(`PostUnit${Id}`).appendChild(EditPostForm);
    return;
}

// document.querySelector("footer").style.bottom = "calc(var(--footer-height)*-1)";


function showReplyPanel(Id) {
    var ReplyPostForm = document.createElement("form");
    ReplyPostForm.setAttribute("method", "POST");
    ReplyPostForm.setAttribute("action", `/Reply/Reply/${Id}`);
    ReplyPostForm.innerHTML = `<input name="Content"><button>Reply</button>`;
    document.getElementById(`PostUnit${Id}`).appendChild(ReplyPostForm);
    return;
}