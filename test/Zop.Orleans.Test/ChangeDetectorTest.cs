using KellermanSoftware.CompareNetObjects.TypeComparers;
using System;
using System.Collections.Generic;
using Xunit;
using KellermanSoftware.CompareNetObjects;
using Zop.Domain.Entities;
using System.Linq;
using Zop.Repositories.ChangeDetector;

namespace Zop.Orleans.Test
{
    public class ChangeDetectorTest
    {
        [Fact]
        public void Detector()
        {

            var date = DateTime.Now;
            var user1 = new User(1)
            {
                Password = null,
                Posts = new List<Post>()
                {
                new Post(1)
                {
                    CreatedDate =date,Description=date.Ticks.ToString(),Title="测试1",Author = new Author(1){
                           Title="author"
                       } }
                   ,new Post(2){CreatedDate=date,Description=date.Ticks.ToString(),Title="测试",Author = new Author(2)
                   {
                           Title="author"
                    } }
                    ,new Post(3)
                    {
                        CreatedDate =date,Description=date.Ticks.ToString(),Title="测试",Author = new Author(3){
                           Title=null
                    } }
                    , new Post(4){CreatedDate=date,Description=date.Ticks.ToString(),Title="测试2",Author =null }
                    , new Post(5){CreatedDate=date,Description=date.Ticks.ToString(),Title="测试b",Author =null }
                    , new Post(6){CreatedDate=date,Description=date.Ticks.ToString(),Title="测试b",Author = new Author(3){ } }
                }
            };

            var user2 = user1.Clone<User>();
            user2.Password = "";
            user2.Aaccount = null;
            var post1 = user2.Posts.FirstOrDefault(f => f.Id == 1);
            user2.Posts.Remove(post1);
            var post2 = user2.Posts.FirstOrDefault(f => f.Id == 2);
            post2.Title = "测试修改子级";
            var post3 = user2.Posts.FirstOrDefault(f => f.Id == 3);
            post3.Author.Title = "测试修改子子级";
            var post4 = user2.Posts.FirstOrDefault(f => f.Id == 4);
            post4.Author = new Author(1) { Title = "添加子子级，设置ID" };
            var post5 = user2.Posts.FirstOrDefault(f => f.Id == 5);
            post5.Author = new Author() { Title = "添加子子级，不设置ID" };
            var post6 = user2.Posts.FirstOrDefault(f => f.Id == 6);
            post6.Author = null;
            var post7 = new Post(7) { CreatedDate = date, Description = date.Ticks.ToString(), Title = "测试", Author = null };
            user2.Posts.Add(post7);


            CompareLogic compareLogic = new CompareLogic();
            compareLogic.Config.MaxDifferences = int.MaxValue;
            compareLogic.Config.CompareStaticFields = false;//静态字段不比较
            compareLogic.Config.CompareStaticProperties = false;//静态属性不比较
            compareLogic.Config.Caching = true;
            compareLogic.Config.CustomComparers.Add(new ZopDictionaryComparer(RootComparerFactory.GetRootComparer()));
            compareLogic.Config.CustomComparers.Add(new ZopListComparer(RootComparerFactory.GetRootComparer()));
            compareLogic.Config.CustomComparers.Add(new EntityCollectionComparer(RootComparerFactory.GetRootComparer()));

            var result = compareLogic.Compare(user1, user2);

            ChangeManagerFactory changeManagerFactory = new ChangeManagerFactory();
            IChangeManager changeManager = changeManagerFactory.Create(new EntityChange(user1, user2, 0), result);
            var ch = changeManager.GetChange();
            string json = ch.ToJsonString();

            User user3 = json.ToFromJson<User>();

        }
    }


    [Serializable]
    public class User : Entity<int>
    {
        public User(int id)
        {
            this.Id = id;
        }
        public string Aaccount { get; set; } = "1";
        public string Password { get; set; }
        public List<Post> Posts { get; set; }
    }
    [Serializable]
    public class Post : Entity<int>
    {
        public Post(int id)
        {
            this.Id = id;
        }
        public string Title { set; get; }
        public string Description { set; get; }
        public DateTime CreatedDate { set; get; }
        public Author Author { get; set; }
    }

    [Serializable]

    public class Author : Entity<int>
    {
        public Author()
        {

        }
        public Author(int id)
        {
            this.Id = id;
        }
        public string Title { set; get; }
    }
}
