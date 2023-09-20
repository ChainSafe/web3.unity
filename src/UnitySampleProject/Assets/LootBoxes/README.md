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

### Adding lootboxes of different type

By default 'devsetup' script will only add lootboxes of one type.
In 'vrf-lootbox-contracts' project add this to file 'hardhat.config.js', task('devsetup'..) to
add more lootboxes of different types:
```ts
await hre.run('mint', { tokenid: 2, amount: 4 });
await hre.run('mint', { tokenid: 3, amount: 3 });
await hre.run('mint', { tokenid: 4, amount: 2 });
await hre.run('mint', { tokenid: 5, amount: 1 });
```