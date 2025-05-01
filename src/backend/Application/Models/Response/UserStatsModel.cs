using Domain.Enum;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Models.Response
{
    public class UserStatsModel
    {
        public const int WordLearnedExp = 20;
        public const int WordAddedExp = 2;
        public int WordsLearned { get; set; } = 0;
        public int WordsStreak { get; set; } = 0;
        public int WordsAdded { get; set; } = 0;
        public DateTime FirstTodayWordAddedTime { get; set; } = DateTime.UtcNow;

        public int Experience { get; set; } = 0;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RankExp Rank { get; set; } = RankExp.Beginner;

        public int NextRankExp { get; set; } = 500;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RankExp NextRank { get; set; } = RankExp.Explorer;
    }
}
