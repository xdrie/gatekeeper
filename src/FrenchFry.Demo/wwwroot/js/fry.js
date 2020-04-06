
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
            $("#monthly").innerText = `${monthly.used} of ${monthly.quota} monthly fry orders used`;
            if (monthly.used >= monthly.quota) {
                $('#order').setAttribute("disabled", "disabled");
                // bottom text
            }
        })
}

window.addEventListener("load", e => {
    // fetch user data
    fetch('/a/me', {
        ...getHttpConfig()
    })
        .then(response => response.json())
        .then(auth => {
            $('#status').innerHTML = `
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
    $('#order').addEventListener("click", e => {
        // order a fry
        fetch('/a/fries/order', {
            method: 'POST',
            ...getHttpConfig()
        })
            .then(response => response.json())
            .then(fry => {
                console.log('received fry', fry);
                $("#status").innerText = `ordered fry! the fry: ${fry.mysterious}`;
                updateStatus();
            })
            .catch(err => {
                console.error(err);
                $("#status").innerText = 'failed to order fry.'
            });
    });
});

