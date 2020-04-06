// https://stackoverflow.com/questions/41431322/how-to-convert-formdatahtml5-object-to-json
function parseFormData(formData) {
    var object = {};
    formData.forEach((value, key) => {
        // Reflect.has in favor of: object.hasOwnProperty(key)
        if (!Reflect.has(object, key)) {
            object[key] = value;
            return;
        }
        if (!Array.isArray(object[key])) {
            object[key] = [object[key]];
        }
        object[key].push(value);
    });
    return object;
}

function show_auth_error(err) {
    console.error(err);
    let msg = '';
    switch (err.response.status) {
        case 409: // conflict
            msg = 'conflicting username/email';
            break;
        case 422: // unproc entity
        msg = 'invalid fields'
            break;
        case 401: // unauthorized
            msg = 'invalid credentials'
            break;
    }
    $('#error').innerText = msg;
    $('#error').show()
}

async function auth_create(data) {
    try {
        const resp = await axios.post('/a/auth/create', data);
        console.log(resp);
        window.location.href = "/"; // successfully created account
    } catch (err) {
        show_auth_error(err);
    }
}

async function auth_login(data) {
    try {
        const resp = await axios.post('/a/auth/login', data);
        console.log(resp);
        window.location.href = "/"; // successfully logged in
    } catch (err) {
        show_auth_error(err);
    }
}

$("#auth").addEventListener("submit", ev => {
    let formData = new FormData(ev.target);
    let authData = parseFormData(formData);

    // figure out if login or create
    let mode = authData.hasOwnProperty('email') ? 'create' : 'login';
    console.log(`submitting auth (${mode})`, authData);

    switch (mode) {
        case 'create':
            auth_create(authData);
            break;
        case 'login':
            auth_login(authData);
            break;
    }

    ev.preventDefault();
});
