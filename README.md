
# gatekeeper

Gatekeeper is ALTiCU's unified authentication service.

## key features
+ Centralized user and role management
+ Authentication provider for external applications
+ Convenient and secure authentication
    + Two-factor authentication with TOTP
    + Mobile-QR trusted-device authentication
+ Extensible support for integrating with other applications

## dev

### dependencies
+ ASP.NET Core 3.0 (+ Entity Framework Core)

### build

```
dotnet build Gatekeeper.sln
```

### tests
```
cd Gatekeeper.Tests
dotnet test
```
