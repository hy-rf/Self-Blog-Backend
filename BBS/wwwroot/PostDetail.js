
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
    document.getElementById("toggleEditPostBtn").addEventListener("click", () => {
        document.getElementById("PostFormWrapper").classList.toggle("hideEditPost");
    });

    document.getElementById("Tag").value = (document.getElementById("Tags") != null) ? document.getElementById("Tags").innerText.split(" ").join("") : "";
    document.getElementById("Title").value = document.getElementsByClassName("Title")[0].innerText;
    PostContentOnEditor(document.querySelector(".PostUnit p:nth-child(1)").innerText, document.getElementById("PostContent").innerHTML);
    
}