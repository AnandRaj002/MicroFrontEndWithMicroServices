document.getElementById("authRegister").addEventListener('submit', register);

function register(e) {
    e.preventDefault();

    const userName = document.getElementById("UserName");
    const fisrtName = document.getElementById("FirstName");
    const lastName = document.getElementById("LastName");
    const email = document.getElementById("Email");
    const password = document.getElementById("Password");

    const item = {
        UserName: userName.value.trim(),
        FirstName: fisrtName.value.trim(),
        LastName: lastName.value.trim(),
        Email: email.value.trim(),
        Password: password.value.trim()
    };

    fetch (
        "https://localhost:44302/api/auth/register", {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(item)
        })
        .then(response => response.text())
        .then(data => getRegisterData(data))
        .catch(error => getErrorData(error));
};

function getErrorData(error){
    if(error != undefined && error != null){
        document.getElementById('lValue').innerHTML = error;
    }
};

function getRegisterData(data) {
    document.getElementById('lValue').innerHTML = data;
};