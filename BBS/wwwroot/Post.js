
var CreatePostForm = document.createElement("form");
CreatePostForm.setAttribute("method", "POST");
CreatePostForm.setAttribute("action", "/Post/CreatePost");
CreatePostForm.innerHTML = `<input name="Title"><input name="Content"><button>Post</button>`;

EditPostForm = (Id) => {
    var EditPostForm = document.createElement("form");
    EditPostForm.setAttribute("method", "POST");
    EditPostForm.setAttribute("action", `/Post/EditPost/${Id}`);
    EditPostForm.innerHTML = `<input name="Title" placeholder="Title"><input name="Content" placeholder="Content"><button>Post</button>`;
    return EditPostForm;
}

ReplyForm = (Id) => {
    var ReplyForm = document.createElement("form");
    ReplyForm.setAttribute("method", "POST");
    ReplyForm.setAttribute("action", `/Reply/Reply/${Id}`);
    ReplyForm.innerHTML = `<input name="Content" placeholder="Content"><button>Reply</button>`;
    return ReplyForm;
}


document.getElementById("Post").addEventListener("click", () => {
    if (document.getElementById("Post").nextElementSibling.tagName == "FORM") {
        document.getElementById("Post").nextElementSibling.remove();
        return;
    }
    document.querySelector("main").insertBefore(CreatePostForm, document.getElementById("Post").nextElementSibling);
    return;
});

function showEditPostPanel(Id) {
    console.log(this)
    if (document.getElementById(`PostUnit${Id}`).nextElementSibling.tagName == "FORM") {
        document.getElementById(`PostUnit${Id}`).nextElementSibling.remove();
        return;
    }
    document.querySelector("main").insertBefore(EditPostForm(Id), document.getElementById(`PostUnit${Id}`).nextElementSibling);
    return;
}

// document.querySelector("footer").style.bottom = "calc(var(--footer-height)*-1)";


function showReplyPanel(Id) {
    if (document.getElementById(`PostUnit${Id}`).nextElementSibling.tagName == "FORM") {
        document.getElementById(`PostUnit${Id}`).nextElementSibling.remove();
        return;
    }
    document.querySelector("main").insertBefore(ReplyForm(Id), document.getElementById(`PostUnit${Id}`).nextElementSibling);
    return;
}