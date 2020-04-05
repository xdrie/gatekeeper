#region

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

#endregion

namespace Gatekeeper.Server.Models.Identity {
    public class CryptSecret : DatabaseObject {
        public byte[] salt { get; set; }

        public byte[] hash { get; set; }

        public int iterations { get; set; }

        public int length { get; set; }

        public int saltLength { get; set; }

        public static CryptSecret withDefaultParameters() {
            return new CryptSecret {
                iterations = DEFAULT_ITERATION_COUNT,
                length = DEFAULT_LENGTH,
                saltLength = DEFAULT_SALT_LENGTH
            };
        }

        public const int DEFAULT_ITERATION_COUNT = 10000;
        public const int DEFAULT_LENGTH = 128;
        public const int DEFAULT_SALT_LENGTH = 64;
    }
}