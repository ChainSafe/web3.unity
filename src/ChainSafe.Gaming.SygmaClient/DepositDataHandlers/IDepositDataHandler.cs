using ChainSafe.Gaming.SygmaClient.Types;

namespace ChainSafe.Gaming.SygmaClient.DepositDataHandlers
{
    public interface IDepositDataHandler
    {
        byte[] CreateDepositData(Transfer t);
    }
}