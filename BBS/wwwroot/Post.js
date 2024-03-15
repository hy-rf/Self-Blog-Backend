
//var CreatePostForm = document.createElement("form");
////CreatePostForm.setAttribute("method", "POST");
////CreatePostForm.setAttribute("action", "/Post/CreatePost");
//CreatePostForm.setAttribute("id", "CreatePostForm");
////CreatePostForm.innerHTML = `<input name="Title"><textarea name="Content" placeholder="Content"></textarea><button>Post</button>`;

//EditPostForm = (Id) => {
//    var EditPostForm = document.createElement("form");
//    EditPostForm.setAttribute("method", "POST");
//    EditPostForm.setAttribute("action", `/Post/EditPost/${Id}`);
//    EditPostForm.innerHTML = `<input name="Title" placeholder="Title"><textarea name="Content" placeholder="Content"></textarea><button>Post</button>`;
//    return EditPostForm;
//}

ReplyForm = (Id) => {
    var ReplyForm = document.createElement("form");
    ReplyForm.setAttribute("method", "POST");
    ReplyForm.setAttribute("action", `/Reply/Reply/${Id}`);
    ReplyForm.innerHTML = `<input name="Content" placeholder="Content"><button>Reply</button>`;
    return ReplyForm;
}


//document.getElementById("Post").addEventListener("click", () => {
//    if (document.getElementById("Post").nextElementSibling.tagName == "FORM") {
//        document.getElementById("Post").nextElementSibling.remove();
//        return;
//    }
//    document.querySelector("main").insertBefore(CreatePostForm, document.getElementById("Post").nextElementSibling);
//    return;
//});



window.onload = () => {
    var PostEditorConfig = {};
    PostEditorConfig.toolbar = "basic";
    var PostEditor = new RichTextEditor("#CreatePostForm", PostEditorConfig);
    document.getElementById("submitPost").addEventListener("click", (e) => {
        fetch(`Post/CreatePost`, {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                Title: document.getElementById("Title").value,
                Content: PostEditor.getHTMLCode()
            }),
        }).then(response => {
            location.reload();
            return;
        });
    });
}