
// guard to ensure we are authorized
async function authGuard() {
    try {
        const client = get_client();
        if (!client.authed) {
            throw "client not authorized";
        }
        // try fetching me page
        const resp = await client.get('/u/me');
        console.log('me', resp.data);
        // success.
        $('#load').hide();
        $('#dash').show();
    } catch (err) {
        console.error(err);
        // send back to login
        window.location.href = "/login";
    }
}

authGuard();
