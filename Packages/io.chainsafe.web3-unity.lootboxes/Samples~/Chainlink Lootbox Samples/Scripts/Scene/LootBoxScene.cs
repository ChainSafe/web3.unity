using System.Collections.Generic;
using System.Threading.Tasks;
using LootBoxes.Chainlink.Scene.StageItems;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Lootboxes.Chainlink;
using UnityEngine;

namespace LootBoxes.Chainlink.Scene
{
    public class LootBoxScene : MonoBehaviour
    {
        public Animator animator;
        public Stage stage;
        public StageCamera stageCamera;
        public StageFocus stageFocus;
        public LootBoxesFrontEndDataSet frontEndDataSet;
        public Blackout blackout;
        public RewardStageItemSpawner rewardSpawner;
        [Header("UI")]
        public SelectLootBoxesUI selectLootBoxesUI;
        public OpenLootBoxUI openLootBoxUI;
        public PromptClaimRewardsUI promptClaimRewardsUI;
        public ViewRewardsUI viewRewardsUI;
        public EmptyStateUI emptyStateUI;

        private ILootboxService lootBoxService;

        public LootBoxStageItemFactory LootBoxStageItemFactory { get; private set; }
        public uint ActiveType { get; set; }
        public LootboxRewards LastClaimedRewards { get; set; }
        public List<LootboxTypeInfo> LastFetchedLootBoxes { get; set; }

        public void Configure(ILootboxService lootBoxService, IContractBuilder contractBuilder,
            Erc1155MetaDataReader erc1155MetaDataReader)
        {
            this.lootBoxService = lootBoxService;
            LootBoxStageItemFactory = new LootBoxStageItemFactory();

            stageCamera.Configure(stage);
            stageFocus.Configure(stage, stageCamera);
            rewardSpawner.Configure(contractBuilder, erc1155MetaDataReader);
        }

        public void Launch() => animator.SetTrigger("Launch");

        public Task<List<uint>> GetTypes() => lootBoxService.GetLootboxTypes();
        public Task<uint> GetBalance(uint typeId) => lootBoxService.BalanceOf(typeId);
        public Task<bool> CanClaimRewards() => lootBoxService.CanClaimRewards();
        public Task<LootboxRewards> ClaimRewards() => lootBoxService.ClaimRewards();
        public Task<bool> IsOpeningLootBox() => lootBoxService.IsOpeningLootbox();
        public Task<List<LootboxTypeInfo>> FetchAllLootBoxes() => lootBoxService.FetchAllLootboxes();
        public Task<uint> OpeningLootBoxType() => lootBoxService.OpeningLootboxType();
        public Task OpenLootBoxes(uint lootBoxType, uint count) => lootBoxService.OpenLootbox(lootBoxType, count);
    }
}