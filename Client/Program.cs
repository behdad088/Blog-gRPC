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

            try
            {
                //await CreateBlogAsync(client);
                await ReadBlogAsync(client);
            }
            catch (RpcException e)
            {
                Console.WriteLine(e.Status.Detail);
            }

            channel.ShutdownAsync().Wait();
            Console.ReadKey();
        }

        private static async Task CreateBlogAsync(BlogService.BlogServiceClient client)
        {
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

            Console.WriteLine($"Blog with Id: {response.Blog.Id} was created!");

        }

        private static async Task ReadBlogAsync(BlogService.BlogServiceClient client)
        {
            var readBlog = new ReadBlogRequest
            {
                BlogId = "6438e4eb5274266f6e292e6f"
            };

            var response = await client.ReadBlogAsync(readBlog);
            Console.WriteLine(response.Blog.ToString());
        }
    }
}
