using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;

namespace WpfParSiteAn
{
    public static class SiteParser
    {
        private static IDictionary<string, int> GetKeywordCount(IList<string> keywords, string siteURI)
        {
            string downloadString = "";
            try
            {
                using (var client = new WebClient())
                {
                    downloadString = client.DownloadString(siteURI);
                }
            }
            catch
            {
                // log something here if needed(site is unaccesible etc)
            }
            return keywords.ToDictionary(x => x, y => downloadString.CountStringOccurrences(y)); // returns dictionary where key is keyword, value is number of occurences
        }

        public static void AnalyseSites(IList<string> sites, IList<string> keywords, Action<IList<string>> updater, CancellationToken ct)
        {
            var taskArray = sites.Select(x => Task<IDictionary<string, int>>.Factory.StartNew(() => GetKeywordCount(keywords, x))).ToList(); // get list of tasks. ToList forces lazy init. Otherwise tasks will be started when first accessed i.e. in foreach and because task.Result is synchronous operation it will lead to sequental site parsing
            var keywordCounts = new List<string>();
            for (var i = 0; i < taskArray.Count; i++) {
                var task = taskArray[i];
                task.Wait(5000);
                foreach (var keywordCount in task.Result)
                {
                    if (ct.IsCancellationRequested)
                        return;
                    keywordCounts.Add("On site '" + sites[i] + "' keyword '" + keywordCount.Key + "' was found " + keywordCount.Value + " times");
                }
            }
            updater(keywordCounts);
        }
    }
}
