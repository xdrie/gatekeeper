#region

using Gatekeeper.Tests.Meta;
using Xunit;

#endregion

namespace Gatekeeper.Tests.Base {
    [Collection(ContextTestCollection.KEY)]
    public class DependencyTests {
        protected readonly DependencyFixture fx;

        public DependencyTests(DependencyFixture fx) {
            this.fx = fx;
        }
    }
}