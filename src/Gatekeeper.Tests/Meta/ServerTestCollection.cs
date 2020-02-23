#region

using Gatekeeper.Tests.Base;
using Xunit;

#endregion

namespace Gatekeeper.Tests.Meta {
    [CollectionDefinition(KEY)]
    public class ServerTestCollection : ICollectionFixture<ServerTestFixture> {
        public const string KEY = "server";
    }
}