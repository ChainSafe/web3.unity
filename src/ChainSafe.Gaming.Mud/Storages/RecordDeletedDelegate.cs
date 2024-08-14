using System.Collections.Generic;

namespace ChainSafe.Gaming.Mud.Storages
{
    public delegate void RecordDeletedDelegate(byte[] tableId, List<byte[]> key);
}