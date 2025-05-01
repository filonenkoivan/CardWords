using Domain.Models;

namespace Api.Models.DTOs
{
    public class CollectionModel
    {
        public string? Name { get; set; }
        public List<Card>? CardList { get; set; } = new List<Card>();
        public string CreatedTime { get; set; }

        public int Id { get; set; }

    }
    public static class CollectionModelExtensions
    {
        public static CollectionModel ToModel(this CardCollection card)
        {
            return new CollectionModel
            {
                Name = card.Name,
                CardList = card.CardList,
                CreatedTime = card.CreatedTime.ToString("F"),
                Id = card.Id

            };
        }
    }
}
