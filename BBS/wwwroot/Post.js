
const quillEdit = new Quill('#CreatePostForm', {
    modules: {
        toolbar: ['bold', 'italic', 'underline', 'strike', 'link', 'image', 'code-block', 'video', 'blockquote', 'clean']
    },
    placeholder: 'Compose an epic...',
    theme: 'snow'
});
document.getElementById("submitPost").addEventListener("click", (e) => {
    fetch(`/Post/CreatePost`, {
        method: "POST",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            Title: document.getElementById("Title").value,
            Tag: document.getElementById("Tag").value,
            Content: quillEdit.getContents()
        }),
    }).then(response => {
        return response.json();
    }).then(ret => {
        if (ret.success) {
            location.reload();
        }
        else {
            alert(ret.message);
        }
    });
});
document.getElementById("toggleCreatePostBtn").addEventListener("click", () => {
    document.getElementById("PostFormWrapper").classList.toggle("showCreatePost");
});

