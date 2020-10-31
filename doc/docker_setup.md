
# docker setup

a quick guide on suggested configuration and setup for running Gatekeeper in docker.

we will set up a docker container that:
+ exposes port 80 to host 19000
+ connects to host postgres via unix domain socket
+ links in configuration file from host

## build

first, we need to build the `gatekeeper:alpine` image.

from the repository root, run:
```sh
docker build --pull -t gatekeeper:alpine -f src/docker/Dockerfile .
```

## preparation

first, update the configuration file to tell it how to connect to postgres:

`config_prod.toml`
```toml
[server]
database_connection = "Host=/var/run/postgresql;Database=gatekeeper;Username=user"
database_backend = "postgres"
```

## run

run the container:

```sh
docker run --rm --name gk -p 19000:80 -v $(pwd)/config_prod.toml:/app/config.toml -v /var/run/postgresql:/var/run/postgresql gatekeeper:alpine
```
