
# sample flow

this sample flow will walk through integration with gatekeeper as an authentication provider.

we will be referencing a fictional app called `FrenchFry`.
when an authenticated user calls `POST frenchfry.local/order_fry`, they will place an order for a single fry.
since fries are a limited resource, especially within alticu, every user has a monthly fry quota.

gatekeeper simplifies adding the authentication layer for this app.
firstly, app layer management allows admins to grant access to the `/Food` layer to a select group of users.
