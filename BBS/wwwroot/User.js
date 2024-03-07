
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
    document.querySelector('#name').tagName = "input";
});