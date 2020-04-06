
// fry access

function getHttpConfig() {
    let key = localStorage.getItem("key");
    return {
        headers: {
            Authorization: `Bearer ${key}`
        }
    }
}

function updateStatus() {
    fetch('/a/fries', { ...getHttpConfig() })
        .then(response => response.json())
        .then(monthly => {
            $("#status").innerText = `${monthly.used} of ${monthly.quota} monthly fry orders used`
        })
}

window.addEventListener("load", e => {
    // fetch user data
    fetch('/a/me', {
        ...getHttpConfig()
    })
        .then(response => response.json())
        .then(auth => {
            $('#user').innerHTML = `
                <p>
                welcome, ${auth.user.name}, also known as <i>${auth.user.username}</i>!
                </p>
            `;
            updateStatus();
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

