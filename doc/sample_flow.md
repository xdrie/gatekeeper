
# sample flow

this sample flow will walk through integration with gatekeeper as an authentication provider.

we will be referencing a fictional app called `FrenchFry`.
when an authenticated user calls `POST https://frenchfry.local/order_fry`, they will place an order for a single fry.
since fries are a limited resource, especially within alticu, every user has a monthly fry quota.

## gatekeeper setup

gatekeeper simplifies adding the authentication layer for this app.
on the server side, a group called `Insiders` is defined.
the `Insiders` group has access to the `/Food` layer, and the rule `FrenchFry.quota = 100`, entitling each member to that many monthly fries.
there is also a group called `Friends`, which can also access the `/Food` layer, but has rule `FrenchFry.quota = 10`.

the corresponding configuration is shown here:

```toml

[[apps]]
name = "FrenchFry"
layers = [ "/Food" ]
secret = "yeet"

[[groups]]
name = "Friends"
priority = 1
permissions = [ "/Food" ]
    [groups.rules]
    FrenchFry.quota = 10

[[groups]]
name = "Insiders"
priority = 10
permissions = [ "/Food" ]
    [groups.rules]
    FrenchFry.quota = 100
```

## login

the user wants to get authorized to the fry order page.
so they visit `https://frenchfry.local/login`, which redirects them to `https://auth.alt.icu/link?app=FrenchFry?cb=https://frenchfry.local/gate`.
this will present a consent screen to the user, who will accept, and then they will be redirected to `POST https://frenchfry.local/gate` with the token in form data.

## frenchfry session

when `FrenchFry` receives `POST /gate`, it will then request user details from gatekeeper by using the remote api.
this user details object will contain basic identity information such as `username`, `name`, and `uuid`, and also a list of `rules`.
internally, it will create a session with the identity and rules and then issue an apikey to the `FrenchFry` frontend.

from now on, when the frontend uses its apikey, the `FrenchFry` server will match it to the cached user session, and can from there get user information and quota information.
