using Domain.Enum;

namespace Api.Models
{
    public class CardRequest
    {
        public string? FrontSideText { get; set; }
        public string? BackSideText { get; set; }
        public CardPriority Priority { get; set; }
        public string? Decsription { get; set; }
    }

}

