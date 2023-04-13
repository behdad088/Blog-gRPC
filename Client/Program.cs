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

            channel.ShutdownAsync().Wait();
            Console.ReadKey();
        }
    }
}
