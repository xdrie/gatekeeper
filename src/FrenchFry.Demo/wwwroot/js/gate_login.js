function _gklogin_recieve(data) {

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
            let gklink = window.open(`${config.server}/link?app=${config.app}`)

            window[config.cb](token);
        });
    });

    function get_login_button(cfg) {
        return gate_login_button.replace("$SERVER", cfg.server);
    }

    // END FILE SCOPE
})();