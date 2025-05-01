using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Card
    {
        public string? FrontSideText { get; set; }
        public string? BackSideText { get; set; }
        public CardPriority Priority { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? Decsription { get; set; }
        public int Id { get; set; }
        public DateTime? ExpiresTime { get; set; }

        [JsonIgnore]
        public CardCollection? CardCollection { get; set; }
        public int CardCollectionId { get; set; }
    }
}
