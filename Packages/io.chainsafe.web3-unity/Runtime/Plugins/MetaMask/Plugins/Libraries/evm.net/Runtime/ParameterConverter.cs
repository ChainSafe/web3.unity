using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;
using Nethereum.ABI;
using evm.net.Models;

namespace evm.net
{
    public class ParameterConverter
    {
        private static readonly Dictionary<Type, string> _typesToEvm = new()
        {
            { typeof(BigInteger), "uint256" },
            { typeof(short), "int16" },
            { typeof(int), "int32" },
            { typeof(long), "int64" },
            //{ typeof(Int128), "int128" },
            { typeof(ushort), "uint16" },
            { typeof(uint), "uint32" },
            { typeof(ulong), "uint64" },
            //{ typeof(UInt128), "uint128" },
            { typeof(bool), "bool" },
            { typeof(string), "string" },
            { typeof(EvmAddress), "address" },
            { typeof(EvmAddress[]), "address[]" },
            { typeof(byte[]), "bytes" },
            { typeof(byte), "bytes1" },
            { typeof(string[]), "string[]" },
            { typeof(BigInteger[]), "uint256[]" },
            { typeof(short[]), "int16[]" },
            { typeof(int[]), "int32[]" },
            { typeof(long[]), "int64[]" },
            //{ typeof(Int128[]), "int128[]" },
            { typeof(ushort[]), "uint16[]" },
            { typeof(uint[]), "uint32[]" },
            { typeof(ulong[]), "uint64[]" },
            //{ typeof(UInt128[]), "uint128[]" },
            { typeof(bool[]), "bool[]" },
            { typeof(HexString), "bytes32" }
        };

        public static IDictionary<Type, string> TypeToEvm
        {
            get
            {
                return _typesToEvm;
            }
        }

        public static IDictionary<string, Type> StrictEvmToType
        {
            get
            {
                return _typesToEvm.ToDictionary(x => x.Value, x => x.Key);
            }
        }

        public static IDictionary<string, Type> DynamicEvmToType
        {
            get
            {
                var types = _typesToEvm.ToDictionary(x => x.Value, x => x.Key);
                types.Add("bytes4", typeof(byte[]));
                types.Add("uint8", typeof(ushort));
                return types;
            }
        }

        public virtual void AddType(Type type, string evmTypeName)
        {
            if (_typesToEvm.ContainsKey(type))
                throw new ArgumentException("Type " + type.FullName + " already has mapped EVM type " +
                                            _typesToEvm[type] + ". Cannot remap to new evm type " + evmTypeName);

            _typesToEvm.Add(type, evmTypeName);
        }

        public virtual ABIType ConvertParameterInfo(ParameterInfo info)
        {
            var evmParamterAttribute = info.GetCustomAttributes<EvmParameterInfoAttribute>()
                .FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.Type));

            if (evmParamterAttribute != null && !string.IsNullOrWhiteSpace(evmParamterAttribute.Type))
            {
                var typeName = evmParamterAttribute.Type;

                if (!string.IsNullOrWhiteSpace(typeName))
                {
                    return ABIType.CreateABIType(typeName.ToLower());
                }
            }

            // No attribute found, determine ABIType based on parameter type
            var type = info.ParameterType;

            if (_typesToEvm.ContainsKey(type))
                return ABIType.CreateABIType(_typesToEvm[type]);

            var isAsyncParameter = type.GetGenericTypeDefinition() == typeof(Task<>) ||
                                   type.GetGenericTypeDefinition() ==
                                   typeof(System.Runtime.CompilerServices.AsyncTaskMethodBuilder<>);

            if (!isAsyncParameter)
                throw new ArgumentException("Parameter " + info.ParameterType.FullName + " has no mapped evmType");

            type = type.GetGenericArguments()[0];
            if (_typesToEvm.ContainsKey(type))
                return ABIType.CreateABIType(_typesToEvm[type]);

            throw new ArgumentException("Inner parameter " + type.FullName +
                                        " has no mapped evmType from full parameter type " +
                                        info.ParameterType.FullName);
        }
    }
}