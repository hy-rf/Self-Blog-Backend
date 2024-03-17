var prepos = window.scrollY;
window.onscroll = () => {
    var curpos = window.scrollY;
    if (prepos >= curpos) {
        document.querySelector("header").classList.remove("header_hide");
    }
    else {
        document.querySelector("header").classList.add("header_hide");
    }
    prepos = curpos;
}

readJson = (object) => {
    return fetch("/test", {
        method: "POST",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        body: JSON.stringify(object),
    }).then((response) => {
        return response.json();
    }).catch((err) => {
        console.log(err);
    });
}