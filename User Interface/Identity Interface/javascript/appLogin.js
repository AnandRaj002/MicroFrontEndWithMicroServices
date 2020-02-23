document.getElementById("authLogin").addEventListener('submit', login);

function login(e) {
    e.preventDefault()
    const userInput = document.getElementById("uName");
    const passInput = document.getElementById("pWd");

    const item = {
        UserName: userInput.value.trim(),
        Password: passInput.value.trim()
    };

    // fetch(
    //         "https://localhost:44302/api/auth/login", {
    //             method: "POST",
    //             headers: {
    //                 'Content-Type': 'application/json'
    //             },
    //             body: JSON.stringify(item),                
    //         })
    //     .then(response => response.text())
    //     .then(data => displaydata(data))
    //     .catch(error => document.getElementById("lValue").innerHTML = error);
    //.catch(error => document.getElementById("lValue").innerHTML = error);    

    // Create Request
    var xmlhttp = new XMLHttpRequest();
    
    // Open Request - type, url, async
    xmlhttp.open("POST", "https://localhost:44302/api/auth/login", true);
    
    // Set Header for request optional
    xmlhttp.setRequestHeader("Content-Type", "application/json");
    
    // Send body of request for POSU
    xmlhttp.send(JSON.stringify(item));
    
    console.log('Ready State: ', xmlhttp.readyState);

    // On Progress - used for progress check - optional
    xmlhttp.onprogress = function() {
        console.log('Ready State: ', xmlhttp.readyState);
    }

    // One Ready State - Older way
    // xmlhttp.onreadystatechange = function() {
    //     console.log('Ready State: ', xmlhttp.readyState);
    //     if(this.readyState == 4 && this.status == 200) {
    //         document.getElementById("lValue").innerHTML = this.responseText;
    //     }        
    // }

    // Ready State Value
    // 0: Request not intialize
    // 1: Server connection established
    // 2: Request received
    // 3: Processinf request
    // 4: Request finished and response is ready

    // Onload Function - newer way
    xmlhttp.onload = function() {
        console.log('Ready State: ', xmlhttp.readyState);
        if(this.status == 200) {
            console.log(this.responseText);
            document.getElementById("lValue").innerHTML = this.responseText;
        } else if(this.status == 404) {
            console.log(this.responseText);
            document.getElementById("lValue").innerHTML = this.responseText;
        }
    }

    // On Error
    xmlhttp.onerror = function() {
        console.log(this.onerror);
    }

    // HTTP Status
    // 200: Ok
    // 400: Bad Request
    // 403: Forbidden
    // 404: Not Found
    // 500: Internal server error
    // Informational responses (100–199),
    // Successful responses (200–299),
    // Redirects (300–399),
    // Client errors (400–499),
    // and Server errors (500–599).

};

function displaydata(data) {
    document.getElementById("lValue").innerHTML = data;
};

