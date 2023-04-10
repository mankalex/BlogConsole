using NLog;
using System.Linq;

// See https://aka.ms/new-console-template for more information
string path = Directory.GetCurrentDirectory() + "\\nlog.config";

// create instance of Logger
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
logger.Info("Program started");

try
{
    Console.WriteLine("1) Display All Blogs");
    Console.WriteLine("2) Add Blog");
    Console.WriteLine("3) Create Post");
    Console.WriteLine("4) Display All Posts");
    Console.WriteLine("Enter to quit");
    //Prompt user for input
    string choice = Console.ReadLine();

    if(choice == "1")
    {
        // Display all Blogs from the database
        var db = new BloggingContext();
        var query = db.Blogs.OrderBy(b => b.BlogId);

        Console.WriteLine("All blogs in the database:");
        foreach (var item in query)
        {
            Console.WriteLine(item.BlogId + ") " + item.Name);
        }
    }
    else if(choice == "2")
    {
        // Create and save a new Blog
        Console.Write("Enter a name for a new Blog: ");
        var name = Console.ReadLine();

        var blog = new Blog { Name = name };

        var db = new BloggingContext();
        db.AddBlog(blog);
        logger.Info("Blog added - {name}", name);
    }
    else if(choice == "3")
    {
        // Display all Blogs from the database
        var db = new BloggingContext();
        var query = db.Blogs.OrderBy(b => b.BlogId);

        Console.WriteLine("All blogs in the database:");
        foreach (var item in query)
        {
            Console.WriteLine(item.BlogId + ") " + item.Name);
        }

        //user input
        Console.WriteLine("Enter blog you are looking to post on: ");
        string input = Console.ReadLine();
        int blogid;
        //test if user input is valid
        if(Int32.TryParse(input, out blogid))
        {

            Console.WriteLine("Etner the Title of this post: ");
            var ptitle = Console.ReadLine();
            if(ptitle == null || ptitle == " " || ptitle == "")
            {
                logger.Error("Title is null");
            }

            Console.WriteLine("Enter the Content of this post ");
            var pcontent = Console.ReadLine();

            var post = new Post { Title = ptitle, BlogId = blogid, Content = pcontent};

            db.AddPost(post);
            logger.Info("post added - {ptitle}",ptitle);
        }
        else{
            logger.Error("Invalid input for blog id");
        }
    }
    else if(choice == "4")
    {
        // Display all Posts from the database
        var db = new BloggingContext();
        var query = db.Blogs.OrderBy(b => b.BlogId);
        var blogCount = db.Blogs.Count();

        Console.WriteLine("Select the blog's posts to display:");
        Console.WriteLine("0) Posts from all blogs");
        foreach (var item in query)
        {
            Console.WriteLine(item.BlogId + ") Posts from " + item.Name);
        }
        string input = Console.ReadLine();
        int blogid;
        //tests user input is valid
        if(Int32.TryParse(input, out blogid))
        {
            //if user input get all posts
            if(blogid == 0)
            {
                var pquery = db.Posts.OrderBy(x => x.PostId);
                foreach(var item in pquery)
                {
                    Console.WriteLine("BlogId: " + item.BlogId);
                    Console.WriteLine("Title: " + item.Title);
                    Console.WriteLine("Content: " + item.Content);
                }
            }
            //if user input any specific blog id posts
            
            else if(blogid >=1 && blogid <=blogCount)
            {
                var pquery = db.Posts.Where(x => x.BlogId == blogid);
                foreach(var item in pquery)
                {
                    Console.WriteLine("BlogId: " + item.BlogId);
                    Console.WriteLine("Title: " + item.Title);
                    Console.WriteLine("Content: " + item.Content);
                }

            }
            else
            {
                logger.Error("blog id does not exist");
            }
            
        }
        else{
            logger.Error("Invalid input for blog id");
        }

    }
    
}
catch (Exception ex)
{
    logger.Error(ex.Message);
}

logger.Info("Program ended");
