using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SiggaTechAPP.Models
{
    public class User
    {
        [JsonProperty("id"), PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }
        [JsonProperty("name"), NotNull]
        public string Name { get; set; }
        [JsonProperty("username"), NotNull]
        public string UserName { get; set; }
        [JsonProperty("email"), NotNull]
        public string Email { get; set; }
        [JsonProperty("phone"), NotNull]
        public string Phone { get; set; }
        [JsonProperty("website"), NotNull]
        public string WebSite { get; set; }
        
        [JsonProperty("address"), Ignore, OneToOne]
        public Address Address { get; set; }
        [JsonProperty("company"), Ignore, OneToOne] 
        public Company Company { get; set; }
        
        [Ignore, OneToMany]
        public List<Post> Posts { get; set; }
        public User()
        {
            Posts = new List<Post>();
        }

        [Ignore]
        public bool IsOnline { get; set; }
        [Ignore]
        public ImageSource ImageSource => IsOnline ? "user.png" : "avatar.png";
    }
}
