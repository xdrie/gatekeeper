
async function fetchGroups() {
    const client = get_client();
    const resp = await client.get('/u/groups');
    let groups = resp.data;
    if (groups.length > 0) {
        // populate groups list
        $('#profile-groups').innerText = groups.join(', ');
    }
}

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

    // fetch groups
    fetchGroups();
}

$load(() => {
    authGuard(populateDash);
});
