using System;
using System.Collections.Generic;
using System.Text;
using LGFA.Properties;
using LiteDB;
using Serilog;

namespace LGFA.Modules.News
{
    class NewsWriter
    {
        public static bool SaveTrade(DateTime date, string teamOne, string teamTwo, string system)
        {
            DateTime dbDateTime = new DateTime();
            const int index = 1;

            using var tradesDb = new LiteDatabase(@"Filename=Database/LGFA.db;connection=shared");
            var trade = tradesDb.GetCollection<LeagueNews.News>("Trades");
            var result = trade.Find(x => x.Id == index);

            foreach (var datetime in result)
            {
                dbDateTime = datetime.Date;
            }

            using var database = new LiteDatabase(@"Filename=Database/LGFA.db;connection=shared");
            var tradeCollection = database.GetCollection<LeagueNews.News>("Trades");
            tradeCollection.EnsureIndex(x => x.Date);
            var tradeNews = new LeagueNews.News
            {
                Id = index,
                Date = date,
                NewsLineOne = teamOne,
                NewsLineTwo = teamTwo,
                System = system
            };
            try
            {
                var datefound = tradeCollection.FindById(index);
                var value = DateTime.Compare(dbDateTime, date);
                if (datefound == null)
                {
                    tradeCollection.Insert(tradeNews);
                    return true;
                } else if (datefound != null)
                {
                    if (value < 0)
                    {
                        tradeCollection.Update(tradeNews);
                        return true;
                    }
                    else if (value == 0) return false;
                    else return false;
                }
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Exception thrown: {e}");
                throw;
            }
            return false;
        }

        public static bool SaveWaiver(DateTime dateStamp, string line, string system)
        {
            DateTime dbDateTime = new DateTime();
            const int index = 1;

            using var waiverDb = new LiteDatabase(@"Filename=Database/LGFA.db;connection=shared");
            var waiver = waiverDb.GetCollection<LeagueNews.Waivers>("Waivers");
            var result = waiver.Find(x => x.Id == index);
            foreach (var datetime in result)
            {
                dbDateTime = datetime.Date;
            }

            waiver.EnsureIndex(x => x.Date);
            var waiverNews = new LeagueNews.Waivers
            {
                Id = index,
                Date = dateStamp,
                Line = line,
                System = system
            };
            try
            {
                var dateFound = waiver.FindById(index);
                var value = DateTime.Compare(dbDateTime, dateStamp);
                if (dateFound == null)
                {
                    waiver.Insert(waiverNews);
                    return true;
                } else if (dateFound != null)
                {
                    if (value < 0)
                    {
                        waiver.Update(waiverNews);
                        return true;
                    } else if (value == 0) return false;
                }
                else return false;
            }
            catch (Exception e)
            {
                
                Log.Logger.Error($"Exception thrown: {e}");
                throw;
            }
            return false;
        }
    }
}
