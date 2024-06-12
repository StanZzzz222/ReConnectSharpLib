using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibReConnect.Model
{
    /// <summary>
    /// Auth 对象
    /// @RED
    /// </summary>
    public class Auth
    {
        public virtual String Account { get; set; }
        public virtual String Password { get; set; }
        public virtual String Token { get; set; }

        public Auth() { }
        public Auth(String account, String password, String token) 
        { 
            this.Account = account;
            this.Password = password;
            this.Token = token;
        }
    }
}
