
# remote auth overview

a registered, activated user account is required for app authentication.
the app auth system allows gatekeeper to issue an auth token to another application.
this auth token is then used to retrieve scoped user information (based on their privacy preferences) and information about user roles and permissions.

in order to enable providing authorization for external applications, the administrator must first configure app definitions in the config file.

## app configuration

apps are configured in the `apps` node array.
as a demonstration, we will configure an app called `FrenchFry` that belongs to the layer `/Food`.

```toml
[[apps]]
name = "FrenchFry"
layers = [ "/Food" ]
secret = "yeet"
```

## admin group/permission management

membership to default groups can be configured in `users.default_groups`.
the group management api allows an administrator to add and remove users from additional custom groups.

an admin account is needed for using admin functions. follow the basic account creation steps again. at the end, this time, update the database to set the user's role to `Admin`.

to modify permissions, use `PATCH /a/groups/update` with a user uuid, a type specifying whether to `add` or `remove` permissions, and an array of permission paths (of the format `/Layer`).

to get your groups as a user, use `GET /a/u/groups`. this will return all groups that a given user belongs to.

### defining groups

groups are declaratively defined in configuration.
a group defines layer permissions and a key-value store of per-app access rules.
groups also have a priority field to allow rules of certain groups to supersede other rules.

here is an example group called `Friends` that has access to the `/Food` layer (and thus the `FrenchFry` app we previously described)

```toml
[[groups]]
name = "Friends"
priority = 1
permissions = [ "/Food" ]
    [groups.rules]
    FrenchFry.quota = 10
```

when a user authenticates to a remote application, it can obtain the list of rules that apply to the user for that specific app. this is extremely useful for remote configuration such as quotas.

## issuing an app token

for our `FrenchFry` app, `GET /a/app/token/FrenchFry` to request that the server grant an app token. if the user has been granted permission to the app, a token will be returned.
for fetching user details along with an app token, use `GET /a/app/login/FrenchFry` to get a `RemoteIdentity`, containing both public user information and a token.

## fetching user info

once the remote app recieves an app token, it can use the remote app api to access relevant information.
the remote api is accessible under `/a/remote`.

use `GET /a/remote/user` to get public user details, respecting their privacy settings.