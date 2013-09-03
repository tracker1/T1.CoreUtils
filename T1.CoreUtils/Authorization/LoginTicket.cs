using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T1.CoreUtils.Authorization
{
    public class LoginTicket<TIdType, TRoleEnum> where TRoleEnum : struct, IConvertible
    {
        [JsonProperty("e")]
        public DateTimeOffset Expiration { get; set; }

        [JsonProperty("u")]
        public LoginUser<TIdType, TRoleEnum> User { get; set; }
        
        public bool Expired { get {
            return Expiration < DateTimeOffset.UtcNow;
        } }

        public LoginTicket() { }

        public LoginTicket(LoginUser<TIdType, TRoleEnum> user, bool persistent = true, DateTimeOffset? Expiration = null)
        {
            this.User = user;
            this.Expiration = Expiration ?? (persistent ? DateTime.UtcNow.AddDays(14) : DateTime.UtcNow.AddHours(12));
        }
    }
}
