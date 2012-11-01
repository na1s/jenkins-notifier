using System.Collections.Generic;

namespace JenkinsRestClient
{
    public interface IJenkinsRestApi
    {
        void Login(string username, string password);
        IEnumerable<Job> GetJobs();
    }
}