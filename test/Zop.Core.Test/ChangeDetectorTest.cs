using System;
using System.Collections.Generic;
using Xunit;
using Zop.Repositories.ChangeDetector;

namespace Zop.Core.Test
{
    public class ChangeDetectorTest
    {
        [Fact]
        public void Detector()
        {

            var date = DateTime.Now;
            var user1 = new User
            {
                Id = 1,
                Password = null,
                Posts = new List<Post>()
                    {
                       new Post(){CreatedDate=date,Description=date.Ticks.ToString(),Title="≤‚ ‘1", Id=1,IsTransient=false,Author = new Author(){
                           Title=null
                       } },
                          new Post(){CreatedDate=date,Description=date.Ticks.ToString(),Title="≤‚ ‘",Id=2,IsTransient=false,Author = new Author(){
                           Title="author"
                       } },
                             new Post(){CreatedDate=date,Description=date.Ticks.ToString(),Title="≤‚ ‘",Id=3,IsTransient=false,Author = new Author(){
                           Title="author"
                       } },
                                new Post(){CreatedDate=date,Description=date.Ticks.ToString(),Title="≤‚ ‘2",Id=4,IsTransient=false,Author = new Author(){
                           Title="author"
                       } }, new Post(){CreatedDate=date,Description=date.Ticks.ToString(),Title="≤‚ ‘b",Id=5,IsTransient=false,Author = new Author(){
                           Title="author"
                       } },new Post(){CreatedDate=date,Description=date.Ticks.ToString(),Title="≤‚ ‘",Id=6,IsTransient=false,Author = new Author(){
                           Title="author"
                       } },
                    }
            };
            var user2 = new User
            {
                Id = 1,
                Password = "123",
                Posts = new List<Post>()
                    {
                       new Post(){CreatedDate=date,Description=date.Ticks.ToString(),Title="≤‚ ‘",Id=1,IsTransient=false,Author = new Author(){
                           Title="author"
                       } },
                          new Post(){CreatedDate=date,Description=date.Ticks.ToString(),Title="≤‚ ‘",Id=2,IsTransient=false,Author = new Author(){
                           Title="author"
                       } },
                             new Post(){CreatedDate=date,Description=date.Ticks.ToString(),Title="≤‚ ‘",Id=3,IsTransient=false,Author = new Author(){
                           Title="author"
                       } },
                                new Post(){CreatedDate=date,Description=date.Ticks.ToString(),Title="≤‚ ‘",Id=4,IsTransient=false,Author = new Author(){
                           Title="author"
                       } },
                         new Post(){CreatedDate=date,Description=date.Ticks.ToString(),Title="≤‚ ‘",Author = new Author(){
                           Title="author2"
                       } }
                    }
            };
        



            IChangeManager manager = new ChangeManager();
            IChangeManagerFactory managerFactory = new ChangeManagerFactory(manager);
            IChangeDetector detector = new ChangeDetector(managerFactory);
            manager = detector.DetectChanges(user2, user1);
        }
    }



    public partial class User
    {
        public int Id { get; set; }
        public string Aaccount { get; private set; } = "1";
        public string Password { get; set; }
        public List<Post> Posts { get; set; }

    }

    public class Post
    {
        public int Id { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }
        public DateTime CreatedDate { set; get; }
        public bool IsTransient { get; set; } = true;
        public Author Author { get; set; }
    }

    public class Author
    {
        public int Id { set; get; }
        public string Title { set; get; }
    }
}
