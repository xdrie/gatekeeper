#region

using Gatekeeper.Tests.Base;
using Xunit;

#endregion

namespace Gatekeeper.Tests.Meta {
    [CollectionDefinition(KEY)]
    public class UserTestCollection : ICollectionFixture<UserTestFixture> {
        public const string KEY = "user";
    }
}