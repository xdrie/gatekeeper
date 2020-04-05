using Gatekeeper.Models.Identity;
using Xunit;

namespace Gatekeeper.Tests.Models {
    public class AccessScopeTests {
        [Theory]
        [InlineData("*")]
        [InlineData("/")]
        [InlineData("/Layer")]
        [InlineData("/Layer/App")]
        [InlineData("/Layer1/Layer2/App")]
        public void canParseScopesFromPath(string path) {
            // ensure parsed path matches input path
            Assert.Equal(path, AccessScope.parse(path).path);
        }

        [Theory]
        [InlineData("/", "*/")]
        [InlineData("/", "*/Test")]
        [InlineData("/", "/Layer")]
        [InlineData("/Layer", "/Layer/App")]
        [InlineData("/Layer/Sublayer", "/Layer/Sublayer/App")]
        public void canCompareScopes(string greater, string lesser) {
            AccessScope greaterScope = AccessScope.parse(greater);
            AccessScope lesserScope = AccessScope.parse(lesser);
            Assert.True(greaterScope.greaterThan(lesserScope));
        }
    }
}