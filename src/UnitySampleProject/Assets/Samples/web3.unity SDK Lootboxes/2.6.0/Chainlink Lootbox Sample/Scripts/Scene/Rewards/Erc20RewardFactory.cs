using System;
using System.Globalization;
using System.Numerics;
using System.Threading.Tasks;
using LootBoxes.Chainlink.Scene.StageItems;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Lootboxes.Chainlink;
using Scripts.EVM.Token;
using UnityEngine;

namespace LootBoxes.Chainlink.Scene
{
    public class Erc20RewardFactory : MonoBehaviour
    {
        public StageItem CoinRewardItemPrefab;
        public int AmountDigits = 2;

        private IContractBuilder contractBuilder;

        private void OnValidate()
        {
            if (CoinRewardItemPrefab && CoinRewardItemPrefab.Reward is not CoinReward)
            {
                Debug.LogError($"{nameof(CoinRewardItemPrefab.Reward)} is not {nameof(CoinReward)}");
                CoinRewardItemPrefab = null;
            }
        }

        public void Configure(IContractBuilder contractBuilder)
        {
            this.contractBuilder = contractBuilder;
        }

        public async Task<StageItem> Create(Erc20Reward data)
        {
            var item = Instantiate(CoinRewardItemPrefab);
            var reward = (CoinReward)item.Reward;
            var contract = contractBuilder.Build(ABI.Erc20, data.ContractAddress);
            var symbol = (await contract.Call("symbol"))[0].ToString();
            var decimals = BigInteger.Parse((await contract.Call("decimals"))[0].ToString());
            var humanizedAmount = HumanizeAmount(data.AmountRaw, decimals);
            var roundedAmount = Math.Round(humanizedAmount, AmountDigits);

            reward.SymbolLabel.text = symbol;
            reward.Amount.text = roundedAmount.ToString(CultureInfo.InvariantCulture);

            return item;

            float HumanizeAmount(BigInteger amountRaw, BigInteger decimals)
            {
                if (decimals.IsZero) return (float)amountRaw;

                return (float)BigInteger.Divide(amountRaw, decimals);
            }
        }
    }
}