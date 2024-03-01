var prepos = window.scrollY;
window.onscroll = () => {
    var curpos = window.scrollY;
    if (prepos >= curpos) {
        document.querySelector("header").classList.add("header_show");
        document.querySelector("header").classList.remove("header_hide");
    }
    else {
        document.querySelector("header").classList.remove("header_show");
        document.querySelector("header").classList.add("header_hide");
    }
    console.log(curpos);
    prepos = curpos;
    if (curpos==0){
        document.querySelector("header").classList.remove("header_show");
    }
}