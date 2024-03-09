


//function startDrag(e) {
//    var e = window.Event;
//    var targ = e.target;

//    if (targ.className != 'dragme') {
//        return false;
//    };
//    // calculate event X, Y coordinates
//    offsetX = e.clientX;
//    offsetY = e.clientY;

//     //assign default values for top and left properties
//    if (!targ.style.left) {
//        targ.style.left = '25%'
//    };
//    if (!targ.style.top) {
//        targ.style.top = '10%'
//    };

//    // calculate integer values for top and left 
//    // properties
//    coordX = parseInt(targ.style.left);
//    coordY = parseInt(targ.style.top);
//    drag = true;

//    // move div element
//    document.onmousemove = dragDiv;
//    return false;
//}

//function dragDiv(e) {
//    if (!drag) {
//        return
//    };
//    if (!e) {
//        var e = window.Event
//    };
//    var targ = e.target;
//    targ.style.left = coordX + e.clientX - offsetX + 'px';
//    targ.style.top = coordY + e.clientY - offsetY + 'px';
//    return false;
//}

//function stopDrag() {
//    drag = false;
//}
//window.onload = function () {
//    document.querySelector("#avatar").addEventListener("change", () => {
//        const [file] = document.querySelector("#avatar").files;
//        if (file) {
//            document.querySelector('#Preview').src = URL.createObjectURL(file);
//        }
//    });
//    document.onmousedown = startDrag();
//    document.onmouseup = stopDrag;
//}