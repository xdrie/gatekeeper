// app-wide script
const $ = document.querySelector.bind(document)

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

window.addEventListener("load", function () {
    // bind defaults
    let logoutBtn = $('#logout');
    if (logoutBtn) {
        logoutBtn.addEventListener("click", e => auth_logout());
    }
});
