
async function UploadFile(FormEle) {
    var resultElement = FormEle.elements.namedItem("result");
    const formData = new FormData(FormEle);

    try {
        const response = await fetch(FormEle.action, {
            method: 'POST',
            body: formData
        });
        location.reload();
    } catch (error) {
        console.error('Error:', error);
    }
}

document.querySelector("#avatar").addEventListener("change", () => {
    const [file] = document.querySelector("#avatar").files;
    if (file) {
        document.querySelector('#Preview').src = URL.createObjectURL(file);
    }
    document.querySelector("form>button").removeAttribute("disabled");
});

document.querySelector('#name').addEventListener('click', () => {
    var ele = document.querySelector('#name');
    ele.outerHTML = `<input id="inputName" value="${ele.innerHTML}"></input><button id="changeNameButton">Update Name</button>`;
    document.getElementById("changeNameButton").addEventListener("click", () => { EditName(document.querySelector('#Id').innerText.split(':')[1]) });
});



EditName = (Id) => {
    var newName = document.getElementById('inputName').value;
    fetch(`/User/EditName/${Id}`, {
        method: "POST",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            Name: newName
        }),
    }).then();
    console.log(Id);
    }

