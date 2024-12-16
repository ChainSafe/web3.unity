using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace ChainSafe.Gaming.Lootboxes.Chainlink
{
    public interface ILootboxService
    {
        /// <summary>
        /// Event invoked when rewards have been claimed.
        /// </summary>
        event Action<LootboxRewards> OnRewardsClaimed;

        /// <summary>
        /// This method returns all lootbox type ids registered in the smart-contract.
        /// Lootbox type id also represents the number of rewards, that can be
        /// claimed by user when he opens the lootbox.
        /// </summary>
        /// <returns>Lootbox type ids registered in the smart-contract.</returns>
        Task<List<int>> GetLootboxTypes();

        /// <summary>
        /// This method returns the balance of lootboxes for the current user.
        /// </summary>
        /// <param name="lootboxType">Lootbox type id.</param>
        /// <returns>The balance of lootboxes for the current user.</returns>
        /// <exception cref="Web3Exception">No signer was registered when building Web3. Can't get current user's address.</exception>
        Task<int> BalanceOf(int lootboxType);

        /// <summary>
        /// This method returns the balance of lootboxes for the specified user.
        /// </summary>
        /// <param name="account">User's public address.</param>
        /// <param name="lootboxType">Lootbox type id.</param>
        /// <returns>The balance of lootboxes for the specified user.</returns>
        Task<int> BalanceOf(string account, int lootboxType);

        /// <summary>
        /// Calculates open price for the player.
        /// </summary>
        /// <param name="lootboxType">Lootbox type id.</param>
        /// <param name="lootboxCount">Number of lootboxes to open.</param>
        /// <returns>Price in network's default currency.</returns>
        Task<BigInteger> CalculateOpenPrice(int lootboxType, int lootboxCount);

        /// <summary>
        /// Checks if a lootbox opening operation is currently in progress.
        /// </summary>
        /// <returns>True if a lootbox is being opened, otherwise false.</returns>
        Task<bool> IsOpeningLootbox();

        /// <summary>
        /// Gets the type id of the lootbox that is currently being opened.
        /// </summary>
        /// <returns>Type id of the lootbox being opened.</returns>
        Task<int> OpeningLootboxType();

        /// <summary>
        /// Initiates the process to open a lootbox of a specific type.
        /// </summary>
        /// <param name="lootboxType">Lootbox type id to open.</param>
        /// <param name="lootboxCount">Optional parameter indicating the number of lootboxes to open. Default is 1.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task OpenLootbox(int lootboxType, int lootboxCount = 1);

        /// <summary>
        /// Recovers lootboxes if an open has failed.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task RecoverLootboxes();

        /// <summary>
        /// Gets all possible items listed in the lootbox.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<LootboxItemList> GetInventory();

        /// <summary>
        /// Checks if the current user can claim rewards.
        /// </summary>
        /// <returns>True if rewards can be claimed by the current user, otherwise false.</returns>
        Task<bool> CanClaimRewards();

        /// <summary>
        /// Checks if a specified user can claim rewards.
        /// </summary>
        /// <param name="account">User's public address to check for claimable rewards.</param>
        /// <returns>True if rewards can be claimed by the specified user, otherwise false.</returns>
        Task<bool> CanClaimRewards(string account);

        /// <summary>
        /// Initiates the process for the current user to claim their rewards.
        /// </summary>
        /// <returns>An instance of <see cref="LootboxRewards"/> containing the details of the claimed rewards.</returns>
        Task ClaimRewards();

        /// <summary>
        /// Initiates the process for a specified user to claim their rewards.
        /// </summary>
        /// <param name="account">User's public address from which rewards are to be claimed.</param>
        /// <returns>An instance of <see cref="LootboxRewards"/> containing the details of the claimed rewards for the specified user.</returns>
        Task ClaimRewards(string account);

        /// <summary>
        /// Retrieves a list of all lootboxes along with their balances for the current user.
        /// This method aggregates data from multiple methods to provide a comprehensive view of available lootboxes.
        /// </summary>
        /// <returns>A list of <see cref="LootboxTypeInfo"/> representing each lootbox type and its corresponding balance for the current user.</returns>
        async Task<List<LootboxTypeInfo>> FetchAllLootboxes()
        {
            var typeIds = await this.GetLootboxTypes();
            var loadBalanceTasks = typeIds.Select(this.BalanceOf);
            var balances = await Task.WhenAll(loadBalanceTasks);

            return Enumerable.Range(0, typeIds.Count)
                .Select(i => new LootboxTypeInfo { TypeId = typeIds[i], Amount = balances[i] })
                .ToList();
        }

        /// <summary>
        /// Gets the native currency price to buy a lootbox.
        /// </summary>
        /// <returns>The native currency price as a string.</returns>
        Task<BigInteger> GetPrice();

        /// <summary>
        /// Sets the native currency price to buy a lootbox, admin/owner only.
        /// </summary>
        /// <param name="price">An amount of native currency user needs to pay to get a single lootbox.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SetPrice(BigInteger price);

        /// <summary>
        /// Mints/buys the requested amount of lootboxes for the caller assuming valid payment.
        /// </summary>
        /// <param name="amount">An amount lootboxes to mint.</param>
        /// <param name="maxPrice">A maximum price the caller is willing to pay per lootbox.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Buy(int amount, BigInteger maxPrice);
    }
}