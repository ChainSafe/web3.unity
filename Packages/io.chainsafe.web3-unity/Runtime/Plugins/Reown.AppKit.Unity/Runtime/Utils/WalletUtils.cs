using Newtonsoft.Json;
using Reown.AppKit.Unity.Model;
using Reown.Sign.Unity;
using UnityEngine;

namespace Reown.AppKit.Unity.Utils
{
    public static class WalletUtils
    {
        public static bool IsWalletInstalled(Wallet wallet)
        {
            if (wallet.MobileLink == null || wallet.MobileLink.StartsWith("http"))
                return false;

            var link = wallet.MobileLink;

            if (!link.EndsWith("//"))
                link = $"{link}//";

            link = $"{link}wc";

            return Linker.CanOpenURL(link);
        }

        public static void SetRecentWallet(Wallet wallet)
        {
            if (wallet == null)
                return;

            PlayerPrefs.SetString("RE_RECENT_WALLET", JsonConvert.SerializeObject(wallet));
        }

        public static bool TryGetRecentWallet(out Wallet wallet)
        {
            wallet = null;

            var recentWalletJson = PlayerPrefs.GetString("RE_RECENT_WALLET");

            if (string.IsNullOrWhiteSpace(recentWalletJson))
                return false;

            wallet = JsonConvert.DeserializeObject<Wallet>(recentWalletJson);

            return wallet != null;
        }

        public static void SetLastViewedWallet(Wallet wallet)
        {
            if (wallet == null)
                return;

            PlayerPrefs.SetString("RE_LAST_VIEWED_WALLET", JsonConvert.SerializeObject(wallet));
            PlayerPrefs.SetString("RE_RECENT_WALLET_DEEPLINK", Application.isMobilePlatform ? wallet.MobileLink : wallet.DesktopLink);
        }

        public static void RemoveLastViewedWallet()
        {
            PlayerPrefs.DeleteKey("RE_LAST_VIEWED_WALLET");
        }

        public static bool TryGetLastViewedWallet(out Wallet wallet)
        {
            wallet = null;

            var recentWalletJson = PlayerPrefs.GetString("RE_LAST_VIEWED_WALLET");

            if (string.IsNullOrWhiteSpace(recentWalletJson))
                return false;

            wallet = JsonConvert.DeserializeObject<Wallet>(recentWalletJson);

            return wallet != null;
        }
    }
}