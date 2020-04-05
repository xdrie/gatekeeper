using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Gatekeeper.Models.Access;

namespace Gatekeeper.Models.Identity {
    public class User : DatabaseObject {
        public string name { get; set; }
        public string username { get; set; }
        public string email { get; set; }

        /// <summary>
        /// a unique identifier for the user
        /// </summary>
        public string uuid { get; set; }

        public bool emailVisible { get; set; } = false;
        public CryptSecret password { get; set; }
        public byte[]? totp { get; set; }
        public bool totpEnabled { get; set; } = false;
        public Pronouns pronouns { get; set; }
        public Role role { get; set; } = Role.Pending;
        public string verification { get; set; }
        public DateTime registered { get; set; }
        public string groupList { get; set; }

        [NotMapped]
        public string[] groups {
            get => groupList.Split(',');
            set => groupList = string.Join(",", value);
        }

        public enum Role {
            Pending,
            User,
            Admin
        }

        public enum Pronouns {
            TheyThem,
            HeHim,
            SheHer
        }
    }
}