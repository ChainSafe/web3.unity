using System.Collections.Generic;
using System.Linq;
using ChainSafe.Gaming.Mud.Tables;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.ABI.Model;

namespace ChainSafe.Gaming.Mud.Storages.InMemory
{
    public static class MudTableSchemaExtensions
    {
        public static IEnumerable<ParameterOutput> ColumnsToValueParametersOutput(this MudTableSchema tableSchema)
        {
            return tableSchema.Columns
                .Where(pair => tableSchema.KeyColumns.Length == 0 || !tableSchema.KeyColumns.Contains(pair.Key)) // skip keys
                .Select((pair, i) =>
            {
                var order = i + 1;
                var name = pair.Key;
                var type = pair.Value;
                var parameter = new Parameter(type, name, order);

                // we can only GetDefaultDecodingType after parameter gets constructed
                parameter.DecodedType = parameter.ABIType.GetDefaultDecodingType();

                return new ParameterOutput { Parameter = parameter, };
            });
        }

        public static IEnumerable<ParameterOutput> KeyToParametersOutput(this MudTableSchema tableSchema)
        {
            return tableSchema.KeyColumns.Select((columnName, i) =>
            {
                var columnType = tableSchema.GetColumnType(columnName);
                var parameter = new Parameter(columnType, columnName, i);

                // we can only GetDefaultDecodingType after parameter gets constructed
                parameter.DecodedType = parameter.ABIType.GetDefaultDecodingType();

                return new ParameterOutput { Parameter = parameter };
            });
        }
    }
}