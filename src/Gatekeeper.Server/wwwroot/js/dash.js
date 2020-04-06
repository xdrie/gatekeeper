
function populateDash(user) {
    // fill the dash info
    // $('#profile-name').innerText = `${user.name} (${parse_pronouns(user.pronouns)})`;
    $('#profile-name').innerText = user.name;
    $('#profile-pronouns').innerText = parse_pronouns(user.pronouns);
    $('#profile-username').innerText = user.username;
    $('#profile-email').innerText = user.email;
    $('#profile-email').href = `mailto:${user.email}`;
    $('#profile-registered').innerText = new Date(user.registered).toLocaleDateString();
}

// guard to ensure we are authorized
async function authGuard() {
    try {
        const client = get_client();
        if (!client.authed) {
            throw "client not authorized";
        }
        // try fetching me page
        const resp = await client.get('/u/me');
        let me = resp.data;
        console.log('me', me);
        // success.
        $('#load').hide();
        $('#dash').show();
        populateDash(me);
    } catch (err) {
        console.error(err);
        // send back to login
        window.location.href = "/login";
    }
}

authGuard();
