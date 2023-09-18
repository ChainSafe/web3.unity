using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.Chainlink.Lootboxes;
using LootBoxes.Scene.StageItems;
using UnityEngine;

namespace LootBoxes.Scene
{
    public class LootBoxScene : MonoBehaviour
    {
        public Animator animator;
        public Stage stage;
        public StageCamera stageCamera;
        public StageFocus stageFocus;
        public StageItemPrefabSet prefabSet;
        public Blackout blackout;
        [Header("UI")]
        public SelectLootBoxesUI selectLootBoxesUI;
        public PromptClaimRewardsUI promptClaimRewardsUI;
        
        private StageItemFactory stageItemFactory;
        private ILootboxService lootBoxService;
        
        public uint ActiveType { get; set; }
        public LootboxRewards LastClaimedRewards { get; set; }

        public void Configure(ILootboxService lootBoxService)
        {
            this.lootBoxService = lootBoxService;
            stageItemFactory = new StageItemFactory();
            
            stage.Configure(stageItemFactory);
            stageCamera.Configure(stage);
            stageFocus.Configure(stage, stageCamera);
        }

        public void Launch() => animator.SetTrigger("Launch");

        public Task<List<uint>> GetTypes() => lootBoxService.GetLootboxTypes();
        public Task<uint> GetBalance(uint typeId) => lootBoxService.BalanceOf(typeId);
        public Task<bool> CanClaimRewards() => lootBoxService.CanClaimRewards();
        public Task<LootboxRewards> ClaimRewards() => lootBoxService.ClaimRewards();
        public Task<bool> IsOpeningLootBox() => lootBoxService.IsOpeningLootbox();
        public Task<List<LootboxTypeInfo>> FetchAllLootBoxes() => lootBoxService.FetchAllLootboxes();
        public Task<uint> OpeningLootBoxType() => lootBoxService.OpeningLootboxType();
    }
}