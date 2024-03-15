PostContentOnEditor = async (PostId, html) => {
    var PostEditorConfig = {};
    PostEditorConfig.toolbar = "basic";
    var PostEditor = await new RichTextEditor("#EditPostForm", PostEditorConfig);
    PostEditor.setHTMLCode(html);
    document.getElementById("submitEditPost").addEventListener("click", (e) => {
        fetch(`Post/EditPost`, {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                PostId: parseInt(parseInt(PostId)),
                Title: document.getElementById("Title").value,
                Content: PostEditor.getHTMLCode()
            }),
        }).then(response => {
            location.reload();
            return;
        });
    });
}

