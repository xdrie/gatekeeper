#region

using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Gatekeeper.Config;
using Gatekeeper.Models;
using Gatekeeper.Models.Identity;
using Gatekeeper.Models.Responses;
using Gatekeeper.Tests.Utilities;
using Hexagon.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

#endregion

namespace Gatekeeper.Tests.Base {
    public class UserTestFixture : ServerTestFixture {
        public string username;
        public AuthedUserResponse authedUser;
        private bool initialized = false;

        public async Task initialize() {
            if (initialized) throw new InvalidOperationException("already initialized");
            initialized = true;
            // register account
            username = $"{AccountRegistrar.TEST_USERNAME}_{StringUtils.secureRandomString(4)}";
            authedUser = await new AccountRegistrar(serverContext).registerAccount(getClient(), username, true);
        }

        public async Task<HttpClient> getAuthedClient() {
            await initialize();
            var client = base.getClient();
            client.addToken(authedUser.token);
            return client;
        }
    }
}