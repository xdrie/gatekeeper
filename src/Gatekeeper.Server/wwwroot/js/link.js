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

async function linkApplication(config) {
    // request an app token
    try {
        const client = get_client();
        const resp = await client.get(`/app/login/${config.app}`);
        console.log(resp);
        // we successfully obtained our app token.
        let auth = resp.data;
        console.log('authenticated as', auth.user.username, 'app token', auth.token);
        // return data to callback
        if ('p' in config) {
            // post message to parent window
            console.log('posting auth to parent');
            window.opener.postMessage(auth, config.cb);
            window.close();
        } else {
            redirectPost(config.cb, auth.token);
        }
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

    const config = args;

    $("#link").on("submit", e => {
        // do the linking
        console.log('link requested');
        linkApplication(config);
        e.preventDefault();
    });
}

$load(() => {
    authGuard(wireLinking);
});
