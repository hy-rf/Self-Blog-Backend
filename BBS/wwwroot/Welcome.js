
document.getElementById("loginBtn").addEventListener("click", () => {
    document.getElementById("togglePanel").innerHTML = `<form method="POST" action="/User/Login"><input name="Name" /><input name="Pwd" type="password" /><button type="submit">login</button></form>`;
});

document.getElementById("signupBtn").addEventListener("click", () => {
    document.getElementById("togglePanel").innerHTML = `<form method="POST" action="/User/Signup"><input name="Name" /><input name="Pwd" type="password" /><button type="submit">sign up</button></form>`;
});