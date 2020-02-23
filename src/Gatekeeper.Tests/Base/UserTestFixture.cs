#region

using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Gatekeeper.Config;
using Gatekeeper.Models;
using Gatekeeper.Tests.Utilities;
using Hexagon.Services.Application;
using Hexagon.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

namespace Gatekeeper.Tests.Base {
    public class UserTestFixture : ServerTestFixture {
        public string username;

        public async Task initialize() {
            // register account
            var response = await AccountRegistrar.registerAccount(getClient(),
                $"{AccountRegistrar.TEST_USERNAME}_{StringUtils.secureRandomString(4)}");
            response.EnsureSuccessStatusCode();
        }
    }
}