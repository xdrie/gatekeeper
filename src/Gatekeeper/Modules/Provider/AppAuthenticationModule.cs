using System;
using System.Linq;
using System.Net;
using Carter.Request;
using Gatekeeper.Config;
using Gatekeeper.Models.Identity;
using Gatekeeper.Models.Responses;
using Hexagon.Services.Serialization;

namespace Gatekeeper.Modules.Provider {
    public class AppAuthenticationModule : AuthenticatedUserModule {
        public AppAuthenticationModule(SContext serverContext) : base(AccessScope.rootScope, "/app", serverContext) {
            Get("/token/{appId}", async (req, res) => {
                var appId = req.RouteValues.As<string>("appId");
                // check config for app layers
                var appDef = serverContext.config.apps.FirstOrDefault(x => x.name == appId);
                if (appDef == null) {
                    res.StatusCode = (int) HttpStatusCode.NotFound;
                    return;
                }
                // check if any permission grants app access
                var grantedScope = default(string);
                foreach (var permission in currentUser.permissions) {
                    var permissionScope = new AccessScope(permission.path);
                    foreach (var appPath in appDef.paths) {
                        var appScope = new AccessScope(appPath);
                        if (appScope.atLeast(permissionScope)) { // if permission grants app
                            grantedScope = permissionScope.path;
                        }
                    }
                }

                if (grantedScope == null) {
                    res.StatusCode = (int) HttpStatusCode.Unauthorized;
                    return;
                }
                
                // issue a new token
                // TODO: configurable timespan
                var token = serverContext.tokenAuthenticator.issue(grantedScope, TimeSpan.FromDays(7));

                res.StatusCode = (int) HttpStatusCode.Created;
                await res.respondSerialized(token);
            });
        }
    }
}