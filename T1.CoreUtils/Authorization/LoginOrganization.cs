using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using Newtonsoft.Json;

namespace T1.CoreUtils.Authorization
{
    public class LoginOrganization<TIdType, TRoleEnum> where TRoleEnum : struct, IConvertible
    {
        [JsonProperty("id")]
        public TIdType Id { get; set; }

        [JsonProperty("n")]
        public string Name { get; set; }

        [JsonProperty("r")]
        public List<TRoleEnum> Roles { get; set; }

        public LoginOrganization(TIdType id, string name = "Default", List<TRoleEnum> roles = null)
        {
            this.Id = id;
            this.Name = name;
            this.Roles = roles ?? new List<TRoleEnum>();
        }
    }
}