using System.Collections.Generic;

namespace ChainSafe.Gaming.Lootboxes.Chainlink
{
    public class LootboxRewards
    {
        public static LootboxRewards Empty =>
            new LootboxRewards
            {
                Erc20Rewards = new List<Erc20Reward>(),
                Erc721Rewards = new List<Erc721Reward>(),
                Erc1155Rewards = new List<Erc1155Reward>(),
                Erc1155NftRewards = new List<Erc1155NftReward>(),
            };

        public List<Erc20Reward> Erc20Rewards { get; set; }

        public List<Erc721Reward> Erc721Rewards { get; set; }

        public List<Erc1155Reward> Erc1155Rewards { get; set; }

        public List<Erc1155NftReward> Erc1155NftRewards { get; set; }
    }
}