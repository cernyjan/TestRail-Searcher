using System;
using System.Linq;
using Gurock.TestRail;
using Newtonsoft.Json.Linq;

namespace TestRail_Searcher
{
    class TestRailReader
    {
        readonly APIClient _client;
        
        public TestRailReader(string server, string user, string password)
        {
            this._client = new APIClient(server);
            _client.User = user;
            _client.Password = password;
        }

        public bool TryLogin(string domain = "")
        {
            var email = _client.User;
            try
            {
                JObject c;
                if (domain.Equals(string.Empty))
                {
                    c = (JObject)this._client.SendGet("get_user_by_email&email=" + email);
                }
                else
                {
                    c = (JObject)this._client.SendGet("get_user_by_email&email=" + email.Replace(domain, "@quadient.com"));
                }
                
                Console.WriteLine(c["email"]);
                return true;
            }
            catch (Exception ex)
            {
                Program.LogException(ex);
                return false;
            }
        }

        public JArray GetProjects()
        {
            JArray c = (JArray)this._client.SendGet("get_projects");
            return c;
        }

        public JArray GetSuites(int projectId)
        {
            JArray c = (JArray)this._client.SendGet("get_suites/" + projectId);
            return c;
        }

        public JArray GetTestCases(int projectId, int suiteId)
        {
            JArray c = (JArray)this._client.SendGet("get_cases/" + projectId + "&suite_id=" + suiteId);
            return c;
        }
        public JArray GetSections(int projectId, int suiteId)
        {
            JArray c = (JArray)this._client.SendGet("get_sections/" + projectId + "&suite_id=" + suiteId);
            return c;
        }

        public JArray GetCaseFields()
        {
            JArray c = (JArray)this._client.SendGet("get_case_fields");
            return c;
        }

        public JArray GetUsers()
        {
            JArray c = (JArray)this._client.SendGet("get_users");
            return c;
        }
    }
}
