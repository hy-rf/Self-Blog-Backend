
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

document.querySelector("#Avatar").addEventListener("change", () => {
    const [file] = document.querySelector("#Avatar").files;
    if (file) {
        document.querySelector('#Preview').src = URL.createObjectURL(file);
    }
    document.querySelector("form>button").removeAttribute("disabled");
});

BindEditNameEvent = () => {
    document.querySelector('#name').addEventListener('click', () => {
        var ele = document.querySelector('#name');
        ele.outerHTML = `<input id="inputName" value="${ele.innerHTML}"></input><button id="changeNameButton">Update Name</button>`;
        document.getElementById("changeNameButton").addEventListener("click", () => { EditName(document.querySelector('#Id').innerText.split(':')[1]) });
    });
}
BindEditNameEvent();



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
    }).then(response => {
        return;
    });
    var ele = document.querySelector('#inputName');
    ele.outerHTML = `<span id="name">${ele.value}</span>`;
    document.getElementById("changeNameButton").remove();
    BindEditNameEvent();
    document.getElementById('Title').innerText = `${ele.value}'s Info:`;
    document.getElementById('userlink').innerText = `Hello! ${ele.value}`;
}

