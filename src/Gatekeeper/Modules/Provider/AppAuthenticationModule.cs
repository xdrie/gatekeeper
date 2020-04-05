using System;
using System.Linq;
using System.Net;
using Carter.Request;
using Gatekeeper.Config;
using Gatekeeper.Models.Identity;
using Gatekeeper.OpenApi.Provider;
using Hexagon.Serialization;

namespace Gatekeeper.Modules.Provider {
    public class AppAuthenticationModule : AuthenticatedUserModule {
        public AppAuthenticationModule(SContext serverContext) : base("/app", serverContext) {
            Get<GetAppToken>("/token/{appId}", async (req, res) => {
                var appId = req.RouteValues.As<string>("appId");
                // check config for app layers
                var appDef = serverContext.config.apps.SingleOrDefault(x => x.name == appId);
                if (appDef == null) {
                    res.StatusCode = (int) HttpStatusCode.NotFound;
                    return;
                }

                // load user permissions
                var user = serverContext.userManager.loadPermissions(currentUser);
                // check if any permission grants app access
                var maybeGrantedScope = default(AccessScope?);
                foreach (var permission in user.permissions) {
                    var permissionScope = AccessScope.parse(permission.path); // permission: "/Layer" or "/Layer/App"
                    foreach (var appDefLayer in appDef.layers) {
                        var appScope = new AccessScope(appDefLayer, appDef.name); // scope: "/Layer/App"
                        if (permissionScope.greaterThan(appScope)) {
                            maybeGrantedScope = appScope; // the granted scope is the one we authorize
                        }
                    }
                }

                if (!maybeGrantedScope.HasValue) {
                    res.StatusCode = (int) HttpStatusCode.Forbidden;
                    return;
                }

                var grantedScope = maybeGrantedScope.Value;

                // issue a new token for the app
                // TODO: configurable timespan
                var token = serverContext.userManager.issueTokenFor(user.dbid,
                        serverContext.tokenAuthenticator.issue(grantedScope, TimeSpan.FromDays(7)));

                res.StatusCode = (int) HttpStatusCode.Created;
                await res.respondSerialized(token);
            });
        }
    }
}