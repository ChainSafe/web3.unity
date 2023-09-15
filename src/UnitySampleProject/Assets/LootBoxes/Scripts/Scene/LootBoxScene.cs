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
        public Animator Animator;
        public StageItemFactory StageItemFactory;
        public Stage stage;
        public StageCamera stageCamera;
        public StageFocus stageFocus;
        public StageItemPrefabSet prefabSet;
        public Blackout blackout;
        [Header("UI")]
        public SelectLootBoxesUI SelectLootBoxesUI;
        
        private ILootboxService lootBoxService;
        
        public uint ActiveType { get; set; }
        public uint ActiveStageItem { get; set; }
        
        public Task<List<uint>> GetTypes() => lootBoxService.GetLootboxTypes();
        public Task<uint> GetBalance(uint typeId) => lootBoxService.BalanceOf(typeId);

        public void Configure(ILootboxService lootBoxService)
        {
            this.lootBoxService = lootBoxService;
            StageItemFactory = new StageItemFactory();
            
            stage.Configure(StageItemFactory);
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
                Animator.SetTrigger("LaunchEmpty");
            }

            void LaunchSelection()
            {
                Animator.SetTrigger("LaunchSelection");
            }

            void LaunchOpening()
            {
                Animator.SetTrigger("LaunchOpening");
            }
        }
    }
}