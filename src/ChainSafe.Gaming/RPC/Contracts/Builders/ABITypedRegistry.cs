using System;
using System.Collections.Concurrent;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;

namespace ChainSafe.Gaming.Evm.Contracts.Builders
{
    /// <summary>
    /// A static class for managing and accessing ABI (Application Binary Interface) types for smart contracts.
    /// </summary>
    public static class ABITypedRegistry
    {
        private static readonly ConcurrentDictionary<Type, FunctionABI> FunctionAbiRegistry = new();
        private static readonly ConcurrentDictionary<Type, EventABI> EventAbiRegistry = new();
        private static readonly ConcurrentDictionary<Type, ErrorABI> ErrorAbiRegistry = new();

        private static readonly AttributesToABIExtractor AbiExtractor = new();

        /// <summary>
        /// Gets the FunctionABI of type TFunctionMessage.
        /// </summary>
        /// <typeparam name="TFunctionMessage">The type of the function message.</typeparam>
        /// <returns>An object of type FunctionABI.</returns>
        public static FunctionABI GetFunctionABI<TFunctionMessage>()
        {
            return GetFunctionABI(typeof(TFunctionMessage));
        }

        /// <summary>
        /// Gets the FunctionABI for the given type.
        /// </summary>
        /// <param name="functionABIType">The type to get the FunctionABI for.</param>
        /// <returns>An object of type FunctionABI.</returns>
        /// <exception cref="ArgumentException">Thrown when the given type is not a valid FunctionABI type.</exception>
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

        /// <summary>
        /// Gets the EventABI of type TEvent.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <returns>An object of type EventABI.</returns>
        public static EventABI GetEvent<TEvent>()
        {
            return GetEvent(typeof(TEvent));
        }

        /// <summary>
        /// Gets the EventABI for the given type.
        /// </summary>
        /// <param name="type">The type to get the EventABI for.</param>
        /// <returns>An object of type EventABI.</returns>
        /// <exception cref="ArgumentException">Thrown when the given type is not a valid EventABI type.</exception>
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

        /// <summary>
        /// Gets the ErrorABI of type TError.
        /// </summary>
        /// <typeparam name="TError">The type of the error.</typeparam>
        /// <returns>An object of type ErrorABI.</returns>
        public static ErrorABI GetError<TError>()
        {
            return GetError(typeof(TError));
        }

        /// <summary>
        /// Gets the ErrorABI for the given type.
        /// </summary>
        /// <param name="type">The type to get the ErrorABI for.</param>
        /// <returns>An object of type ErrorABI.</returns>
        /// <exception cref="ArgumentException">Thrown when the given type is not a valid ErrorABI type.</exception>
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