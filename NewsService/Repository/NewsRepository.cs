using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NewsService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace NewsService.Repository
{
    /// <summary>
    /// Class for fascilitating CRUD operations on news entity
    /// </summary>
    public class NewsRepository : INewsRepository
    {
        /// <summary>
        /// readonly property for database context
        /// </summary>
        readonly NewsContext newsContext;

        /// <summary>
        /// Parametrised constructor for injecting database context
        /// </summary>
        /// <param name="newsContext"></param>
        public NewsRepository(NewsContext newsContext)
        {
            this.newsContext = newsContext;
        }

        /// <summary>
        /// Method for adding or updating a reminder
        /// </summary>
        /// <param name="userId">The id of the user of the news</param>
        /// <param name="newsId">The id of the news to whom reminder is to be added or updated</param>
        /// <param name="reminder">The object with new values for reminder</param>
        /// <returns>True if operation success else false</returns>
        public async Task<bool> AddOrUpdateReminder(string userId, int newsId, Reminder reminder)
        {
            var builder = Builders<UserNews>.Filter;

            ///Finding the matching document
            var filter = builder.Eq(u => u.UserId, userId)
                & builder.ElemMatch(u => u.NewsList, n => n.NewsId == newsId);

            ///Configure updation of reminder
            var update = Builders<UserNews>.Update.Set("NewsList.$.Reminder", reminder);

            ///Update the reminder of the matching document if reminder already present else insert it
            var result = await newsContext.News.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
            return result.IsAcknowledged && (result.ModifiedCount > 0 || result.UpsertedId != null);
        }

        /// <summary>
        /// Method for creating a news
        /// </summary>
        /// <param name="userId">The id of the user who is creating the news</param>
        /// <param name="news">The properties of the news to be added</param>
        /// <returns>The id of the newly created news</returns>
        public async Task<int> CreateNews(string userId, News news)
        {
            ///The custom starting id of news
            news.NewsId = 101;
            var filter = Builders<UserNews>.Filter.Eq(u => u.UserId, userId);
            var userNewsResult = await newsContext.News.FindAsync(filter);

            ///Check whether user exists or not
            var userNews = await userNewsResult.FirstOrDefaultAsync();

            ///if user doesnt exists create a new document and insert the news
            if (userNews == null)
            {
                var newUserNews = new UserNews()
                {
                    UserId = userId,
                    NewsList = new List<News>()
                    {
                        news
                    }
                };
                await newsContext.News.InsertOneAsync(newUserNews);
                var inserted = await GetNewsById(userId, news.NewsId);
                return inserted != null ? inserted.NewsId : -1;
            }
            ///If user exists and user has any news find the maximum id of the news present
            else if (userNews != null && userNews.NewsList != null && userNews.NewsList.Any())
            {
                news.NewsId = userNews.NewsList.Max(n => n.NewsId) + 1;
            }

            ///Configure update for pushing new news to the list of news of the user
            var update = Builders<UserNews>.Update.Push(u => u.NewsList, news);

            ///Update the newslist of the user if present or else insert the item
            var result = await newsContext.News.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
            if (result.IsAcknowledged && (result.ModifiedCount > 0 || result.UpsertedId != null))
            {
                return news.NewsId;
            }
            return -1;
        }

        /// <summary>
        /// Method for deleting a news
        /// </summary>
        /// <param name="userId">The id of the user of the news</param>
        /// <param name="newsId">The id of the news to be deleted</param>
        /// <returns>True if deletion successful</returns>
        public async Task<bool> DeleteNews(string userId, int newsId)
        {
            var builder = Builders<UserNews>.Filter;

            ///Pulling out the matching news from the list
            var pull = Builders<UserNews>.Update.PullFilter(u => u.NewsList, n => n.NewsId == newsId);

            ///Filter for finding the matching document
            var filter = builder.Eq(u => u.UserId, userId) & builder.ElemMatch(u => u.NewsList, n => n.NewsId == newsId);

            ///update the matching document with the newslist from which the news to be deleted was pulled out
            var result = await newsContext.News.UpdateOneAsync(filter, pull);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        /// <summary>
        /// Method for deleting reminder of a news
        /// </summary>
        /// <param name="userId">The id of the user of the news</param>
        /// <param name="newsId">The id of the news whose reminder is to be deleted</param>
        /// <returns>True if deletion is successful</returns>
        public async Task<bool> DeleteReminder(string userId, int newsId)
        {
            var builder = Builders<UserNews>.Filter;

            ///Filter for finding the matching document
            var filter = builder.Eq(u => u.UserId, userId) & builder.ElemMatch(u => u.NewsList, n => n.NewsId == newsId);
            var update = Builders<UserNews>.Update.Unset("NewsList.$.Reminder");
            var result = await newsContext.News.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
            return result.IsAcknowledged && (result.ModifiedCount > 0 || result.UpsertedId != null);
        }

        /// <summary>
        /// Method for finding all news by the user
        /// </summary>
        /// <param name="userId">The id of the user whose news are to be fetched</param>
        /// <returns>The list of news of the user</returns>
        public async Task<List<News>> FindAllNewsByUserId(string userId)
        {
            var result = await newsContext.News.FindAsync(u => string.Equals(userId, u.UserId));
            var userNews = await result.FirstOrDefaultAsync();
            return userNews?.NewsList;
        }


        /// <summary>
        /// Method for finding a news by its id
        /// </summary>
        /// <param name="userId">The id of the user of the news</param>
        /// <param name="newsId">The id of the news to be found</param>
        /// <returns>The news with matching criterias</returns>
        public async Task<News> GetNewsById(string userId, int newsId)
        {
            var builder = Builders<UserNews>.Filter;

            ///Filter for finding matching document
            var filter = builder.Eq(u => u.UserId, userId) &
                builder.ElemMatch(u => u.NewsList, n => n.NewsId == newsId);

            ///Configuring a projection for fetching only the matching news from the document
            var projection = Builders<UserNews>.Projection.ElemMatch(u => u.NewsList, n => n.NewsId == newsId);

            ///Finding the news from filter based on projection
            var result = await newsContext.News.FindAsync(filter, new FindOptions<UserNews, UserNews> { Projection = projection });
            var userNews = await result.SingleOrDefaultAsync();

            ///There should be only one news with an id
            if (userNews != null && userNews.NewsList?.Count == 1)
            {
                return userNews.NewsList.First();
            }
            return null;
        }

        /// <summary>
        /// Method to check whether a news already exists or not
        /// </summary>
        /// <param name="userId">The id of the user of the news</param>
        /// <param name="title">The title of the news</param>
        /// <returns>True if news exists else false</returns>
        public async Task<bool> IsNewsExist(string userId, string title)
        {
            var builder = Builders<UserNews>.Filter;
            var filter = builder.Eq(u => u.UserId, userId)
                & builder.ElemMatch(u => u.NewsList, n => string.Equals(n.Title, title));
            var news = await newsContext.News.FindAsync(filter);
            if (news.Any())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method to find whether a reminder exists for a news or not
        /// </summary>
        /// <param name="userId">The id of the user of the news</param>
        /// <param name="newsId">The id of the news in which the presence of reminder is to be checked</param>
        /// <returns>True if reminder exists for the news</returns>
        public async Task<bool> IsReminderExists(string userId, int newsId)
        {
            var news = await GetNewsById(userId, newsId);
            if (news != null && news.Reminder != null)
            {
                return true;
            }
            return false;
        }
    }
}
