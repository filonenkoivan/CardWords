using Application.Interfaces;
using Domain.Enum;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserStatsService
    {
        public (int, RankExp) GetNextRank(int exp)
        {
            int expForNextRank = 0;
            RankExp nextRank = RankExp.Beginner;

            switch (exp)
            {
                case >= 3000:
                    nextRank = RankExp.FluencyHero;
                    break;
                case >= 1500:
                    expForNextRank = 3000 - exp;
                    nextRank = RankExp.GrammarGuardian;
                    break;
                case >= 500:
                    expForNextRank = 1500 - exp;
                    nextRank = RankExp.WordMaster;
                    break;
                case < 500:
                    expForNextRank = 500 - exp;
                    nextRank = RankExp.Explorer;
                    break;

            }
            return (expForNextRank, nextRank);
        }
        public RankExp GetCurrentRank(int exp)
        {
            RankExp currentRank = RankExp.Beginner;

            switch (exp)
            {
                case >= 5000:
                    currentRank = RankExp.FluencyHero;
                    break;
                case >= 3000:
                    currentRank = RankExp.GrammarGuardian;
                    break;
                case >= 1500:
                    currentRank = RankExp.WordMaster;
                    break;
                case >= 500:
                    currentRank = RankExp.Explorer;
                    break;
                case < 500:
                    currentRank = RankExp.Beginner;
                    break;

            }
            return currentRank;
        }
        public async Task<UserStats> StatsUpdate(bool isAdd, CancellationToken cancellationToken, UserStats stats, int words = 1)
        {
            if (isAdd)
            {
                stats.WordsAdded++;
                stats.Experience += UserStats.WordAddedExp;
                (int, RankExp) nextRank = GetNextRank(stats.Experience);
                stats.NextRankExp = nextRank.Item1;
            }
            else
            {
                stats.WordsLearned += words;
                stats.Experience += UserStats.WordLearnedExp * words;
                (int, RankExp) nextRank = GetNextRank(stats.Experience);
                stats.NextRankExp = nextRank.Item1;
            }

            if (stats.Experience >= (int)stats.Rank && stats.Experience >= 500)
            {

                (int, RankExp) nextRank = GetNextRank(stats.Experience);
                stats.NextRank = nextRank.Item2;
                stats.Rank = GetCurrentRank(stats.Experience);
            }

            return stats;
        }

    }
}
