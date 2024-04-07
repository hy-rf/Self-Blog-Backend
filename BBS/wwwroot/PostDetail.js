
var PostContent;
const quill = new Quill('#PostContent', {
    readOnly: true,
});


const quillEdit = new Quill('#EditPostForm', {
    modules: {
        toolbar: ['bold', 'italic', 'underline', 'strike', 'link', 'image', 'code-block', 'video', 'blockquote', 'clean']
    },
    placeholder: 'Compose an epic...',
    theme: 'snow'
});

document.getElementById("toggleEditPostBtn").addEventListener("click", () => {
    document.getElementById("PostFormWrapper").classList.toggle("hideEditPost");
});

// Get Post Content then set it to quill editors
fetch(`/api/Post/${document.location.href.split("/")[5]}`).then(res => {
    return res.json();
}).then(ret => {
    if (ret.success) {
        PostContent = JSON.parse(ret.payload)
        quill.setContents(PostContent);
        quillEdit.setContents(PostContent);
    }
});



// Set Tag and Title to the input fields in the edit form
document.getElementById("Tag").value = (document.getElementById("Tags") != null) ? document.getElementById("Tags").innerText.split(" ").join("") : "";
document.getElementById("Title").value = document.getElementsByClassName("Title")[0].innerText;

document.getElementById("submitEditPost").addEventListener("click", (e) => {
    fetch(`/Post/EditPost`, {
        method: "POST",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            PostId: parseInt(parseInt(document.location.href.split("/")[5])),
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