using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Webshot
{
    public sealed class ChromeAuthExtension : IDisposable
    {
        private readonly ProjectCredentials _creds;
        private readonly string _dir;
        private readonly HashSet<string> _createdFiles = new HashSet<string>();

        public ChromeAuthExtension(ProjectCredentials creds, string dir)
        {
            dir = dir ?? "";
            creds = creds ?? new ProjectCredentials();

            _creds = creds;
            _dir = dir;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            else if (Directory.GetFiles(dir).Any())
            {
                throw new Exception("The extension directory must be empty.");
            }
        }

        public bool IsNeeded => _creds.CredentialsByDomain.Any();

        /// <summary>
        ///
        /// </summary>
        /// <returns>The path of the created zip file.</returns>
        public void CreateZip(string targetPath)
        {
            CreateManifestFile();
            CreateWebRequestFile();
            ZipFile.CreateFromDirectory(_dir, targetPath);
        }

        private void CreateManifestFile()
        {
            const string Filename = "manifest.json";
            var contents =
@"{
""name"":""Webrequest API"",
""version"":""1.0"",
""description"":""Extension to handle Authentication window"",
""permissions"":[""webRequest"",""webRequestBlocking"",""<all_urls>""],
""background"":{""scripts"":[""webrequest.js""]},
""manifest_version"": 2
}";
            WriteFile(Filename, contents);
        }

        /// <summary>
        /// Escapes a single quote within a string (WILL BREAK ALREADY ESCAPED STRINGS).
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private string EscapeSingleQuote(string s) => s.Replace("'", @"\'");

        private void CreateWebRequestFile()
        {
            const string Filename = "webrequest.js";

            var creds = _creds.CredentialsByDomain.Select(x => $@"
if(details.challenger
    && details.challenger.host
    && details.challenger.host.toUpperCase() === '{EscapeSingleQuote(x.Key.ToUpper())}') {{
    return {{
        'authCredentials':{{
            username:'{EscapeSingleQuote(x.Value.User)}',
            password:'{EscapeSingleQuote(x.Value.Password)}'
        }}
    }};
}} ");
            var allCreds = string.Join("", creds);
            var content = @"
chrome.webRequest.onAuthRequired.addListener(
    function handler(details){" + allCreds + @"},
    {urls:['<all_urls>']},
    ['blocking']
);";
            WriteFile(Filename, content);
        }

        private void WriteFile(string filename, string contents)
        {
            var filepath = Path.Combine(_dir, filename);
            File.WriteAllText(filepath, contents);
            _createdFiles.Add(filepath);
        }

        public void Dispose()
        {
            _createdFiles.ForEach(File.Delete);
        }
    }
}