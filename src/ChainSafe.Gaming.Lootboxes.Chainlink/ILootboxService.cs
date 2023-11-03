using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace ChainSafe.Gaming.Lootboxes.Chainlink
{
    public interface ILootboxService
    {
        /// <summary>
        /// This method returns all lootbox type ids registered in the smart-contract.
        /// Lootbox type id also represents the number of rewards, that can be
        /// claimed by user when he opens the lootbox.
        /// </summary>
        /// <returns>Lootbox type ids registered in the smart-contract.</returns>
        Task<List<uint>> GetLootboxTypes();

        /// <summary>
        /// This method returns the balance of lootboxes for the current user.
        /// </summary>
        /// <param name="lootboxType">Lootbox type id.</param>
        /// <returns>The balance of lootboxes for the current user.</returns>
        /// <exception cref="Web3Exception">No signer was registered when building Web3. Can't get current user's address.</exception>
        Task<uint> BalanceOf(uint lootboxType);

        /// <summary>
        /// This method returns the balance of lootboxes for the specified user.
        /// </summary>
        /// <param name="account">User's public address.</param>
        /// <param name="lootboxType">Lootbox type id.</param>
        /// <returns>The balance of lootboxes for the specified user.</returns>
        Task<uint> BalanceOf(string account, uint lootboxType);

        /// <summary>
        /// Calculates open price for the player.
        /// </summary>
        /// <param name="lootboxType">Lootbox type id.</param>
        /// <param name="lootboxCount">Number of lootboxes to open.</param>
        /// <returns>Price in network's default currency.</returns>
        Task<BigInteger> CalculateOpenPrice(uint lootboxType, uint lootboxCount);

        /// <summary>
        /// Checks if a lootbox opening operation is currently in progress.
        /// </summary>
        /// <returns>True if a lootbox is being opened, otherwise false.</returns>
        Task<bool> IsOpeningLootbox();

        /// <summary>
        /// Gets the type id of the lootbox that is currently being opened.
        /// </summary>
        /// <returns>Type id of the lootbox being opened.</returns>
        Task<uint> OpeningLootboxType();

        /// <summary>
        /// Initiates the process to open a lootbox of a specific type.
        /// </summary>
        /// <param name="lootboxType">Lootbox type id to open.</param>
        /// <param name="lootboxCount">Optional parameter indicating the number of lootboxes to open. Default is 1.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task OpenLootbox(uint lootboxType, uint lootboxCount = 1);

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
        Task<LootboxRewards> ClaimRewards();

        /// <summary>
        /// Initiates the process for a specified user to claim their rewards.
        /// </summary>
        /// <param name="account">User's public address from which rewards are to be claimed.</param>
        /// <returns>An instance of <see cref="LootboxRewards"/> containing the details of the claimed rewards for the specified user.</returns>
        Task<LootboxRewards> ClaimRewards(string account);

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
    }
}