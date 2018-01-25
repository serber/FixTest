using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FixTest.Entities;
using FixTest.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FixTest.Services.Schedule
{
    public class ScheduleService : IHostedService, IScheduleService
    {
        private CancellationTokenSource _cts;

        private readonly IServiceScopeFactory _scopeFactory;

        private readonly ILogger<IHostedService> _logger;

        private readonly Timer _timer;
        
        private IList<ScheduleInfo> _schedules = new List<ScheduleInfo>();

        public ScheduleService(IServiceScopeFactory scopeFactory, ILogger<IHostedService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;

            _timer = new Timer(state => RunSchedules(), null, Timeout.Infinite, Timeout.Infinite);
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            LoadSchedules();

            RunSchedules();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);

            _cts.Cancel();

            cancellationToken.ThrowIfCancellationRequested();

            return Task.CompletedTask;
        }

        private void LoadSchedules()
        {
            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                DateTimeOffset now = DateTimeOffset.Now;

                IWebSiteService webSiteService = scope.ServiceProvider.GetService<IWebSiteService>();

                IReadOnlyCollection<WebSite> webSitesList = webSiteService.ListActive();

                foreach (WebSite webSite in webSitesList)
                {
                    _schedules.Add(new ScheduleInfo
                    {
                        Key = webSite.Id,
                        Interval = webSite.CheckInterval,
                        NextRun = now.AddSeconds(webSite.CheckInterval)
                    });
                }
            }
        }

        private void RunSchedules()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);

            if (_schedules.Count == 0)
            {
                return;
            }

            DateTimeOffset now = DateTimeOffset.Now;

            _schedules = _schedules.OrderBy(x => x.NextRun).ToList();

            ScheduleInfo schedule = _schedules.First();

            if (schedule.NextRun <= now)
            {
                ExecuteSchedule(schedule);

                schedule.NextRun = now.AddSeconds(schedule.Interval);

                RunSchedules();

                return;
            }

            TimeSpan interval = schedule.NextRun - now;

            if (interval <= TimeSpan.Zero)
            {
                RunSchedules();
                return;
            }

            _timer.Change(interval, interval);
        }

        private void ExecuteSchedule(ScheduleInfo schedule)
        {
            Task task = new Task(async () =>
            {
                _logger.LogInformation($"Execute check #{schedule.Key} at {schedule.NextRun}");

                using (IServiceScope scope = _scopeFactory.CreateScope())
                {
                    IWebSiteService webSiteService = scope.ServiceProvider.GetService<IWebSiteService>();

                    await webSiteService.CheckWebSite(schedule.Key);
                }
            }, _cts.Token);
            
            task.Start();
        }

        public void AddOrUpdate(WebSite webSite)
        {
            ScheduleInfo existSchedule = _schedules.SingleOrDefault(x => x.Key == webSite.Id);
            if (existSchedule == null)
            {
                if (webSite.CheckInterval > 0)
                {
                    _schedules.Add(new ScheduleInfo
                    {
                        Key = webSite.Id,
                        Interval = webSite.CheckInterval,
                        NextRun = DateTimeOffset.Now.AddSeconds(webSite.CheckInterval)
                    });

                    RunSchedules();
                }
            }
            else
            {
                if (webSite.CheckInterval == 0)
                {
                    _schedules.Remove(existSchedule);
                }
                else
                {
                    existSchedule.Interval = webSite.CheckInterval;
                    existSchedule.NextRun = DateTimeOffset.Now.AddSeconds(webSite.CheckInterval);
                }

                RunSchedules();
            }
        }
    }
}