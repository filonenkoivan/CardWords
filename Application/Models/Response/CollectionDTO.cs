using Domain.Models;

namespace Api.Models.DTOs
{
    public class CollectionDTO
    {
        public CollectionDTO(string name, List<Card> cardList, DateTime createdTime, int id)
        {
            Name = name;
            CardList = cardList;
            CreatedTime = createdTime.ToString("F");
            Id = id;
        }
        public string? Name { get; set; }
        public List<Card>? CardList { get; set; } = new List<Card>();
        public string CreatedTime { get; set; }

        public int Id { get; set; }
    }
}
