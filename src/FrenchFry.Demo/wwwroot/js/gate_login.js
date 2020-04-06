
function _gklogin_recieve(config, event) {
    // ensure event is from auth server
    if (event.origin !== config.server) return;
    let auth = event.data;
    console.log('received login', auth);
    window[config.cb](auth);
}

(async () => {
    // BEGIN FILE SCOPE

    // - constants
    let gate_login_button = `<a class="btn btn-primary gate-signin-button">\n<img src="$SERVER/img/glyph.png"/>\nlogin\n</a>`

    // - lib
    const $ = document.querySelector.bind(document)
    const $load = function (cb) {
        window.addEventListener("load", cb);
    }

    $load(e => {
        // update all signin sections
        let sign = $('.gate-signin');
        let config = {
            cb: sign.getAttribute('data-onsuccess'),
            server: sign.getAttribute('data-server'),
            app: sign.getAttribute('data-appid')
        };
        console.log('configurining gate signin', config);
        sign.innerHTML = get_login_button(config);

        // register click event on login button
        let login_btn = $('.gate-signin-button');
        login_btn.addEventListener("click", e => {
            console.log("requesting login", config);

            // popup gatekeeper link
            let origin = window.location.origin;
            let gk_window = window.open(`${config.server}/link?app=${config.app}&cb=${origin}&p=1`);
            
            // register callback
            window.addEventListener("message",
                (e) => _gklogin_recieve(config, e),
                false);
        });
    });

    function get_login_button(cfg) {
        return gate_login_button.replace("$SERVER", cfg.server);
    }

    // END FILE SCOPE
})();