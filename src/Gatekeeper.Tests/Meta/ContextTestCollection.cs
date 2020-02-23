#region

using Gatekeeper.Tests.Base;
using Xunit;

#endregion

namespace Gatekeeper.Tests.Meta {
    [CollectionDefinition(KEY)]
    public class ContextTestCollection : ICollectionFixture<DependencyFixture> {
        public const string KEY = "scontext";
    }
}