window.onload = () => {
    var PostEditorConfig = {};
    PostEditorConfig.toolbar = "basic";
    var PostEditor = new RichTextEditor("#EditPostForm", PostEditorConfig);
    //PostEditor.setHTMLCode(ViewBag.Post.Content);
    document.getElementById("submitEditPost").addEventListener("click", (e) => {
        fetch(`Post/EditPost`, {
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
function test(html) {
    PostEditor.setHTMLCode(html);
}
