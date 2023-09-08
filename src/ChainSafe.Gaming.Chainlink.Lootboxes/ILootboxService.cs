using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.GamingWeb3;

namespace ChainSafe.Gaming.Chainlink.Lootboxes
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
        /// TODO
        /// </summary>
        Task OpenLootbox(uint lootboxType, uint lootboxCount = 1);

        Task<bool> CanClaimRewards();

        Task<bool> CanClaimRewards(string account);

        Task<LootboxRewards> ClaimRewards();

        Task<LootboxRewards> ClaimRewards(string account);
    }
}