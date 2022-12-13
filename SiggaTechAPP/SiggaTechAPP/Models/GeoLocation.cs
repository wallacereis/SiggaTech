using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace SiggaTechAPP.Models
{
    public class GeoLocation
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }
        [ForeignKey(typeof(Address)), NotNull]
        public int AddressId { get; set; }
        [JsonProperty("lat"), NotNull]
        public string Latitude { get; set; }
        [JsonProperty("lng"), NotNull]
        public string Longitude { get; set; }

        [Ignore, OneToOne]
        public Address Address { get; set; }
    }
}
