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
                //await ReadBlogAsync(client);
                //await UpdateBlogAsync(client);
                //await DeleteBlogAsync(client);
                await ListBlogAsync(client);
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

        private static async Task UpdateBlogAsync(BlogService.BlogServiceClient client)
        {
            var updateBlog = new UpdateBlogRequest
            {
                Blog = new Blog.Blog
                {
                    Id = "6438ed08f21c29c3e2336e5b",
                    AuthorId = "Behdad-test",
                    Content = "This content is updated",
                    Title = "Updated"
                }
            };

            var response = await client.UpdateBlogAsync(updateBlog);
            Console.WriteLine(response.Blog.ToString());
        }

        private static async Task DeleteBlogAsync(BlogService.BlogServiceClient client)
        {
            var deleteRequest = new DeleteBlogRequest
            {
                BlogId = "6438ecb8dd00bd054551678e"
            };

            var response = await client.DeleteBlogAsync(deleteRequest);
            Console.WriteLine($"Blog with id {response.BlogId} was deleted.");
        }

        private static async Task ListBlogAsync(BlogService.BlogServiceClient client)
        {
            var response = client.ListBlog(new ListBlogRequest());

            while (await response.ResponseStream.MoveNext())
            {
                Console.WriteLine(response.ResponseStream.Current.Blog.ToString());
            }
        }
    }
}
