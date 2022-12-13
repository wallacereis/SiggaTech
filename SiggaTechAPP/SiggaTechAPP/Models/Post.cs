using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace SiggaTechAPP.Models
{
    public class Post
    {
        [JsonProperty("id"), PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }
        [JsonProperty("userId"), ForeignKey(typeof(User)), NotNull]
        public int UserId { get; set; }
        [JsonProperty("title"), NotNull]
        public string Title { get; set; }
        [JsonProperty("body"), NotNull]
        public string Body { get; set; }

        [Ignore, ManyToOne]
        public User User { get; set; }
        [Ignore]
        public string PostTitle => Id + " - " + Title;
    }
}
