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

int numberOfLinksToLoad = 100;
Console.WriteLine($"Stats for {numberOfLinksToLoad} links");
if(links.Count < numberOfLinksToLoad) numberOfLinksToLoad = links.Count;

Console.Write("LoadLinks:                     ");
Console.WriteLine(perfService.LoadLinks(numberOfLinksToLoad));
Console.Write("LoadLinksAsync:                ");
Console.WriteLine(await perfService.LoadLinksAsync(numberOfLinksToLoad));
Console.Write("LoadLinksThreaded8Cores:       ");
Console.WriteLine(perfService.LoadLinksThreaded8Cores(numberOfLinksToLoad));
Console.Write("LoadLinks8Tasks:               ");
Console.WriteLine(perfService.LoadLinks8Tasks(numberOfLinksToLoad));
Console.Write("LoadLinksParallelForeach:      ");
Console.WriteLine(perfService.LoadLinksParallelForeach(numberOfLinksToLoad));
Console.Write("LoadLinksParallelForeachAsync: ");
Console.WriteLine(await perfService.LoadLinksParallelForeachAsync(numberOfLinksToLoad));
