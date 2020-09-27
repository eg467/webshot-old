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

        public string DecryptUser() => string.IsNullOrEmpty(User) ? null : Encryption.Unprotect(User);

        public string DecryptPassword() => string.IsNullOrEmpty(Password) ? null : Encryption.Unprotect(Password);

        /// <summary>
        /// Stores encrypted credentials for web authentication.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        public AuthCredentials(string user, string password)
        {
            this.User = Encryption.Protect(user);
            this.Password = Encryption.Protect(password);
        }
    }
}