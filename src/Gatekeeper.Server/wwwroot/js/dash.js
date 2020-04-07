
function populateDash(user) {
    $('#load').hide();
    $('#dash').show();
    // fill the dash info
    // $('#profile-name').innerText = `${user.name} (${parse_pronouns(user.pronouns)})`;
    $('#profile-name').innerText = user.name;
    $('#profile-pronouns').innerText = parse_pronouns(user.pronouns);
    $('#profile-username').innerText = user.username;
    $('#profile-email').innerText = user.email;
    $('#profile-email').href = `mailto:${user.email}`;
    $('#profile-registered').innerText = new Date(user.registered).toLocaleDateString();
    $('#profile-uuid').innerText = user.uuid;
}

$load(() => {
    authGuard(populateDash);
});
