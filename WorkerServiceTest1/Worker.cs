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
        private IWebCrawler _iwc;

        public Worker(ILogger<Worker> logger,IAddDatabase iad,IWebCrawler iwc)
        {
            _logger = logger;
            _iad = iad;
            _iwc = iwc;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            //_iad.dbOpen();
            //_logger.LogInformation("database open");
            //_iad.dbClose();
            _iwc.SaveHtmlTxt(@"https://www.steepandcheap.com/Store/catalog/search.jsp?s=u&q=arcteryx+men+alpha+sv");
            _logger.LogInformation("OK!");
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                _iad.AddOneRecord();
                await Task.Delay(new TimeSpan(0,0,10), stoppingToken);
            }
        }


    }
}
