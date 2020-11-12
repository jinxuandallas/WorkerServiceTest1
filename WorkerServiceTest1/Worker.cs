using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkerServiceTest1.BLL;

namespace WorkerServiceTest1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IAddDatabase _iad;

        public Worker(ILogger<Worker> logger,IAddDatabase iad)
        {
            _logger = logger;
            _iad = iad;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            //_iad.dbOpen();
            _logger.LogInformation("database open");
            //_iad.dbClose();
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                _iad.AddOneRecord();
                await Task.Delay(1000, stoppingToken);
            }
        }


    }
}
