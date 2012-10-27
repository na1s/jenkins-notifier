using System.Collections.Generic;

namespace JenkinsRestClient
{
    public interface IJenkinsRestApi
    {
        IEnumerable<Job> GetJobs();
    }
}