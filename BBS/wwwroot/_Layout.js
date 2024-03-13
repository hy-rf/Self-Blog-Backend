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

window.onload = () => {
    document.querySelector('#user').addEventListener('mouseenter', () => {
        document.querySelector('#UserMenu').style.display = "block";
    });
    document.querySelector('#user').addEventListener('mouseleave', () => {
        document.querySelector('#UserMenu').style.display = "none";
    });
}