
PostContentOnEditor = async (PostId, html) => {
    var PostEditorConfig = {};
    PostEditorConfig.toolbar = "basic";
    var PostEditor = await new RichTextEditor("#EditPostForm", PostEditorConfig);
    PostEditor.setHTMLCode(html);
    document.getElementById("submitEditPost").addEventListener("click", (e) => {
        fetch(`/Post/EditPost`, {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                PostId: parseInt(parseInt(PostId)),
                Title: document.getElementById("Title").value,
                Tag: document.getElementById("Tag").value,
                Content: PostEditor.getHTMLCode()
            }),
        }).then(response => {
            location.reload();
            return;
        });
    });
}

window.onload = () => {
    document.getElementById("Tag").value = document.getElementById("Tags").innerText.split(" ").join("");
}