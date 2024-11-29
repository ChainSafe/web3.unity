using System.Collections.Generic;

namespace ChainSafe.Gaming.Lootboxes.Chainlink
{
    public class ExtraRewardInfo
    {
        public ExtraRewardInfo(uint id, uint units, uint amountPerUnit, uint balance)
        {
            Id = id;
            Units = units;
            AmountPerUnit = amountPerUnit;
            Balance = balance;
        }

        public uint Id { get; set; }

        public uint Units { get; set; }

        public uint AmountPerUnit { get; set; }

        public uint Balance { get; set; }
    }

    public class RewardView
    {
        public RewardView(string rewardToken, uint rewardType, uint units, uint amountPerUnit, uint balance, List<ExtraRewardInfo> extra)
        {
            RewardToken = rewardToken;
            RewardType = rewardType;
            Units = units;
            AmountPerUnit = amountPerUnit;
            Balance = balance;
            Extra = extra ?? new List<ExtraRewardInfo>();
        }

        public string RewardToken { get; set; }

        public uint RewardType { get; set; }

        public uint Units { get; set; }

        public uint AmountPerUnit { get; set; }

        public uint Balance { get; set; }

        public List<ExtraRewardInfo> Extra { get; set; }
    }
}