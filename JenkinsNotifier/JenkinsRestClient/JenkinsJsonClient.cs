using System.Collections.Generic;
using System.Net;
using RestSharp;

namespace JenkinsRestClient
{
    public class JenkinsJsonClient : IJenkinsJsonClient
    {
        private readonly RestClient _client;
        
        public JenkinsJsonClient(string host)
        {
            _client = new RestClient("http://" + host) { CookieContainer = new CookieContainer() };
        }

        private JenkinsData GetAllData()
        {
            var request = new RestRequest("/api/json", Method.GET);
            var response = _client.Execute<JenkinsData>(request);
            return response.Data;
        }

        public IEnumerable<Job> GetAllJobs()
        {
            return GetAllData().Jobs;
        }

        public void StartJob(Job job)
        {
            var request = new RestRequest("/job/"+job.Name+"/build", Method.POST);
            _client.Execute(request);
        }
    }
}