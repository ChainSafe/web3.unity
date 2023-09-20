# VRF Lootbox - Unity

### Install

1. Clone [ChainSafe/web3.unity](https://github.com/ChainSafe/web3.unity/tree/oleksandr/lootboxes).
2. Checkout branch 'oleksandr/lootboxes'.
3. Run 'web3.unity\src\ChainSafe.GamingSdk.EVM.Unity\publish-to-unity-package.bat' to build Unity packages.
4. Clone [ChainSafe/vrf-lootbox-contracts](https://github.com/ChainSafe/vrf-lootbox-contracts).
5. Follow instructions for 'vrf-lootbox-contracts' repo (install, run node, run hardhat devsetup script).

### Usage

1. Open 'web3.unity\src\UnitySampleProject' in Unity Hub.
2. Ensure hardhat is up and running and 'devsetup' script executed successfully.
3. Open 'Assets\LootBoxes\LootBoxes.unity' scene in UnityEditor.
4. Hit Play.
5. Enjoy!

### Opening lootboxes

After you've sent an open request from Unity scene, you have to manually
open those lootboxes on the hardhat side. You can do this by calling:
```shell
npm run hardhat -- fulfill
```

### Keyboard

- You can use WASD/Arrows to move around.
- Space to Select/Deselect
- Enter/Return to open selected.