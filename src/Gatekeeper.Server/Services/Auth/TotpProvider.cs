using System;
using OtpNet;

namespace Gatekeeper.Server.Services.Auth {
    public class TotpProvider {
        public Totp totp;
        public const int TOTP_SECRET_LENGTH = 32;

        public TotpProvider(byte[] secret) {
            totp = new Totp(secret, mode: OtpHashMode.Sha256, totpSize: 6);
        }

        public string getCode() {
            return totp.ComputeTotp(DateTime.UtcNow);
        }

        public bool verify(string code) {
            return totp.VerifyTotp(DateTime.UtcNow, code, out long timeWindowUsed);
        }
    }
}