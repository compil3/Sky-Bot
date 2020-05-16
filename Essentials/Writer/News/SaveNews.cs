using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LGFA.Properties;
using LiteDB;
using LGFA.Modules.News;

namespace LGFA.Essentials.Writer.News
{
    public class SaveNews
    {
        public static bool SaveTrade(DateTime dateStamp, string teamOne, string teamTwo, string system, string table)
        {
            DateTime dbDateTime = new DateTime();
            const int index = 1;

            using (var tradeDb = new LiteDatabase(@"News.db"))
            {
                var news = tradeDb.GetCollection<NewsProperties.News>(table);
                var result = news.Find(x => x.Id == index);

                foreach (var datetime in result)
                {
                    dbDateTime = datetime.date;
                }
            }

            using (var database = new LiteDatabase("News.db"))
            {
                var newsCollection = database.GetCollection<NewsProperties.News>(table);
                newsCollection.EnsureIndex(x => x.date);
                var newsFeed = new NewsProperties.News
                {
                    Id = index,
                    date = dateStamp,
                    newsLineOne = teamOne,
                    newsLineTwo = teamTwo
                };
                try
                {
                    var dateFound = newsCollection.FindById(index);
                    var value = DateTime.Compare(dbDateTime, dateStamp);
                    if (dateFound == null)
                    {
                        newsCollection.Insert(newsFeed);
                        return true;
                    } else if (dateFound != null)
                    {
                        if (value < 0)
                        {
                            newsCollection.Update(newsFeed);
                            return true;
                        }
                        else if (value == 0) return false;
                        else return false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return false;
        }

        public static bool SaveWaivers(DateTime dateStamp, string line, string system, string table)
        {
            DateTime dbDateTime = new DateTime();
            
            using (var waiverDb = new LiteDatabase("News.db"))
            {
                var waiver = waiverDb.GetCollection<NewsProperties.Waivers>(table);
                var result = waiver.Find(x => x.Id == 1);
                foreach (var dateTime in result)
                {
                    dbDateTime = dateTime.dateTime;
                }
            }

            using (var database = new LiteDatabase("News.db"))
            {
                var newsCollection = database.GetCollection<NewsProperties.Waivers>(table);
                newsCollection.EnsureIndex(x => x.dateTime);

                int Id = 1;
                var waiverFeed = new NewsProperties.Waivers()
                {
                    Id = Id,
                    dateTime = dateStamp,
                    line = line
                };

                try
                {
                    var dateFound = newsCollection.FindById(1);
                    var value = DateTime.Compare(dbDateTime, dateStamp);

                    if (dateFound == null)
                    {
                        newsCollection.Insert(waiverFeed);
                        return true;
                    } else if (dateFound != null)
                    {
                        if (value < 0)
                        {
                            newsCollection.Update(waiverFeed);
                            return true;
                        } else if (value == 0) return false;
                        else return false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return false;
        }
    }
}
