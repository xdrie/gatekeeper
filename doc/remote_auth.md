
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
secret = "VERY_SECURE"
```

## admin permission management

membership to default layers can be configured in `users.default_layers`.
however, if the administrator wishes to create a layer only available to specific users, the permission management API can be used to grant those users layer permissions.

an admin account is needed for using admin functions. follow the basic account creation steps again. at the end, this time, update the database to set the user's role to `Admin`.

to modify permissions, use `PATCH /a/perms/update` with a user uuid, a type specifying whether to `add` or `remove` permissions, and an array of permission paths (of the format `/Layer`).

to get your permissions as a user, use `GET /a/perms`. this will return all permissions associated with a given user account.

## issuing an app token

for our `FrenchFry` app, `GET /a/app/token/FrenchFry` to request that the server grant an app token. if the user has been granted permission to the app, a token will be returned.

## fetching user info

once the remote app recieves an app token, it can use the remote app api to access relevant information.
the remote api is accessible under `/a/remote`.

use `GET /a/remote/user` to get public user details, respecting their privacy settings.