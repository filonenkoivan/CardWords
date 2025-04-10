using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CardCollection
    {
        public string? Name { get; set; }
        public List<Card>? CardList { get; set; } = new List<Card>();
        public DateTime CreatedTime { get; set; }
        public int Id { get; set; }

    }
}
