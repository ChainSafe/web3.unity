using System;
using ChainSafe.Gaming.SygmaClient.Dto;
using ChainSafe.Gaming.SygmaClient.Types;

namespace ChainSafe.Gaming.SygmaClient.DepositDataHandlers
{
    public static class DepositDataFactory
    {
        public static IDepositDataHandler Handler(ResourceType resourceType)
        {
            return resourceType switch
            {
                ResourceType.Erc1155 => Erc115DepositDataHandler.Instance,
                ResourceType.Erc721 => Erc721DepositDataHandler.Instance,
                _ => throw new NotImplementedException($"Handler not implemented for resource type: {resourceType} And Resource Type: {resourceType}")
            };
        }
    }
}