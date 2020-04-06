
// frenchfry auth

function onsignin(gate) {
    console.log(`welcome, ${gate.user.name}!`, gate.token);
    // post the token to our server and then save the session key
    fetch('/a/bridge', {
        method: "POST",
        body: JSON.stringify(gate.token)
    })
        .then(response => response.json())
        .then(data => {
            let session = data.session;
            localStorage.setItem("key", session);

            // redirect to fry
            window.location.href = '/fry';
        })
        .catch(err => {
            console.error('failed to send data to bridge');
            console.error(err);
        });
}
