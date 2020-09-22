using NewsService.Exceptions;
using NewsService.Models;
using NewsService.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace NewsService.Services
{
    /// <summary>
    /// The service class for reminder entity
    /// </summary>
    public class NewsService : INewsService
    {
        /// <summary>
        /// readonly property for repository
        /// </summary>
        readonly INewsRepository newsRepository;

        /// <summary>
        /// Parametrised constructor for injecting repository
        /// </summary>
        /// <param name="newsRepository"></param>
        public NewsService(INewsRepository newsRepository)
        {
            this.newsRepository = newsRepository;
        }

        /// <summary>
        /// Method for adding or updating a reminder of a news
        /// </summary>
        /// <param name="userId">The id of the user of the news</param>
        /// <param name="newsId">The id of the news to whom reminder is to be added or updated</param>
        /// <param name="reminder">The object with new values for reminder</param>
        /// <returns>True if operation success else false</returns>
        public async Task<bool> AddOrUpdateReminder(string userId, int newsId, Reminder reminder)
        {
            if (await newsRepository.GetNewsById(userId, newsId) != null)
            {
                return await newsRepository.AddOrUpdateReminder(userId, newsId, reminder);
            }
            else
            {
                throw new NoNewsFoundException($"NewsId {newsId} for {userId} doesn't exist");
            }
        }

        /// <summary>
        /// Method for creating a news
        /// </summary>
        /// <param name="userId">The id of the user who is creating the news</param>
        /// <param name="news">The properties of the news to be added</param>
        /// <returns>The id of the newly created news</returns>
        public async Task<int> CreateNews(string userId, News news)
        {
            if (!await newsRepository.IsNewsExist(userId, news.Title))
            {
                return await newsRepository.CreateNews(userId, news);
            }
            else
            {
                throw new NewsAlreadyExistsException($"{userId} have already added this news");
            }
        }

        /// <summary>
        /// Method for deleting a news
        /// </summary>
        /// <param name="userId">The id of the user of the news</param>
        /// <param name="newsId">The id of the news to be deleted</param>
        /// <returns>True if deletion successful</returns>
        public async Task<bool> DeleteNews(string userId, int newsId)
        {
            var deleted = await newsRepository.DeleteNews(userId, newsId);
            if (deleted)
            {
                return deleted;
            }
            else
            {
                throw new NoNewsFoundException($"NewsId {newsId} for {userId} doesn't exist");
            }
        }

        /// <summary>
        /// Method for deleting reminder of a news
        /// </summary>
        /// <param name="userId">The id of the user of the news</param>
        /// <param name="newsId">The id of the news whose reminder is to be deleted</param>
        /// <returns>True if deletion is successful</returns>
        public async Task<bool> DeleteReminder(string userId, int newsId)
        {
            if (await newsRepository.IsReminderExists(userId, newsId))
            {
                return await newsRepository.DeleteReminder(userId, newsId);
            }
            else
            {
                throw new NoReminderFoundException("No reminder found for this news");
            }
        }

        /// <summary>
        /// Method for finding all news by the user
        /// </summary>
        /// <param name="userId">The id of the user whose news are to be fetched</param>
        /// <returns>The list of news of the user</returns>
        public async Task<List<News>> FindAllNewsByUserId(string userId)
        {
            var newsList = await newsRepository.FindAllNewsByUserId(userId);
            if (newsList != null)
            {
                return newsList;
            }
            else
            {
                throw new NoNewsFoundException($"No news found for {userId}");
            }
        }
    }
}
