using System.Collections.Generic;
using System.Linq;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.ABI.Model;

namespace ChainSafe.Gaming.Mud.Draft.InMemory
{
    public static class MudTableSchemaExtensions
    {
        public static IEnumerable<ParameterOutput> ColumnsToParametersOutput(this MudTableSchema tableSchema)
        {
            return tableSchema.Columns.Select((pair, i) =>
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
    }
}