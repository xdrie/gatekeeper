using System;
using System.Net;
using Degate.Modules;
using Degate.Utilities;
using FrenchFry.Demo.Config;
using FrenchFry.Demo.Models;
using FrenchFry.Demo.Models.Responses;
using Hexagon.Serialization;

namespace FrenchFry.Demo.Modules {
    public class FryModule : BridgeAuthModule<SContext> {
        public FryModule(SContext serverContext) : base("/fries", serverContext) {
            Get("/", async (req, res) => {
                var account = getAccount();
                await res.respondSerialized(new MonthlyResponse {
                    quota = getFryQuota(),
                    used = account.orderedFries
                });
            });
            Post("/order", async (req, res) => {
                var account = getAccount();
                if (account.orderedFries >= getFryQuota()) {
                    res.StatusCode = (int) HttpStatusCode.PaymentRequired;
                    return;
                }

                account.orderedFries++;
                serverContext.userManager.save(account);
                res.StatusCode = (int) HttpStatusCode.Accepted;
                var r = new Random();
                await res.respondSerialized(new FryResponse {
                    mysterious = r.Next()
                });
            });
        }

        public long getFryQuota() {
            remoteUser.rules.getAppRule<long>(SContext.GATE_APP, "quota", 0, out var fryQuota);
            return fryQuota;
        }

        public Account getAccount() => serverContext.userManager.findByUid(userId);
    }
}