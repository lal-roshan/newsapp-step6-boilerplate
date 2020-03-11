using NewsService.Models;
using NewsService.Repository;
using NewsService.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace NewsService.Services
{
    public class NewsService
    {
        /*
       * NewsRepository should  be injected through constructor injection. 
       * Please note that we should not create NewsRepository object using the new keyword
       */
        readonly INewsRepository repository;
        public NewsService(INewsRepository newsRepository)
        {
            
        }

        /* Implement all the methods of respective interface asynchronously*/

        /* Implement CreateNews method to add the new news details*/

        /* Implement AddOrUpdateReminder using userId and newsId*/

        /* Implement DeleteNews method to remove the existing news*/

        /* Implement DeleteReminder method to delte the Reminder using userId*/

        /* Implement FindAllNewsByUserId to get the News Details by userId*/
    }
}
