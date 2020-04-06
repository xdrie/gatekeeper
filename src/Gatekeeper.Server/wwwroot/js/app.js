// app-wide script
const $ = document.querySelector.bind(document)
const $load = function (cb) {
    window.addEventListener("load", cb);
}

Element.prototype.on = function (ev, cb) {
    this.addEventListener(ev, e => cb(e));
}

Element.prototype.show = function () {
    this.style.display = '';
}

Element.prototype.hide = function () {
    this.style.display = 'none';
}

Element.prototype.toggle = function () {
    if (this.style.display == 'none') {
        this.show();
    } else {
        this.hide();
    }
}

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

function parseQuery(queryString) {
    var query = {};
    var pairs = (queryString[0] === '?' ? queryString.substr(1) : queryString).split('&');
    for (var i = 0; i < pairs.length; i++) {
        var pair = pairs[i].split('=');
        query[decodeURIComponent(pair[0])] = decodeURIComponent(pair[1] || '');
    }
    return query;
}

function storeToken(token) {
    window.localStorage.setItem('tok', token);
    console.log('stored token:', token);
}

function loadToken() {
    let v = window.localStorage.getItem('tok');
    console.log('loaded token:', v);
    return v;
}


// save authorization
function storeAuthorization(data) {
    let user = data.user;
    console.log('authorized as', user);
    let token = data.token;
    storeToken(token.content);
}

function get_client() {
    let cfg = {
        baseURL: '/a',
        // timeout: 1000,
        headers: {}
    };
    // check if token is saved
    let token = loadToken();
    if (token) {
        cfg.headers['Authorization'] = `Bearer ${token}`;
    }
    let client = axios.create(cfg);
    client.authed = token != null;
    return client;
}

function auth_logout() {
    storeToken(null); // clear token
    window.location.href = "/"; // return to home
}

function parse_pronouns(id) {
    switch (id) {
        case 0:
            return 'they/them';
        case 1:
            return 'he/him';
        case 2:
            return 'she/her';
    }
}

// guard to ensure we are authorized
async function authGuard(cb) {
    try {
        const client = get_client();
        if (!client.authed) {
            throw "client not authorized";
        }
        // try fetching me page
        const resp = await client.get('/u/me');
        let me = resp.data;
        console.log('me', me);
        cb(me);
    } catch (err) {
        console.error(err);
        // send back to login
        window.location.href = "/login";
    }
}

$load(() => {
    // bind defaults
    let logoutBtn = $('#logout');
    if (logoutBtn) {
        logoutBtn.addEventListener("click", e => auth_logout());
    }
});
