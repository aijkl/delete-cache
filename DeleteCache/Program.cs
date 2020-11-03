using Aijkl.CloudFlare.Cache;
using Aijkl.CloudFlare.Cache.Models;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Octokit;
using System.Linq;
using System.IO;

namespace CloudFlare
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string json = string.Empty;
#if DEBUG 
            json =  File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}appsettings.json");
#endif
#if !DEBUG
            json = args[0];
#endif
            AppSettings settings = JsonConvert.DeserializeObject<AppSettings>(json);            
            GitHubClient github = new GitHubClient(new ProductHeaderValue(settings.GitHub.UserAgent));
            github.Credentials = new Credentials(settings.GitHub.Token);
            using CloudFlareClient cloudFlareClient = new CloudFlareClient(settings.CloudFlare.EmailAdress, settings.CloudFlare.AuthToken);

            Repository repository = await github.Repository.Get(settings.GitHub.Username, settings.GitHub.Repository);
            Branch master = await github.Repository.Branch.Get(repository.Id, settings.GitHub.Branch);
            Branch ghPages = await github.Repository.Branch.Get(repository.Id, settings.GitHub.Branch);
            GitHubCommit commit = await github.Repository.Commit.Get(repository.Id, master.Commit.Sha);            

            if (commit.Files.Count > 0)
            {
                await cloudFlareClient.Zone.PurgeFilesByUrl(settings.CloudFlare.Zone, commit.Files.Select(x => $"{settings.Core.Url}/{x.Filename}").ToList());
                await github.Repository.Comment.Create(repository.Id, ghPages.Commit.Sha, new NewCommitComment("skip ci"));
            }
            Console.WriteLine($"PurgeFiles:{commit.Files.Count} DateTime:{DateTime.Now} Repository:{repository}");
        }
    }
}
