using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.NetCore;
using ChainSafe.Gaming.Tests.Core;
using ChainSafe.Gaming.WalletConnect;
using ChainSafe.Gaming.WalletConnect.Models;
using ChainSafe.Gaming.WalletConnect.Wallets;
using ChainSafe.Gaming.Web3.Build;
using ChainSafe.Gaming.Web3.Core;
using ChainSafe.Gaming.Web3.Environment;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ChainSafe.Gaming.Tests
{
    // [TestFixture] // todo unstable currently; uncomment after new version of WalletRegistry with pre-downloaded, filtered list of wallets introduced
    // public class WalletConnectDeepLinksTests
    // {
    //     private static readonly List<string> KnownBrokenWalletsIos = new() // todo note in the documentation
    //     {
    //         "Argent", "Blockchain.com", "MEW wallet", "Obvious", "Internet Money Wallet", "Arculus Wallet", "Kelp",
    //         "Coinomi", "Everspace", "Oxalus Wallet", "ISLAMIwallet", "FILWallet", "Snowball", "LocalTrade Wallet",
    //         "Spatium", "Assure", "Flooz", "Keeper", "Klever Wallet", "NeftiWallet", "RiceWallet", "Plasma Wallet",
    //         "Bee Wallet", "SimpleHold", "Flow Wallet", "HUMBL WALLET", "Stickey Wallet", "Catecoin Wallet", "Konio",
    //         "Ronin Wallet", "f(x)Wallet", "pockie", "BeanBag", "New Money", "Concordium", "Ape Wallet",
    //         "Trustee Wallet", "HERE Wallet", "Plena-App", "AZCoiner", "GateWallet", "Puzzle Wallet", "Armana Portal",
    //         "Cogni ", "Feral File", "CoinStats", "paycool", "D'CENT Wallet",
    //     };
    //
    //     private static readonly List<string> KnownBrokenWalletsDesktop = new() // todo note in the documentation
    //     {
    //         "Internet Money Wallet", "Flooz", "RiceWallet", "Okto", "DMToken", "campux.digital", "Dippi", "LichtBit",
    //         "MELDapp", "Numo Wallet", "Hoo!Wallet", "37x", "SmartRush", "DexTrade", "Deficloud",
    //     };
    //
    //     private HttpClient httpClient;
    //
    //     private enum FailureReason
    //     {
    //         NotFound,
    //         TimedOut,
    //         HttpRequestError,
    //     }
    //
    //     [OneTimeSetUp]
    //     public void Setup()
    //     {
    //         httpClient = new HttpClient
    //         {
    //             Timeout = TimeSpan.FromSeconds(10),
    //         };
    //     }
    //
    //     [Test]
    //     public Task TestIOSDeepLinks()
    //     {
    //         return TestDeepLinks(Platform.IOS, KnownBrokenWalletsIos);
    //     }
    //
    //     [Test]
    //     public Task TestDesktopDeepLinks()
    //     {
    //         return TestDeepLinks(Platform.Desktop, KnownBrokenWalletsDesktop);
    //     }
    //
    //     private async Task TestDeepLinks(Platform platform, List<string> ignoreWallets)
    //     {
    //         var web3 = await new Web3Builder(new ProjectConfig { ProjectId = "NULL" }, new ChainConfig())
    //             .Configure(services =>
    //             {
    //                 services.UseStubWeb3Environment(operatingSystemMediator: new StubOperatingSystemMediator() { Platform = platform }, httpClient: new NetCoreHttpClient());
    //                 services.AddSingleton<IRpcProvider, StubRpcProvider>();
    //                 services.AddSingleton<IWalletConnectConfig>(new WalletConnectConfig { ProjectId = "f4bff60eb260841f46b1c77588cd8acb" });
    //                 services.AddSingleton<IWalletRegistry, ILifecycleParticipant, WalletRegistry>();
    //                 services.AddSingleton<RedirectionHandler>();
    //             })
    //             .LaunchAsync();
    //
    //         var supportedWallets = web3.WalletConnect().WalletRegistry()
    //             .EnumerateSupportedWallets(platform)
    //             .Where(w => !ignoreWallets.Contains(w.Name))
    //             .ToList();
    //         TestContext.Out.WriteLine($"Testing {supportedWallets.Count} wallets..");
    //
    //         var brokenWallets = new List<(WalletModel wallet, FailureReason reson)>();
    //         foreach (var walletData in supportedWallets)
    //         {
    //             var deepLink = web3.WalletConnect().RedirectionHandler()
    //                 .BuildConnectionDeeplink(walletData, "TEST_URI");
    //
    //             if (!deepLink.StartsWith("http"))
    //             {
    //                 TestContext.Out.WriteLine($"SKIPPING {deepLink}");
    //                 continue; // todo test native deep links too
    //             }
    //
    //             TestContext.Out.WriteLine(deepLink);
    //
    //             try
    //             {
    //                 var response = await httpClient.GetAsync(deepLink);
    //
    //                 if (response.StatusCode == HttpStatusCode.NotFound)
    //                 {
    //                     brokenWallets.Add((walletData, FailureReason.NotFound));
    //                 }
    //             }
    //             catch (HttpRequestException)
    //             {
    //                 brokenWallets.Add((walletData, FailureReason.HttpRequestError));
    //             }
    //             catch (TaskCanceledException)
    //             {
    //                 brokenWallets.Add((walletData, FailureReason.TimedOut));
    //             }
    //         }
    //
    //         TestContext.Out.WriteLine(string.Join(", ", brokenWallets.Select(tuple => $"\"{tuple.wallet.Name}\"")));
    //
    //         Assert.IsEmpty(brokenWallets, $"{brokenWallets.Count} links are broken:\n{string.Join(",\n", brokenWallets.Select(BuildBrokenWalletString))}");
    //         return;
    //
    //         string BuildBrokenWalletString((WalletModel wallet, FailureReason reason) brokenWallet)
    //         {
    //             var name = brokenWallet.wallet.Name;
    //             var reason = brokenWallet.reason;
    //             var link = web3.WalletConnect().RedirectionHandler()
    //                 .BuildConnectionDeeplink(brokenWallet.wallet, "TEST_URI");
    //             return $"{name} ({reason}) - {link}";
    //         }
    //     }
    // }
}