using System.Text;

namespace Gatekeeper.Utils {
    public static class StringUtils {
        public static string secureRandomString(uint length,
            string charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890") {
            var chars = charset.ToCharArray();
            var data = CryptoUtils.randomBytes(length);

            var result = new StringBuilder((int)length);
            foreach (var b in data) {
                result.Append(chars[b % chars.Length]);
            }

            return result.ToString();
        }
    }
}