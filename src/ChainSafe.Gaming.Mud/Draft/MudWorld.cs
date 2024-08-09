namespace ChainSafe.Gaming.Mud.Draft
{
    public class MudWorld
    {
        private readonly MudWorldSystems systems; // todo implement systems OR make MudWorld an IContract
        private readonly MudWorldTables tables;

        public MudWorld(MudWorldConfig config, IMudStorage storage)
        {
            systems = new MudWorldSystems(config.ContractAddress, config.ContractAbi);
            tables = new MudWorldTables(config.TableSchemas, storage);
        }

        public MudTable GetTable(string @namespace, string tableName)
        {
            return tables.GetTable(@namespace, tableName);
        }
    }

    public class MudWorldSystems
    {
        private string contractAddress;
        private string contractAbi;

        public MudWorldSystems(string contractAddress, string contractAbi)
        {
            this.contractAbi = contractAbi;
            this.contractAddress = contractAddress;
        }
    }
}