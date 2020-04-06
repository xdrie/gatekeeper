
// fry access

window.addEventListener("load", e => {
    let key = localStorage.getItem("key");
    console.log('session key', key);
    // fetch user data
    fetch('/a/me', {
        headers: {
            Authorization: `Bearer ${key}`
        }
    })
        .then(response => response.json())
        .then(auth => {
            let fryQuota = auth.rules.find(x => x.key == 'quota').value;
            $('#user').innerHTML = `
                <p>
                welcome, ${auth.user.name}, also known as <i>${auth.user.username}</i>!
                you can take up to <b>${fryQuota}</b> fries.
                </p>
            `;
        })
        .catch(err => {
            console.error('failed to get user info');
            console.error(err);
            $("#status").innerText = 'you could not be logged in.'
        });
    $('#logout').addEventListener("click", e => {
        // clear key
        localStorage.setItem("key", null);
        window.location.href = '/';
    });
});

