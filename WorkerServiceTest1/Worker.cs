using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkerServiceTest1.BLL;
using System.IO;

namespace WorkerServiceTest1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        //private IAddDatabase _iad;
        private IWebCrawler _iwc;

        public Worker(ILogger<Worker> logger, IWebCrawler iwc)
        {
            _logger = logger;
            //_iad = iad;
            _iwc = iwc;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            //_iad.dbOpen();
            //_logger.LogInformation("database open");
            //_iad.dbClose();

            //_iwc.SaveHtmlTxt(@"https://www.steepandcheap.com/Store/catalog/search.jsp?s=u&q=arcteryx+men+alpha+sv");
            //_iwc.SaveHtmlTxt(@"https://www.steepandcheap.com/Store/catalog/search.jsp?s=u&q=sfdsfdsdfsd");

            string html;

            using (StreamReader sr = new StreamReader(@"Example\example.txt"))
            {
                html = sr.ReadToEnd();
            }

            string result = _iwc.CrawlSteepandCheap(html);

            _logger.LogInformation(result);


            _logger.LogInformation("OK!");
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //_iad.AddOneRecord();
                await Task.Delay(new TimeSpan(0, 0, 5), stoppingToken);
            }
        }


    }
}
