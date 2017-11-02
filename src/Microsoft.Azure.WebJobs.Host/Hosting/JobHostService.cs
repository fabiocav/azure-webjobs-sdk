using Microsoft.Azure.WebJobs.Host.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.WebJobs.Hosting
{
    public class JobHostService : IHostedService
    {
        private readonly ILogger<JobHostService> _logger;
        private readonly IConfiguration _configuration;
        private readonly JobHost _jobHost;

        public JobHostService(IConfiguration configuration, IOptions<JobHostOptions> jobHostOptions, ILogger<JobHostService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration;
            _jobHost = new JobHost();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            _jobHost.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => _jobHost.StartAsync(cancellationToken);
    }
}
