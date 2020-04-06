function toast_error(msg) {
    $('#error').innerText = msg;
    $('#error').show();
}

function show_auth_error(err) {
    console.error(err);
    let msg = 'auth error (unspecified)';
    let errCodes = {
        409: 'conflicting username/email',
        422: 'invalid fields',
        401: 'invalid credentials'
    };
    let sc = err.response.status;
    if (sc in errCodes) {
        msg = errCodes[sc];
    }
    toast_error(msg);

    // re-show forms
    $('#auth-container').show();
    $('#load').hide();
}

async function auth_create(data) {
    try {
        const client = get_client();
        const resp = await client.post('/auth/create', data);
        console.log(resp);
        storeAuthorization(resp.data);
        window.location.href = "/verify"; // successfully created account, please verify
    } catch (err) {
        show_auth_error(err);
    }
}

async function auth_login(data) {
    try {
        const client = get_client();
        const resp = await client.post('/auth/login', data);
        console.log(resp);
        storeAuthorization(resp.data);
        window.location.href = "/user/dash"; // successfully logged in
    } catch (err) {
        show_auth_error(err);
    }
}

async function auth_verify(data) {
    try {
        let code = data.code;
        const client = get_client();
        if (!client.authed) {
            toast_error('no token stored');
            throw "client not authorized";
        }
        const resp = await client.post(`/auth/verify/${code}`);
        console.log(resp);
        window.location.href = "/user/dash"; // successful verify
    } catch (err) {
        show_auth_error(err);
    }
}

let authForm = $("#auth");
if (authForm) {
    authForm.addEventListener("submit", ev => {
        let formData = new FormData(ev.target);
        let authData = parseFormData(formData);

        // figure out auth mode
        let mode = null;
        if (authData.hasOwnProperty('email')) {
            mode = 'create';
        } else if (authData.hasOwnProperty('code')) {
            mode = 'verify';
        } else {
            mode = 'login';
        }
        console.log(`submitting auth (${mode})`, authData);

        $('#auth-container').hide();
        $('#load').show();
        $('#error').hide();

        let task = null;
        switch (mode) {
            case 'create':
                task = auth_create(authData);
                break;
            case 'login':
                task = auth_login(authData);
                break;
            case 'verify':
                task = auth_verify(authData);
                break;
        }

        ev.preventDefault();
    });
}
