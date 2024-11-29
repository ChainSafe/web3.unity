using System.Collections.Generic;

namespace ChainSafe.Gaming.Lootboxes.Chainlink
{
    public class RewardResponse
    {
        public RewardResponse(List<RewardView> rewards, List<ExtraRewardInfo> extraRewards)
        {
            Rewards = rewards;
            ExtraRewards = extraRewards;
        }

        public List<RewardView> Rewards { get; set; }

        public List<ExtraRewardInfo> ExtraRewards { get; set; }
    }
}