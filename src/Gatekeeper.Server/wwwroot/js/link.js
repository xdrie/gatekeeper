function redirectPost(url, data) {
    var form = document.createElement('form');
    form.method = 'POST';
    form.action = url;
    // add fields
    for (var name in data) {
        var input = document.createElement('input');
        input.type = 'hidden';
        input.name = name;
        input.value = data[name];
        form.appendChild(input);
    }
    console.log('posting to', url, form);
    document.body.appendChild(form);
    form.submit();
    document.body.removeChild(form);
};

function show_link_error(err) {
    console.error(err);
    let msg = 'auth error (unspecified)';
    let errCodes = {
        404: 'app not found',
        403: 'you do not have permission to link this app'
    };
    let sc = err.response.status;
    if (sc in errCodes) {
        msg = errCodes[sc];
    }
    toast_error(msg);
}

async function linkApplication(app, cbUri) {
    // request an app token
    try {
        const client = get_client();
        const resp = await client.get(`/app/login/${app}`);
        console.log(resp);
        // we successfully obtained our app token.
        let appUser = resp.data.user;
        let appToken = resp.data.token;
        console.log('authenticated as', appUser.username, 'app token', appToken);
        // now, we need to post to the callback uri
        redirectPost(cbUri, appToken);
    } catch (err) {
        show_link_error(err);
    }
}

function wireLinking(user) {
    // populate ui
    let args = parseQuery(window.location.search);
    $("#app-name").innerText = args.app;
    $("#user").innerText = user.username;
    let callbackUri = args.cb;

    if (!args.app || !args.cb) {
        // show error
        toast_error("app or callback missing");
        $("#accept").disabled = true;
    }

    $("#cancel").on("click", e => {
        if (window.opener && window.opener !== window) {
            window.close();
        } else {
            // default cancel, return to home
            window.location.href = '/';
        }
    });

    $("#link").on("submit", e => {
        // do the linking
        console.log('link requested');
        linkApplication(args.app, args.cb);
        e.preventDefault();
    });
}

$load(() => {
    authGuard(wireLinking);
});
