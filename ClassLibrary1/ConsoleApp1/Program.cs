using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using BlogDataLibrary.Data;
using BlogDataLibrary.Database;
using BlogDataLibrary.Models;



namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SqlData db = GetConnection();

            bool exit = false;

            while (!exit)
            {
                ShowMenu();
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();
                Console.WriteLine();
                switch (choice)
                {
                    case "1":
                        Authenticate(db);
                        Console.WriteLine();
                        break;
                    case "2":
                        Register(db);
                        Console.WriteLine();
                        break;
                    case "3":
                        AddPost(db);
                        Console.WriteLine();
                        break;
                    case "4":
                        ListPosts(db);
                        Console.WriteLine();
                        break;
                    case "5":
                        ShowPostDetails(db);
                        Console.WriteLine();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
                
            }
            
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();

        }

        private static UserModel GetCurrentUser(SqlData db)
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            UserModel user = db.Authenticate(username, password);
            return user;
        }

        public static void Authenticate(SqlData db)
        {
            UserModel user = GetCurrentUser(db);

            if (user == null)
            {
                Console.WriteLine("Invalid Credentials");
            } else {
                Console.WriteLine($"Welcome, {user.UserName}");
            }
        }
        static SqlData GetConnection()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

            IConfiguration configuration = builder.Build();
            ISqlDataAccess dbAccess = new SqlDataAccess(configuration);
            SqlData db = new SqlData(dbAccess);

            return db;
        }

        public static void Register(SqlData db)
        {
            Console.Write("Enter new username: ");
            var username = Console.ReadLine();

            Console.Write("Enter new password: ");
            var password = Console.ReadLine();

            Console.Write("Enter first name: ");
            var firstName = Console.ReadLine();

            Console.Write("Enter lastname: ");
            var lastName = Console.ReadLine();

            db.Register(username, firstName, lastName, password);
        }

        public static void AddPost(SqlData db)
        {
            UserModel user = GetCurrentUser(db);

            if (user != null)
            {

                Console.Write("Title: ");
                string title = Console.ReadLine();

                Console.Write("Write body: ");
                string body = Console.ReadLine();

                PostModel post = new PostModel
                {
                    Title = title,
                    Body = body,
                    DateCreated = DateTime.Now,
                    UserId = user.Id,
                };

                db.AddPost(post);
            } else
            {
                Console.WriteLine("Invalid Credentials!");
            }
        }
        private static void ListPosts(SqlData db)
        {
            List<ListPostModel> posts = db.ListPosts();

            foreach (ListPostModel post in posts)
            {
                Console.WriteLine($"{post.Id}. Title: {post.Title} by {post.UserName} [{post.DateCreated.ToString("yyyy-MM-dd")}]");
                Console.WriteLine($"{post.Body.Substring(0, 20)}...");
                Console.WriteLine();
            }
        }

        private static void ShowPostDetails(SqlData db)
        {
            Console.Write("Enter a post ID: ");
            int id = Int32.Parse(Console.ReadLine());

            ListPostModel post = db.ShowPostDetails(id);
            Console.WriteLine(post.Title);
            Console.WriteLine($"by {post.FirstName} {post.LastName} [{post.UserName}]");
            Console.WriteLine();
            Console.WriteLine(post.Body);
            Console.WriteLine(post.DateCreated.ToString("yyyy-MM-dd"));
        }

        static void ShowMenu()
        {
            Console.WriteLine("=== Main Menu ===");
            Console.WriteLine("1. Authenticate");
            Console.WriteLine("2. Register");
            Console.WriteLine("3. Add Post");
            Console.WriteLine("4. List Posts");
            Console.WriteLine("5. Show Post Details");
            Console.WriteLine("0. Exit");
            Console.WriteLine();
        }
    }
}
