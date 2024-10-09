using System.Collections.Generic;

namespace ChainSafe.Gaming.Mud.Storages
{
    public delegate void RecordSetDelegate(byte[] tableId, List<byte[]> key, bool newRecord);
}