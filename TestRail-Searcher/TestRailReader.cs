using System;
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

        public bool TryLogin()
        {
            try
            {
                JObject c = (JObject)this._client.SendGet("get_user_by_email&email=" + _client.User);
                Console.WriteLine(c["email"]);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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

        public string GetSuiteName(int suiteId)
        {
            JObject c = (JObject)this._client.SendGet("get_suite/" + suiteId);
            return c["name"].ToString();
        }

        public JArray GetTestCases(int projectId, int suiteId)
        {
            JArray c = (JArray)this._client.SendGet("get_cases/" + projectId + "&suite_id=" + suiteId);
            return c;
        }
    }
}
