
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
            Tag: document.getElementById("Tag").value,
            Content: PostEditor.getHTMLCode()
        }),
    }).then(response => {
        location.reload();
        return;
    });
});
document.getElementById("toggleCreatePostBtn").addEventListener("click", () => {
    document.getElementById("PostFormWrapper").classList.toggle("showCreatePost");
});

