using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Carter.Request;
using Gatekeeper.Models.Identity;
using Gatekeeper.Server.Config;
using Gatekeeper.Server.OpenApi.Provider;
using Gatekeeper.Server.Services.Users;
using Hexagon.Serialization;
using Microsoft.AspNetCore.Http;

namespace Gatekeeper.Server.Modules.Provider {
    public class AppAuthenticationModule : AuthenticatedUserModule {
        public async Task<(RemoteIdentity, HttpStatusCode)> getAppAuthentication(HttpRequest req, HttpResponse res) {
            var appId = req.RouteValues.As<string>("appId");
            // check config for app layers
            var appDef = serverContext.config.apps.SingleOrDefault(x => x.name == appId);
            if (appDef == null) {
                return (null, HttpStatusCode.NotFound);
            }

            // load user groups, and aggregate their permissions
            var user = serverContext.userManager.loadGroups(currentUser);
            var userPermissions = new PermissionResolver(serverContext, user)
                .aggregatePermissions();
            // check if any permission grants app access
            var maybeGrantedScope = default(AccessScope?);
            foreach (var permission in userPermissions) {
                var permissionScope = AccessScope.parse(permission.path); // permission: "/Layer" or "/Layer/App"
                foreach (var appDefLayer in appDef.layers) {
                    var appScope = new AccessScope(appDefLayer, appDef.name); // scope: "/Layer/App"
                    if (permissionScope.greaterThan(appScope)) {
                        maybeGrantedScope = appScope; // the granted scope is the one we authorize
                    }
                }
            }

            if (!maybeGrantedScope.HasValue) {
                return (null, HttpStatusCode.Forbidden);
            }

            var grantedScope = maybeGrantedScope.Value;

            // issue a new token for the app
            // TODO: configurable timespan
            var token = serverContext.userManager.issueTokenFor(user.id,
                serverContext.tokenResolver.issue(grantedScope, TimeSpan.FromDays(7)));

            return (new RemoteIdentity(new PublicUser(user), token), HttpStatusCode.Created);
        }

        public AppAuthenticationModule(SContext serverContext) : base("/app", serverContext) {
            Get<GetAppToken>("/token/{appId}", async (req, res) => {
                var (authIdentity, status) = await getAppAuthentication(req, res);
                res.StatusCode = (int) status;
                if (authIdentity != null) {
                    await res.respondSerialized(authIdentity.token);
                }
            });
            Get<GetAppIdentity>("/login/{appId}", async (req, res) => {
                var (authIdentity, status) = await getAppAuthentication(req, res);
                res.StatusCode = (int) status;
                if (authIdentity != null) {
                    await res.respondSerialized(authIdentity);
                }
            });
        }
    }
}