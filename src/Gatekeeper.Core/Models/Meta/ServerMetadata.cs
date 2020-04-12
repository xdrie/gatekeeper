namespace Gatekeeper.Models.Meta {
    public class ServerMetadata {
        public string name { get; set; }
        public string version { get; set; }
#if DEBUG
        public bool development { get; set; }
#endif
    }
}