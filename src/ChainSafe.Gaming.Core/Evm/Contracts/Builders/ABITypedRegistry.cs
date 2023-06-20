using System;
using System.Collections.Concurrent;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;

namespace Web3Unity.Scripts.Library.Ethers.Contracts.Builders
{
    public static class ABITypedRegistry
    {
        private static readonly ConcurrentDictionary<Type, FunctionABI> FunctionAbiRegistry = new();
        private static readonly ConcurrentDictionary<Type, EventABI> EventAbiRegistry = new();
        private static readonly ConcurrentDictionary<Type, ErrorABI> ErrorAbiRegistry = new();

        private static readonly AttributesToABIExtractor AbiExtractor = new();

        public static FunctionABI GetFunctionABI<TFunctionMessage>()
        {
            return GetFunctionABI(typeof(TFunctionMessage));
        }

        public static FunctionABI GetFunctionABI(Type functionABIType)
        {
            if (!FunctionAbiRegistry.ContainsKey(functionABIType))
            {
                var functionAbi = AbiExtractor.ExtractFunctionABI(functionABIType) ??
                    throw new ArgumentException(functionABIType.ToString() + " is not a valid Function Type");
                FunctionAbiRegistry[functionABIType] = functionAbi;
            }

            return FunctionAbiRegistry[functionABIType];
        }

        public static EventABI GetEvent<TEvent>()
        {
            return GetEvent(typeof(TEvent));
        }

        public static EventABI GetEvent(Type type)
        {
            if (!EventAbiRegistry.ContainsKey(type))
            {
                var eventABI = AbiExtractor.ExtractEventABI(type) ??
                    throw new ArgumentException(type.ToString() + " is not a valid Event Type");
                EventAbiRegistry[type] = eventABI;
            }

            return EventAbiRegistry[type];
        }

        public static ErrorABI GetError<TError>()
        {
            return GetError(typeof(TError));
        }

        public static ErrorABI GetError(Type type)
        {
            if (!ErrorAbiRegistry.ContainsKey(type))
            {
                var errorABI = AbiExtractor.ExtractErrorABI(type) ??
                    throw new ArgumentException(type.ToString() + " is not a valid Error Type");
                ErrorAbiRegistry[type] = errorABI;
            }

            return ErrorAbiRegistry[type];
        }
    }
}