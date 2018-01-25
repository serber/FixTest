using System.Collections.Generic;
using System.Threading.Tasks;
using FixTest.Entities;

namespace FixTest.Services.Interfaces
{
    public interface IWebSiteService
    {
        IReadOnlyCollection<WebSite> ListActive();

        IReadOnlyCollection<WebSite> List();

        Task CheckWebSite(long id);

        Task<WebSite> Get(long id);

        Task<WebSite> Add(string url, long interval);

        Task<WebSite> Edit(long id, string url, long interval);
    }
}