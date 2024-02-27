using System.Collections.Generic;
using ChainSafe.Gaming.SygmaClient.Dto;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.SygmaClient
{
    public static class LocalConfig
    {
        public static RawConfig Fetch()
        {
            return new RawConfig()
            {
                Domains = new List<EvmConfig>
                {
                    new()
                    {
                        Id = 1,
                        ChainId = 1337,
                        Name = "Ethereum 1",
                        Type = Network.Evm,
                        Bridge = "0x6CdE2Cd82a4F8B74693Ff5e194c19CA08c2d1c68",
                        NativeTokenSymbol = "eth",
                        NativeTokenFullName = "ether",
                        NativeTokenDecimals = new HexBigInteger(18),
                        BlockConfirmations = 1,
                        StartBlock = new HexBigInteger(3054823),
                        FeeRouter = "0x1CcB4231f2ff299E1E049De76F0a1D2B415C563A",
                        Handlers = new List<Handler>
                        {
                            new()
                            {
                                Type = ResourceType.Fungible,
                                Address = "0x02091EefF969b33A5CE8A729DaE325879bf76f90",
                            },
                            new()
                            {
                                Type = ResourceType.NonFungible,
                                Address = "0xC2D334e2f27A9dB2Ed8C4561De86C1A00EBf6760",
                            },
                            new()
                            {
                                Type = ResourceType.PermissionlessGeneric,
                                Address = "0xE837D42dd3c685839a418886f418769BDD23546b",
                            },
                            new()
                            {
                                Type = ResourceType.PermissionedGeneric,
                                Address = "0xF28c11CB14C6d2B806f99EA8b138F65e74a1Ed66",
                            },
                        },
                        FeeHandlers = new List<FeeHandler>
                        {
                            new()
                            {
                                Address = "0x8dA96a8C2b2d3e5ae7e668d0C94393aa8D5D3B94",
                                Type = FeeHandlerType.Basic,
                            },
                            new()
                            {
                                Address = "0x30d704A60037DfE54e7e4D242Ea0cBC6125aE497",
                                Type = FeeHandlerType.Dynamic,
                            },
                        },
                        Resources = new List<EvmResource>
                        {
                            new()
                            {
                                ResourceId = "0x0000000000000000000000000000000000000000000000000000000000000000",
                                Type = ResourceType.Fungible,
                                Address = "0x37356a2B2EbF65e5Ea18BD93DeA6869769099739",
                                Symbol = "ERC20TST",
                                Decimals = 18,
                            },
                            new()
                            {
                                ResourceId = "0x0000000000000000000000000000000000000000000000000000000000000200",
                                Type = ResourceType.NonFungible,
                                Address = "0xE54Dc792c226AEF99D6086527b98b36a4ADDe56a",
                                Symbol = "ERC721TST",
                                Decimals = 18,
                            },
                            new()
                            {
                                ResourceId = "0x0000000000000000000000000000000000000000000000000000000000000300",
                                Type = ResourceType.Fungible,
                                Address = "0x78E5b9cEC9aEA29071f070C8cC561F692B3511A6",
                                Symbol = "ERC20LRTest",
                                Decimals = 18,
                            },
                        },
                    },
                    new()
                    {
                        Id = 2,
                        ChainId = 1338,
                        Name = "evm2",
                        Type = Network.Evm,
                        Bridge = "0x6CdE2Cd82a4F8B74693Ff5e194c19CA08c2d1c68",
                        NativeTokenSymbol = "eth",
                        NativeTokenFullName = "ether",
                        NativeTokenDecimals = new HexBigInteger(18),
                        BlockConfirmations = 1,
                        StartBlock = new HexBigInteger(3054823),
                        FeeRouter = "0x1CcB4231f2ff299E1E049De76F0a1D2B415C563A",
                        Handlers = new List<Handler>
                        {
                            new()
                            {
                                Type = ResourceType.Fungible,
                                Address = "0x02091EefF969b33A5CE8A729DaE325879bf76f90",
                            },
                            new()
                            {
                                Type = ResourceType.NonFungible,
                                Address = "0xC2D334e2f27A9dB2Ed8C4561De86C1A00EBf6760",
                            },
                            new()
                            {
                                Type = ResourceType.PermissionlessGeneric,
                                Address = "0xE837D42dd3c685839a418886f418769BDD23546b",
                            },
                            new()
                            {
                                Type = ResourceType.PermissionedGeneric,
                                Address = "0xF28c11CB14C6d2B806f99EA8b138F65e74a1Ed66",
                            },
                        },
                        FeeHandlers = new List<FeeHandler>
                        {
                            new()
                            {
                                Address = "0x8dA96a8C2b2d3e5ae7e668d0C94393aa8D5D3B94",
                                Type = FeeHandlerType.Basic,
                            },
                            new()
                            {
                                Address = "0x30d704A60037DfE54e7e4D242Ea0cBC6125aE497",
                                Type = FeeHandlerType.Dynamic,
                            },
                        },
                        Resources = new List<EvmResource>
                        {
                            new()
                            {
                                ResourceId = "0x0000000000000000000000000000000000000000000000000000000000000000",
                                Type = ResourceType.Fungible,
                                Address = "0x37356a2B2EbF65e5Ea18BD93DeA6869769099739",
                                Symbol = "ERC20TST",
                                Decimals = 18,
                            },
                            new()
                            {
                                ResourceId = "0x0000000000000000000000000000000000000000000000000000000000000200",
                                Type = ResourceType.NonFungible,
                                Address = "0xE54Dc792c226AEF99D6086527b98b36a4ADDe56a",
                                Symbol = "ERC721TST",
                                Decimals = 18,
                            },
                            new()
                            {
                                ResourceId = "0x0000000000000000000000000000000000000000000000000000000000000300",
                                Type = ResourceType.Fungible,
                                Address = "0x78E5b9cEC9aEA29071f070C8cC561F692B3511A6",
                                Symbol = "ERC20LRTest",
                                Decimals = 18,
                            },
                        },
                    },
                },
            };
        }
    }
}