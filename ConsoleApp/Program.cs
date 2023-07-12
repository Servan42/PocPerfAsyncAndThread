using PocPerfAsyncAndThread;

List<string> links = new List<string>();
using(StreamReader sr = new StreamReader("links.txt"))
{
    while(!sr.EndOfStream)
    {
        links.Add(sr.ReadLine());
    }
}
Console.WriteLine($"Read {links.Count} links");

IHttpClientAdapter httpClientAdapter = new HttpClientAdapter();
PerfService perfService = new PerfService(links, httpClientAdapter);

int numberOfLinksToLoad = 400;
Console.WriteLine($"Stats for {numberOfLinksToLoad} links");
if(links.Count < numberOfLinksToLoad) numberOfLinksToLoad = links.Count;

Console.Write("LoadLinks:                     ");
TimeSpan initialtimeSpan = perfService.LoadLinks(numberOfLinksToLoad);
Console.WriteLine(initialtimeSpan);

Console.Write("LoadLinksAsync:                ");
TimeSpan timeSpan = await perfService.LoadLinksAsync(numberOfLinksToLoad);
Console.WriteLine(timeSpan + $"    {((initialtimeSpan.Ticks - timeSpan.Ticks) * 100) / initialtimeSpan.Ticks}% faster");

Console.Write("LoadLinksThreaded8Cores:       ");
timeSpan = perfService.LoadLinksThreaded8Cores(numberOfLinksToLoad);
Console.WriteLine(timeSpan + $"    {((initialtimeSpan.Ticks - timeSpan.Ticks) * 100) / initialtimeSpan.Ticks}% faster");

Console.Write("LoadLinks8Tasks:               ");
timeSpan = perfService.LoadLinks8Tasks(numberOfLinksToLoad);
Console.WriteLine(timeSpan + $"    {((initialtimeSpan.Ticks - timeSpan.Ticks) * 100) / initialtimeSpan.Ticks}% faster");

Console.Write("LoadLinksParallelForeach:      ");
timeSpan = perfService.LoadLinksParallelForeach(numberOfLinksToLoad);
Console.WriteLine(timeSpan + $"    {((initialtimeSpan.Ticks - timeSpan.Ticks) * 100) / initialtimeSpan.Ticks}% faster");

Console.Write("LoadLinksParallelForeachAsync: ");
timeSpan = await perfService.LoadLinksParallelForeachAsync(numberOfLinksToLoad);
Console.WriteLine(timeSpan + $"    {((initialtimeSpan.Ticks - timeSpan.Ticks) * 100) / initialtimeSpan.Ticks}% faster");

