using Aijkl.CloudFlare.Cache;
using Aijkl.CloudFlare.Cache.Models;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Octokit;
using System.Linq;
using System.IO;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using Newtonsoft.Json.Serialization;
using Google.Apis.Services;

namespace CloudFlare
{
    class Program
    {
        static GitHubClient GithubClient;
        static DriveService DriveServiceClient;
        static CloudFlareClient CloudFlareClient;
        static AppSettings AppSettings;
        static async Task Main(string[] args)
        {
            try
            {
                string json = args.Length > 0 && !string.IsNullOrEmpty(args[0]) ? args[0] : File.ReadAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}appsettings.json");
                AppSettings = JsonConvert.DeserializeObject<AppSettings>(json);
                Initialize();
                //CreateCache();
                await DeleteCache();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception {ex.StackTrace}");
            }            
        }           
        static void CreateCache()
        {
            Console.WriteLine("[INFO] Connecting GoogleDrive");
            Console.WriteLine("[INFO] StartCacheing");
        }
        static async Task DeleteCache()
        {
            Repository repository = await GithubClient.Repository.Get(AppSettings.GitHub.Username, AppSettings.GitHub.Repository);
            Branch master = await GithubClient.Repository.Branch.Get(repository.Id, AppSettings.GitHub.Branch);
            Branch ghPages = await GithubClient.Repository.Branch.Get(repository.Id, AppSettings.GitHub.Branch);
            GitHubCommit commit = await GithubClient.Repository.Commit.Get(repository.Id, master.Commit.Sha);
            var comments = GithubClient.Repository.Comment.GetAllForCommit(repository.Id, commit.Sha).Result;
            bool purge = commit.Files.Count > 0 && comments.Count > 0 && comments.Last().Body.Equals("skip ci");
            if (purge)
            {
                await CloudFlareClient.Zone.PurgeFilesByUrl(AppSettings.CloudFlare.Zone, commit.Files.Select(x => $"{AppSettings.Core.Url}/{x.Filename}").ToList());
                await GithubClient.Repository.Comment.Create(repository.Id, ghPages.Commit.Sha, new NewCommitComment("skip ci"));
            }
            Console.WriteLine($"PurgeFiles:{(purge ? commit.Files.Count : 0)} DateTime:{DateTime.Now} Repository:{repository.Id}");
        }
        static void Initialize()
        {
            GithubClient = new GitHubClient(new ProductHeaderValue(AppSettings.GitHub.UserAgent))
            {
                Credentials = new Credentials(AppSettings.GitHub.Token)
            };
            Console.WriteLine("[INFO] Login GitHubClient");

            //ICredential credential = GoogleCredential.FromJson(AppSettings.GoogleDrive.AothToken).CreateScoped(new string[] { DriveService.Scope.Drive }).UnderlyingCredential;
            //DriveServiceClient = new DriveService(new BaseClientService.Initializer()
            //{
            //HttpClientInitializer = credential
            //});
            Console.WriteLine("[INFO] Login GoogleDriveClient");

            CloudFlareClient = new CloudFlareClient(AppSettings.CloudFlare.EmailAdress, AppSettings.CloudFlare.AuthToken);
            Console.WriteLine("[INFO] Login CloudFlare");            
        }
    }
}
