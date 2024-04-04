
var PostContent;
const quill = new Quill('#PostContent', {
    readOnly: true,
});
const quillEdit = new Quill('#EditPostForm', {
    modules: {
        toolbar: ['bold', 'italic', 'underline', 'strike']
    },
    theme: 'snow'
});
window.onload = () => {
    fetch(`/api/Post/${document.location.href.split("/")[5]}`).then(res => {
        return res.json();
    }).then(ret => {
        console.log(ret);
        if (ret.success) {
            PostContent = JSON.parse(ret.payload)
            
            quill.setContents(PostContent);

            
            quillEdit.setContents(PostContent);
        }
    })


    document.getElementById("toggleEditPostBtn").addEventListener("click", () => {
        document.getElementById("PostFormWrapper").classList.toggle("hideEditPost");
        
    });

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
            location.reload();
            return;
        });
    });

}