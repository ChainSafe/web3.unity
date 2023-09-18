using Nethereum.Util;
using NUnit.Framework;

namespace ChainSafe.Gaming.Tests
{
    public class AddressTests
    {
        [Test]
        [TestCase("0x7E5F4552091A69125d5DfCb7b8C2659029395Bdf", TestName = "one")]
        [TestCase("0xA04A864273e77BE6fE500AD2F5FAD320D9168Bb6", TestName = "small")]
        [TestCase("0xFc32402667182d11B29fab5c5e323e80483e7800", TestName = "right-zero-bit")]
        [TestCase("0x1899a98824005dDFf362a699845964D08963bC5a", TestName = "random-0")]
        [TestCase("0xA04A864273e77BE6fE500AD2F5FAD320D9168Bb6", TestName = "random-1")]
        [TestCase("0x29Bb98427ebCe59c93852cA9310A90dD89A807aB", TestName = "random-2")]
        [TestCase("0x65B71D4E2302EA1209d781c2abC681B76F9e4E50", TestName = "random-3")]
        [TestCase("0x8b4130A0cA7970b8a8A3b8Ec8a284175949d99F6", TestName = "random-4")]
        [TestCase("0x71112dd7F2178bb88C41AF9828E7b0335b334473", TestName = "random-5")]
        [TestCase("0x634Bd9C793b2F4242C2Cbaeb6070E688e934004E", TestName = "random-6")]
        [TestCase("0xf2ACf9CdCE868E2ae38B657A66aF4342b4266226", TestName = "random-7")]
        [TestCase("0xA011D4E22c57Ba8C66131EA943af8147C9DF75bb", TestName = "random-8")]
        [TestCase("0x98B70f30a7b4ec45F1dA6352CBBed414aCa4Ca3D", TestName = "random-9")]
        [TestCase("0x77EE8EAb44C2a344C676038A47DB190803A721FD", TestName = "random-10")]
        [TestCase("0x263953eB318da2872544bAD6CA7dEc0d44D3b5a2", TestName = "random-11")]
        [TestCase("0x382600e8e1578eE7196abB78Cc74251b4C2975EC", TestName = "random-12")]
        [TestCase("0x8C8A23f9EEA666A402893FeC64090bd630654721", TestName = "random-13")]
        [TestCase("0x2B72D50c8B5dCe3fa1c7DA1FaC6945C8EED41B38", TestName = "random-14")]
        [TestCase("0x2900d43c17f8A671B540Baa57C2a0f252b733b74", TestName = "random-15")]
        [TestCase("0xEd66a1a498DCA7f98cAC8Fc0C4F414e4874d281a", TestName = "random-16")]
        [TestCase("0x3974fBDa2f958A62a476f65b05FCC85844b3CE61", TestName = "random-17")]
        [TestCase("0x6747828810Dd200F5D2b41dd96778B5a2a6295e9", TestName = "random-18")]
        [TestCase("0xEEaB6Fd8b638a6c0F7d4cA16b10e6646dc0A6b97", TestName = "random-19")]
        [TestCase("0x553A8BAbc81a877eB76e443a7e739be04a0142F0", TestName = "random-20")]
        [TestCase("0x25F11a6Cc1567db5B189D89E853aa7d0314F5531", TestName = "random-21")]
        [TestCase("0x06f59896ad475AAf070Bd13aB2123d5bf20D1C21", TestName = "random-22")]
        [TestCase("0x319B11eFA1DA2A8dF366682B5ec451463107E31d", TestName = "random-23")]
        [TestCase("0xE789B95249AAA1ce50377bBa400Dee3C922dE42F", TestName = "random-24")]
        [TestCase("0xAAaFa0aD921E8766E43b342CD155Cd291E9bF8a9", TestName = "random-25")]
        [TestCase("0x3706d65A768B564bb45fE986C506E3441Dc898A1", TestName = "random-26")]
        [TestCase("0x5FFD1244d86A6904aa7fe6F04665BE76425A1537", TestName = "random-27")]
        [TestCase("0x03E5861CA93C93974844EFa340133613837BbC1d", TestName = "random-28")]
        [TestCase("0x7BFd7Bcd6dbc856f3B66058183603Dec9c2Dd6Ea", TestName = "random-29")]
        [TestCase("0xE8cCe710E64f98A17c5B9A93f5746051563D35CB", TestName = "random-30")]
        [TestCase("0x9fb73E72090548CcceF197a9889b19e85E6d42BE", TestName = "random-31")]
        [TestCase("0xE28b8cF6b923fA36103e893d8550FaACDc56E08D", TestName = "random-32")]
        [TestCase("0x8EDb62492E1774701610dD686FA5b14C5Ca99a1f", TestName = "random-33")]
        [TestCase("0x3aF4b5faf2668B59DA0E9E0Fd4290aD22B6bd599", TestName = "random-34")]
        [TestCase("0xfBEb8ab8bC628f27a57512477aD138bed0DB0A04", TestName = "random-35")]
        [TestCase("0xc5d6F7840F5E54Abb020c76944DF4200C942Cf7E", TestName = "random-36")]
        [TestCase("0x544F110EBd9591cCdD7161EE1dc28079cF64f483", TestName = "random-37")]
        [TestCase("0x4bEc025d1F5FDD1A35170ea196C2B2C3881C530A", TestName = "random-38")]
        [TestCase("0x45Fd0C7a541B3fd9e684AD1e70E476d7E4D9cBBE", TestName = "random-39")]
        [TestCase("0x8e7a8088Dec4444db5c88D8A6C698856DFACF5D9", TestName = "random-40")]
        [TestCase("0x08FA44521AA37C0260b52a1708fA1964A5eF1F40", TestName = "random-41")]
        [TestCase("0xD62EE1186dFB856178e5E1467A2E9577eA16ed2d", TestName = "random-42")]
        [TestCase("0xB8948d67849c9e17044F62a5f980f2D5E5C1aaBC", TestName = "random-43")]
        [TestCase("0x93c5c09Eaccfd01301bC4127febA9c2e79cDb76d", TestName = "random-44")]
        [TestCase("0x5923787c5546BEb63982dC0e1b6bD7CFA1A5E3A2", TestName = "random-45")]
        [TestCase("0x07C0e7D3FB740a26e460368bef7F62Ebd446BA5E", TestName = "random-46")]
        [TestCase("0x0962a95E39B6c18F680A3Cb4e9c9fCEDfb53Dc5a", TestName = "random-47")]
        [TestCase("0xA20985bd82Fa350d95a676f1e719892C5FeBbA4B", TestName = "random-48")]
        [TestCase("0x9715B1Af4f08eD48B5805F1f688b785035e056Bc", TestName = "random-49")]
        [TestCase("0xA6717a69C3BdEa8f01EC7EB2E57A6D9748A23027", TestName = "random-50")]
        public void ComputesChecksumAddressTest(string address)
        {
            Assert.IsTrue(address.IsEthereumChecksumAddress());
            Assert.IsTrue(address.IsValidEthereumAddressHexFormat());
            Assert.IsTrue(address.IsValidEthereumAddressLength());
            Assert.IsTrue(address.IsNotAnEmptyAddress());

            Assert.AreEqual(address, address.ConvertToEthereumChecksumAddress());
            Assert.AreEqual(address, address.Substring(2).ConvertToEthereumChecksumAddress());
            Assert.AreEqual(address, address.ToLower().ConvertToEthereumChecksumAddress());
            Assert.AreEqual(
                address,
                ("0x" + address.Substring(2).ToUpper()).ConvertToEthereumChecksumAddress());
        }

        [Test]
        [TestCase("0x742d35Cc6634C0532925a3b844Bc454e4438f44F", TestName = "uppercase-checksum")]
        [TestCase("0x742d35Cc6634C0532925a3b844Bc454e4438f44", TestName = "lowercase-checksum")]
        [TestCase("", TestName = "empty-checksum")]
        [TestCase(null, TestName = "null-checksum")]
        public void InvalidChecksumAddressTest(string address)
        {
            Assert.IsFalse(address.IsEthereumChecksumAddress());
        }

        [Test]
        [TestCase("0x8ba1f109551bd432803012645ac136ddd64dba", TestName = "short-length")]
        [TestCase("0x8ba1f109551bd432803012645ac136ddd64dba7200", TestName = "long-length")]
        [TestCase("", TestName = "empty-length")]
        [TestCase(null, TestName = "null-length")]
        public void InvalidAddressLengthTest(string address)
        {
            Assert.IsFalse(address.IsValidEthereumAddressLength());
        }

        [Test]
        [TestCase("0x742d35Cc6634C0532925a3b844Bc454e4438f44", "0x742d35Cc6634C0532925a3b844Bc454e4438f44", TestName = "equal-addresses")]
        [TestCase("0x742d35Cc6634C0532925a3b844Bc454e4438f44", "0x742d35cc6634c0532925a3b844bc454e4438f44", TestName = "different-casing")]
        public void IsTheSameAddressTest(string address1, string address2)
        {
            var result = address1.IsTheSameAddress(address2);

            Assert.IsTrue(result);
        }

        [Test]
        [TestCase("0x742d35Cc6634C0532925a3b844Bc454e4438f44", "0x742d35Cc6634C0532925a3b844Bc454e4438f441", TestName = "different-length")]
        [TestCase("0x742d35Cc6634C0532925a3b844Bc454e4438f44", "0xinvalidaddress", TestName = "invalid-address")]
        [TestCase("0x742d35Cc6634C0532925a3b844Bc454e4438f44", null, TestName = "null-address")]
        [TestCase("0x742d35Cc6634C0532925a3b844Bc454e4438f44", "", TestName = "empty-address")]
        public void IsNotTheSameAddressTest(string address1, string address2)
        {
            var result = address1.IsTheSameAddress(address2);

            Assert.IsFalse(result);
        }

        [Test]
        public void AddressValueOrEmptyWhenAddressIsValidTest()
        {
            const string address = "0x742d35Cc6634C0532925a3b844Bc454e4438f44";

            var result = address.AddressValueOrEmpty();

            Assert.AreEqual(address, result);
        }

        [Test]
        [TestCase("", TestName = "empty-address")]
        [TestCase(null, TestName = "null-address")]
        public void AddressValueOrEmptyWhenAddressIsEmptyTest(string address)
        {
            var result = address.AddressValueOrEmpty();

            Assert.AreEqual("0x0", result);
        }

        [Test]
        [TestCase("", "0x742d35Cc6634C0532925a3b844Bc454e4438f44", TestName = "empty-address")]
        [TestCase(null, "0x742d35Cc6634C0532925a3b844Bc454e4438f44", TestName = "null-address")]
        [TestCase("0x742d35Cc6634C0532925a3b844Bc454e4438f44", "0x742d35Cc6634C0532925a3b844Bc454e4438f44", TestName = "equal-addresses")]
        public void IsEmptyOrEqualsAddressTest(string address1, string candidate)
        {
            var result = address1.IsEmptyOrEqualsAddress(candidate);

            Assert.IsTrue(result);
        }
    }
}
