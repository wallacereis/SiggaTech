using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace SiggaTechAPP.Models
{
    public class Company
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }
        [ForeignKey(typeof(User)), NotNull]
        public int UserId { get; set; }
        [JsonProperty("name"), NotNull]
        public string Name { get; set; }
        [JsonProperty("catchPhrase"), NotNull]
        public string CatchPhrase { get; set; }
        [JsonProperty("bs"), NotNull]
        public string Bs { get; set; }

        [Ignore,OneToOne]
        public User User { get; set; }  
    }
}
