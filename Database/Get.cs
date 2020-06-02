using System;
using System.Threading;
using System.Web;
using HtmlAgilityPack;
using Serilog;
using ShellProgressBar;

namespace LGFA.Database
{
    internal class Get
    {
        public static bool GetPlayerIds(string system, string trigger, int seasonId, ProgressBar pbar)
        {
            var web = new HtmlWeb();
            HtmlNodeCollection findPlayerCount;

            var doc = web.Load(Fetch.GetUrl(system, trigger, seasonId, 1));

            findPlayerCount = doc.DocumentNode.SelectNodes("//*[@id='lgtable_memberstats51']/tbody/tr");
            if (findPlayerCount == null)
            {
                doc = web.Load(Fetch.GetUrl(system, trigger, seasonId, 0));
                findPlayerCount = doc.DocumentNode.SelectNodes("//*[@id='lgtable_memberstats51']/tbody/tr");
                
            }

            var childOptions = new ProgressBarOptions
            {
                ForegroundColor = ConsoleColor.Green,
                BackgroundColor = ConsoleColor.DarkGreen,
                ProgressCharacter = '\u2593',
                CollapseWhenFinished = false,
                DisplayTimeInRealTime = false,
                ShowEstimatedDuration = true,
                EnableTaskBarProgress = false
            };
            try
            {
                using var child = pbar.Spawn(findPlayerCount.Count, $"Updating Season: {seasonId} player ids",
                    childOptions);

                for (var i = 1; i < findPlayerCount.Count; i++)
                {
                    var findPlayerNodes =
                        doc.DocumentNode.SelectNodes($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]");

                    #region nodes

                    foreach (var player in findPlayerNodes)
                    {
                        child.EstimatedDuration = TimeSpan.FromMilliseconds(findPlayerCount.Count * 25);
                        var playerName = player
                            .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[2]/a").InnerText;
                        var playerShortUrl = player
                            .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[2]/a")
                            .Attributes["href"].Value;

                        var playerUrl = string.Join(string.Empty,
                            "https://www.leaguegaming.com/forums/" + playerShortUrl);
                        var tempId = HttpUtility.ParseQueryString(new Uri(playerUrl).Query);
                        var playerId = int.Parse(tempId.Get("userid"));

                        Writer.SaveInformation(playerId, playerName, playerUrl, system);
                    }

                    Thread.Sleep(25);

                    #endregion

                    child.Tick();
                }
            }
            catch (Exception e)
            {
                Log.Logger.Fatal(e, "Error processing Player stats.");
                throw;
            }

            return false;
        }

        public static bool GetPlayerIds(string system, string trigger, int seasonId)
        {
            var web = new HtmlWeb();
            HtmlNodeCollection findPlayerCount;

            var doc = web.Load(Fetch.GetUrl(system, trigger, seasonId, 1));

            findPlayerCount = doc.DocumentNode.SelectNodes("//*[@id='lgtable_memberstats51']/tbody/tr");
            if (findPlayerCount == null)
            {
                doc = web.Load(Fetch.GetUrl(system, trigger, seasonId, 0));
                findPlayerCount = doc.DocumentNode.SelectNodes("//*[@id='lgtable_memberstats51']/tbody/tr");
            }

            try
            {

                for (var i = 1; i < findPlayerCount.Count; i++)
                {
                    var findPlayerNodes =
                        doc.DocumentNode.SelectNodes($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]");

                    #region nodes

                    foreach (var player in findPlayerNodes)
                    {
                        var playerName = player
                            .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[2]/a").InnerText;
                        var playerShortUrl = player
                            .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[2]/a")
                            .Attributes["href"].Value;

                        var playerUrl = string.Join(string.Empty,
                            "https://www.leaguegaming.com/forums/" + playerShortUrl);
                        var tempId = HttpUtility.ParseQueryString(new Uri(playerUrl).Query);
                        var playerId = int.Parse(tempId.Get("userid"));

                        Writer.SaveInformation(playerId, playerName, playerUrl, system);
                    }
                    #endregion
                }
            }
            catch (Exception e)
            {
                Log.Logger.Fatal(e, "Error processing Player stats.");
                throw;
            }
            return false;
        }
    }
}