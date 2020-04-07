namespace Gatekeeper.Models.Requests {
    public class UpdateGroupRequest {
        public string userUuid { get; set; }
        public string type { get; set; }
        public string[] groups { get; set; }

        
    }
}