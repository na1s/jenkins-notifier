using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;

namespace JenkinsRestClient
{
    public class JenkinsRestApi : IJenkinsRestApi
    {
        private RestClient _client;
        private JsonNetDeserializer _deserializer;

        public JenkinsRestApi(string baseUrl)
        {
            _client = new RestClient(baseUrl) {CookieContainer = new CookieContainer()};

            _deserializer = new JsonNetDeserializer();

            _client.RemoveHandler("application/json");
            _client.RemoveHandler("text/json");
            _client.RemoveHandler("text/x-json");
            _client.RemoveHandler("text/javascript");

            _client.AddHandler("application/json", _deserializer);
            _client.AddHandler("text/json", _deserializer);
            _client.AddHandler("text/x-json", _deserializer);
            _client.AddHandler("text/javascript", _deserializer);
        }
        public void Login(string username, string password)
        {
            var request = new RestRequest("api/json", Method.GET);

            request.AddParameter("user", username);
            request.AddParameter("password", password);

            var response = _client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException("Not authenticated");
        }

        public IEnumerable<Job> GetJobs()
        {
            throw new System.NotImplementedException();
        }
    }
}