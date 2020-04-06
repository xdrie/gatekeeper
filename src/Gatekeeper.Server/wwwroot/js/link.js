
function addFormField(form, key, value) {
    var input = document.createElement("input");
    input.type = "text";
    input.name = key;
    input.value = value;
    form.appendChild(input);
}

function sendCallback(url, token) {
    console.log('posting callback to', uri);
    var form = document.createElement('form');
    form.method = 'post';
    form.action = url;
    // add fields
    addFormField(form, 'content', token.content);
    addFormField(form, 'expires', token.expires);
    addFormField(form, 'scope', token.scope);
    form.submit();
};

async function linkApplication(app, cbUri) {
    // request an app token
    try {
        const client = get_client();
        const resp = await client.get(`/app/token/${app}`);
        console.log(resp);
        // we successfully obtained our app token.
        let appToken = resp.data;
        // now, we need to post to the callback uri
        sendCallback(cbUri, appToken);
    } catch (err) {
        console.error(err);
        toast_error(`failed to link (${err.response.status})`);
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
        window.history.back();
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
