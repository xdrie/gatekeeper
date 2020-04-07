using System.Collections.Generic;
using System.Linq;
using Gatekeeper.Models.Access;
using Hexagon.Utilities;

namespace Degate.Utilities {
    public static class RuleExtensions {
        public static bool getAppRule<T>(this IEnumerable<AccessRule> rules, string app, string key, T defaultVal, out T val) {
            var rule = rules.SingleOrDefault(x => x.app == app && x.key == key);
            if (rule == null) {
                val = defaultVal;
                return false;
            }

            val = Parse.As<T>(rule.value);
            return true;
        }
    }
}