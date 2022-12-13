using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace SiggaTechAPP.Models
{
    public class Address
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }
        [ForeignKey(typeof(User)), NotNull]
        public int UserId { get; set; }
        [JsonProperty("street"), NotNull]
        public string Street { get; set; }
        [JsonProperty("suite"), NotNull]
        public string Suite { get; set; }
        [JsonProperty("city"), NotNull]
        public string City { get; set; }
        [JsonProperty("zipcode"), NotNull]
        public string ZipCode { get; set; }
        
        [Ignore, OneToOne]
        public User User { get; set; }
        [JsonProperty("geo"), Ignore, OneToOne]
        public GeoLocation GeoLocation { get; set; }
    }
}
