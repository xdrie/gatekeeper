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
using Hexagon.Services.Application;
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

        public async Task initialize() {
            // register account
            username = $"{AccountRegistrar.TEST_USERNAME}_{StringUtils.secureRandomString(4)}";
            authedUser = await AccountRegistrar.registerAccount(getClient(), username);
        }

        public HttpClient getAuthedClient() {
            var client = base.getClient();
            client.addUserToken(authedUser);
            return client;
        }
    }
}