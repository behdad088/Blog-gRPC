using Blog;
using Grpc.Core;

namespace Client
{
    internal class Program
    {
        const string target = "localhost:50052";
        static async Task Main(string[] args)
        {
            var channel = new Channel(target, ChannelCredentials.Insecure);

            await channel.ConnectAsync().ContinueWith((t) =>
            {
                if (t.Status == TaskStatus.RanToCompletion)
                {
                    Console.WriteLine("The client connected successfully.");
                }
            });

            var client = new BlogService.BlogServiceClient(channel);

            var createBlog = new CreateBlogRequest
            {
                Blog = new Blog.Blog
                {
                    AuthorId = "Behdad",
                    Title = "New Blog",
                    Content = "Hello World, this is a new blog"
                }
            };

            var response = await client.CreateBlogAsync(createBlog);

            Console.WriteLine($"Blow with Id: {response.Blog.Id} was created!");

            channel.ShutdownAsync().Wait();
            Console.ReadKey();
        }
    }
}
