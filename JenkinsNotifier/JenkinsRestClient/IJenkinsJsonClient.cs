using System.Collections.Generic;

namespace JenkinsRestClient
{
    public interface IJenkinsJsonClient
    {
        IEnumerable<Job> GetAllJobs();
        void StartJob(Job job);
    }
}