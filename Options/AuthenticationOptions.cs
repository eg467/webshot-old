using System.Collections.Generic;

namespace Webshot
{
    public class ProjectCredentials
    {
        public Dictionary<string, AuthCredentials> CredentialsByDomain { get; set; } =
            new Dictionary<string, AuthCredentials>();
    }

    public class AuthCredentials
    {
        public string User { get; set; }
        public string Password { get; set; }

        public AuthCredentials()
        {
        }

        public AuthCredentials(string user, string password)
        {
            this.User = user;
            this.Password = password;
        }
    }
}