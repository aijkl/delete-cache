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
            try
            {
                string json = string.Empty;
#if DEBUG
                json = File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}appsettings.json");
#endif
#if !DEBUG
            json = args[0];
#endif
                AppSettings settings = JsonConvert.DeserializeObject<AppSettings>(json);
                GitHubClient github = new GitHubClient(new ProductHeaderValue(settings.GitHub.UserAgent))
                {
                    Credentials = new Credentials(settings.GitHub.Token)
                };
                using CloudFlareClient cloudFlareClient = new CloudFlareClient(settings.CloudFlare.EmailAdress, settings.CloudFlare.AuthToken);

                Repository repository = await github.Repository.Get(settings.GitHub.Username, settings.GitHub.Repository);
                Branch master = await github.Repository.Branch.Get(repository.Id, settings.GitHub.Branch);
                Branch ghPages = await github.Repository.Branch.Get(repository.Id, settings.GitHub.Branch);
                GitHubCommit commit = await github.Repository.Commit.Get(repository.Id, master.Commit.Sha);
                var comments = github.Repository.Comment.GetAllForCommit(repository.Id, commit.Sha).Result;
                bool purge = commit.Files.Count > 0 && comments.Count > 0 && comments.Last().Body.Equals("skip ci");
                if (true)
                {
                    await cloudFlareClient.Zone.PurgeFilesByUrl(settings.CloudFlare.Zone, commit.Files.Select(x => $"{settings.Core.Url}/{x.Filename}/").ToList());
                    await github.Repository.Comment.Create(repository.Id, ghPages.Commit.Sha, new NewCommitComment("skip ci"));
                }
                Console.WriteLine($"PurgeFiles:{(purge ? commit.Files.Count : 0)} DateTime:{DateTime.Now} Repository:{repository.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception {ex.StackTrace}");
            }            
        }
    }
}
