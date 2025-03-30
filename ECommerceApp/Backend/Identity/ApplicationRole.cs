using AspNetCore.Identity.Mongo.Model;

namespace Backend.Identity
{
    public class ApplicationRole : MongoRole
    {
        // Optional: add role-level metadata
        public string Description { get; set; }
    }
}