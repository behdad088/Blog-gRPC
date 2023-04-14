using Blog;
using Grpc.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using static Blog.BlogService;

namespace server
{
    internal class BlogServiceImpl : BlogServiceBase
    {
        private static MongoClient mongoClient = new MongoClient("mongodb://localhost:27017");
        private static IMongoDatabase mongoDatabase = mongoClient.GetDatabase("mydb");
        private static IMongoCollection<BsonDocument> mongoCollection = mongoDatabase.GetCollection<BsonDocument>("blog");

        public override Task<CreaeteBlogResponse> CreateBlog(CreateBlogRequest request, ServerCallContext context)
        {
            var blog = request.Blog;
            BsonDocument doc = new BsonDocument("author_id", blog.AuthorId)
                .Add("title", blog.Title)
                .Add("content", blog.Content);

            mongoCollection.InsertOne(doc);
            var id = doc.GetValue("_id").ToString();
            blog.Id = id;

            return Task.FromResult(new CreaeteBlogResponse
            {
                Blog = blog
            });
        }
    }
}
