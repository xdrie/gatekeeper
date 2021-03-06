namespace Gatekeeper.Server.OpenApi {
    public static class GateApiConstants {
        public static class Tags {
            public const string USER_MANAGEMENT = "User Management";
            public const string SERVER_META = "Meta";
            public const string ADMIN = "Administration";
            public const string USER_DIRECTORY = "User Directory";
            public const string AUTH_PROVIDER = "Authentication Provider";
            public const string REMOTE_APP = "Remote App";
        }

        public static class Security {
            public const string USER_BEARER_AUTH = "UserBearerAuth";
            public const string REMOTE_APP_APIKEY = "RemoteBearerAuth";
        }
    }
}