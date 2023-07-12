using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocPerfAsyncAndThread
{
    public class PerfService
    {
        private readonly List<string> links;
        private readonly IHttpClientAdapter httpClientAdapter;

        public PerfService(List<string> links, IHttpClientAdapter httpClientAdapter)
        {
            this.links = links;
            this.httpClientAdapter = httpClientAdapter;
        }

        public TimeSpan LoadLinks(int numberOfLinksToLoad)
        {
            DateTime startTime = DateTime.Now;
            foreach (string link in links.Take(numberOfLinksToLoad))
            {
                string html = this.httpClientAdapter.GetStringAsync(link).Result;
            }
            return DateTime.Now - startTime;
        }

        public async Task<TimeSpan> LoadLinksAsync(int numberOfLinksToLoad)
        {
            DateTime startTime = DateTime.Now;
            foreach (string link in links.Take(numberOfLinksToLoad))
            {
                string html = await this.httpClientAdapter.GetStringAsync(link);
            }
            return DateTime.Now - startTime;
        }

        public TimeSpan LoadLinksThreaded8Cores(int numberOfLinksToLoad)
        {
            List<Thread> threads = new();
            Dictionary<int, List<string>> splittedLinks = new Dictionary<int, List<string>>();
            for (int i = 0; i < 8; i++)
            {
                splittedLinks.Add(i, new List<string>());
            }

            int j = 0;
            foreach (string link in links.Take(numberOfLinksToLoad))
            {
                if (j >= 8) j = 0;
                splittedLinks[j].Add(link);
                j++;
            }

            foreach (var kvp in splittedLinks)
            {
                threads.Add(new Thread(() =>
                {
                    foreach (string link in kvp.Value)
                    {
                        string html = this.httpClientAdapter.GetStringAsync(link).Result;
                    }
                }));
            }

            DateTime startTime = DateTime.Now;
            foreach (Thread thread in threads) thread.Start();
            foreach (Thread thread in threads) thread.Join();
            return DateTime.Now - startTime;
        }

        public TimeSpan LoadLinks8Tasks(int numberOfLinksToLoad)
        {
            Task[] tasks = new Task[8];
            Dictionary<int, List<string>> splittedLinks = new Dictionary<int, List<string>>();
            for (int i = 0; i < 8; i++)
            {
                splittedLinks.Add(i, new List<string>());
            }

            int j = 0;
            foreach (string link in links.Take(numberOfLinksToLoad))
            {
                if (j >= 8) j = 0;
                splittedLinks[j].Add(link);
                j++;
            }

            foreach (var kvp in splittedLinks)
            {
                tasks[kvp.Key] = new Task(() =>
                {
                    foreach (string link in kvp.Value)
                    {
                        string html = this.httpClientAdapter.GetStringAsync(link).Result;
                    }
                });
            }

            DateTime startTime = DateTime.Now;
            foreach (Task task in tasks) task.Start();
            Task.WaitAll(tasks);
            return DateTime.Now - startTime;
        }

        public TimeSpan LoadLinksParallelForeach(int numberOfLinksToLoad)
        {
            DateTime startTime = DateTime.Now;
            Parallel.ForEach(links.Take(numberOfLinksToLoad), link =>
            {
                string html = this.httpClientAdapter.GetStringAsync(link).Result;
            });
            return DateTime.Now - startTime;
        }

        public async Task<TimeSpan> LoadLinksParallelForeachAsync(int numberOfLinksToLoad)
        {
            DateTime startTime = DateTime.Now;
            await Parallel.ForEachAsync(links.Take(numberOfLinksToLoad), async (link, token) =>
            {
                string html = await this.httpClientAdapter.GetStringAsync(link);
            });
            return DateTime.Now - startTime;
        }
    }
}
