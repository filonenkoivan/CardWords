using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CardCollection
    {
        public CardCollection(string name)
        {
            Name = name;
        }
        public string? Name { get; set; }
        public List<Card>? CardList { get; set; } = new List<Card>();
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        public int Id { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
        public int UserId { get; set; }
        public int? PreviousId { get; set; }

    }
}
