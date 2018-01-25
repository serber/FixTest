using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FixTest.Entities;
using FixTest.Services.Interfaces;
using FixTest.Utils;
using Microsoft.Extensions.Logging;

namespace FixTest.Services
{
    public class WebSiteService : IWebSiteService
    {
        private readonly FixDbContext _dataContext;

        private readonly ILogger<IWebSiteService> _logger;

        public WebSiteService(FixDbContext dataContext, ILogger<IWebSiteService> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public IReadOnlyCollection<WebSite> ListActive()
        {
            return _dataContext.Set<WebSite>()
                             .Where(x => x.CheckInterval > 0)
                             .ToList();
        }

        public IReadOnlyCollection<WebSite> List()
        {
            return _dataContext.Set<WebSite>().ToList();
        }

        public async Task CheckWebSite(long id)
        {
            try
            {
                await _dataContext.Database.BeginTransactionAsync();

                WebSite webSite = _dataContext.Set<WebSite>().Find(id);

                webSite.IsAvailable = await WebSiteChecker.Check(webSite.Url);
                webSite.LastCheckedAt = DateTimeOffset.Now;

                _dataContext.Update(webSite);
                await _dataContext.SaveChangesAsync();

                _dataContext.Database.CommitTransaction();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                _dataContext.Database.RollbackTransaction();
            }
        }

        public async Task<WebSite> Get(long id)
        {
            return await _dataContext.Set<WebSite>().FindAsync(id);
        }

        public async Task<WebSite> Add(string url, long interval)
        {
            try
            {
                await _dataContext.Database.BeginTransactionAsync();

                WebSite webSite = new WebSite(url, interval);

                await _dataContext.AddAsync(webSite);

                await _dataContext.SaveChangesAsync();

                _dataContext.Database.CommitTransaction();

                return webSite;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _dataContext.Database.RollbackTransaction();

                return null;
            }
        }

        public async Task<WebSite> Edit(long id, string url, long interval)
        {
            try
            {
                await _dataContext.Database.BeginTransactionAsync();

                WebSite webSite = _dataContext.Set<WebSite>().Find(id);

                webSite.Url = url;
                webSite.CheckInterval = interval;

                _dataContext.Update(webSite);
                await _dataContext.SaveChangesAsync();

                _dataContext.Database.CommitTransaction();

                return webSite;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _dataContext.Database.RollbackTransaction();

                return null;
            }
        }
    }
}