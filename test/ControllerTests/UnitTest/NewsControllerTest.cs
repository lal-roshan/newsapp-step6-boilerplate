﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NewsService.Controllers;
using NewsService.Exceptions;
using NewsService.Models;
using NewsService.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Test.ControllerTests.UnitTest
{
    public class NewsControllerTest
    {
        [Fact]
        public async Task GetShouldReturnOk()
        {
            string userId = "Jack";
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("userId", userId)
            }, "mock"));

            var mockService = new Mock<INewsService>();
            mockService.Setup(svc => svc.FindAllNewsByUserId(userId)).Returns(Task.FromResult(newsList));
            var controller = new NewsController(mockService.Object);
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

            var actual = await controller.Get();
            var actionResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsAssignableFrom<List<News>>(actionResult.Value);
        }


        [Fact]
        public async Task PostShouldReturnCreated()
        {
            string userId = "Jack";
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("userId", userId)
            }, "mock"));

            int newsId = 102;
            News news = new News { Title = "chandrayaan2-spacecraft", Content = "The Lander of Chandrayaan-2 was named Vikram after Dr Vikram A Sarabhai", PublishedAt = DateTime.Now, UrlToImage = null, Url = null };
            var mockService = new Mock<INewsService>();
            mockService.Setup(svc => svc.CreateNews(userId, news)).Returns(Task.FromResult(newsId));
            var controller = new NewsController(mockService.Object);
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

            var actual = await controller.Post(news);
            var actionResult = Assert.IsType<CreatedResult>(actual);
            var actionValue = Assert.IsAssignableFrom<int>(actionResult.Value);
            Assert.Equal(newsId, actionValue);
        }

        [Fact]
        public async Task DeleteShouldReturnOk()
        {
            string userId = "Jack";
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
               {
                    new Claim("userId", userId)
               }, "mock"));
            int newsId = 102;
            var mockService = new Mock<INewsService>();
            mockService.Setup(svc => svc.DeleteNews(userId, newsId)).Returns(Task.FromResult(true));
            var controller = new NewsController(mockService.Object);
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

            var actual = await controller.Delete(newsId);
            var actionResult = Assert.IsType<OkObjectResult>(actual);
            Assert.True(Convert.ToBoolean(actionResult.Value));
        }

        [Fact]
        public async Task GetShouldReturnNotFound()
        {
            string userId = "John";
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
             {
                new Claim("userId", userId)
             }, "mock"));
            var mockService = new Mock<INewsService>();
            mockService.Setup(svc => svc.FindAllNewsByUserId(userId)).Throws(new NoNewsFoundException($"No news found for {userId}"));
            var controller = new NewsController(mockService.Object);
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

            var actual = await controller.Get();
            var actionResult = Assert.IsType<NotFoundObjectResult>(actual);
            Assert.Equal($"No news found for {userId}", actionResult.Value);
        }

        [Fact]
        public async Task PostShouldReturnConflict()
        {
            string userId = "Jack";
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("userId", userId)
            }, "mock"));
            News news = new News { Title = "2020 FIFA U-17 Women World Cup", Content = "The tournament will be held in India between 2 and 21 November 2020", PublishedAt = DateTime.Now, UrlToImage = null, Url = null };
            var mockService = new Mock<INewsService>();
            mockService.Setup(svc => svc.CreateNews(userId, news)).Throws(new NewsAlreadyExistsException($"{userId} have already added this news"));
            var controller = new NewsController(mockService.Object);
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

            var actual = await controller.Post(news);
            var actionResult = Assert.IsType<ConflictObjectResult>(actual);
            Assert.Equal($"{userId} have already added this news", actionResult.Value);
        }

        [Fact]
        public async Task DeleteShouldReturnNotFound()
        {
            string userId = "Jack";
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("userId", userId)
            }, "mock"));
            int newsId = 103;
            var mockService = new Mock<INewsService>();
            mockService.Setup(svc => svc.DeleteNews(userId, newsId)).Throws(new NoNewsFoundException($"NewsId {newsId} for {userId} doesn't exist"));
            var controller = new NewsController(mockService.Object);
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

            var actual = await controller.Delete(newsId);
            var actionResult = Assert.IsType<NotFoundObjectResult>(actual);
            Assert.Equal($"NewsId {newsId} for {userId} doesn't exist", actionResult.Value);
        }

        List<News> newsList = new List<News> {
        new News { NewsId = 101, Title = "IT industry in 2020", Content = "It is expected to have positive growth in 2020.", PublishedAt = DateTime.Now, UrlToImage = null,Url=null },
        new News { NewsId = 102, Title = "2020 FIFA U-17 Women World Cup", Content = "The tournament will be held in India between 2 and 21 November 2020", PublishedAt = DateTime.Now, UrlToImage = null, Url=null }
        };

    }
}
