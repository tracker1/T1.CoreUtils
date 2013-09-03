using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using Newtonsoft.Json;

namespace T1.CoreUtils.Authorization
{
    public class LoginUser<TIdType, TRoleEnum> : IPrincipal where TRoleEnum : struct, IConvertible
    {
        #region Properties
        [JsonProperty("id")]
        public TIdType Id { get; set; }

        [JsonProperty("u")]
        public string Username { get; set; }

        [JsonProperty("f")]
        public string FirstName { get; set; }

        [JsonProperty("m")]
        public string MiddleName { get; set; }

        [JsonProperty("l")]
        public string LastName { get; set; }

        [JsonProperty("org")]
        public LoginOrganization<TIdType, TRoleEnum> CurrentOrg { get; set; }

        [JsonProperty("orgs")]
        public List<LoginOrganization<TIdType, TRoleEnum>> Organizations { get; set; }
        #endregion

        #region IPrincipal
        //for accessing if a user has a role, and implementing IPrincipal
        bool IPrincipal.IsInRole(string role)
        {
            TRoleEnum roleToCheck;
            if (!Enum.TryParse<TRoleEnum>(role, true, out roleToCheck)) return false; //invalid role

            //any logged in user has guest and user privileges
            if (roleToCheck.ToString().ToLower() == "guest") return true;
            if (roleToCheck.ToString().ToLower() == "user") return true;

            if (CurrentOrg != null || CurrentOrg.Roles == null) return false;

            return CurrentOrg.Roles.Contains(roleToCheck);
        }

        private GenericIdentity _Identity;
        //don't serialize/deserialize this property for storage, it's useless
        [JsonIgnore]
        IIdentity IPrincipal.Identity
        {
            get
            {
                if (_Identity != null && _Identity.Name == this.Username) return _Identity;
                return _Identity = new GenericIdentity(this.Username);
            }
        }
        #endregion

    }
}