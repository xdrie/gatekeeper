
# local auth overview

this provides a brief introduction to the functionality a frontend to Gatekeeper should provide.

all specific REST api documentation is available via OpenAPI, which is built in to the server.
the openapi file can be found at `GET /openapi`, and the web documentation browser through Swagger can be found at `GET /swagger`.

## basic configuration

configuration is stored in `config.toml`. see the [configuration model](../src/Gatekeeper/Config/SConfig.cs) for a detailed list of options.

of particular interest may be `server.database` for setting the database path, and the `logging` module for configuring logging.

see [remote auth] for details on how to configure the auth provider.

## creating the account
use the `POST /a/auth/create` route to register an account. for pronouns, use one of `SheHer`, `TheyThem`, or `HeHim`.
in debug mode, the `isRobot` value is checked against `"I am not a robot"`.

when given a valid request, gatekeeper will return an object with a `user` and a `token`.
for further authenticated requests, use `token.content` as a Bearer token (pass `Authorization: Bearer <token>` as a header)

after creating the account, it will be marked as `Pending` until it is verified. in order to verify, simply `POST /a/auth/verify/{uuid}/{code}` (in debug mode, `code` defaults to `00000000`).

the account should now have a role of `User` and be usable for most standard operations.

## other account operations
other basic account operations are available under the `User Management` module.

## get user account info

`GET /a/u/me` to get information about the currently authenticated user.
