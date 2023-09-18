using System.Collections.Generic;
using System.Linq;
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

        public async void Launch()
        {
            if (await lootBoxService.IsOpeningLootbox())
            {
                ActiveType = await lootBoxService.OpeningLootboxType();
                LaunchOpening();
                return;
            }
            
            var lootBoxTypes = await lootBoxService.FetchAllLootboxes();
            if (!lootBoxTypes.Any(info => info.Amount > 0))
            {
                LaunchEmpty();
                return;
            }
            
            ActiveType = lootBoxTypes.First(info => info.Amount > 0).TypeId;
            LaunchSelection();

            void LaunchEmpty()
            {
                animator.SetTrigger("LaunchEmpty");
            }

            void LaunchSelection()
            {
                animator.SetTrigger("LaunchSelection");
            }

            void LaunchOpening()
            {
                animator.SetTrigger("LaunchOpening");
            }
        }
        
        public Task<List<uint>> GetTypes() => lootBoxService.GetLootboxTypes();
        public Task<uint> GetBalance(uint typeId) => lootBoxService.BalanceOf(typeId);
        public Task<bool> CanClaimRewards() => lootBoxService.CanClaimRewards();
        public Task<LootboxRewards> ClaimRewards() => lootBoxService.ClaimRewards();
    }
}