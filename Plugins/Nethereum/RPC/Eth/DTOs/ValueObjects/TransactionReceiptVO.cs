﻿using System;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.RPC.Eth.DTOs
{

    public class TransactionReceiptVO
    {
        static readonly HexBigInteger UndefinedBlockNumber = new HexBigInteger(BigInteger.Zero);
        
        public TransactionReceiptVO()
        {
            
        }

        public TransactionReceiptVO(
            Block block,
            Transaction transaction, 
            TransactionReceipt transactionReceipt, 
            bool hasError,
            string error = null, 
            bool hasVmStack = false)
        {
            Block = block;
            Transaction = transaction;
            TransactionReceipt = transactionReceipt;
            HasError = hasError;
            Error = error;
            HasVmStack = hasVmStack;
        }

        public Block Block { get; protected set; }
        public Transaction Transaction { get; protected set; }
        public TransactionReceipt TransactionReceipt { get; protected set; }
        public bool HasError { get; protected set; }
        public HexBigInteger BlockTimestamp => Block?.Timestamp;
        public string Error { get; protected set; }
        public bool HasVmStack { get; protected set; }

        public virtual HexBigInteger BlockNumber => Transaction?.BlockNumber ?? UndefinedBlockNumber;
        public virtual string TransactionHash => Transaction?.TransactionHash;
        public virtual bool Succeeded => TransactionReceipt?.Succeeded() ?? false;
        public virtual bool Failed => !Succeeded;

        public virtual string[] GetAllRelatedAddresses()
        {
            return Transaction?.GetAllRelatedAddresses(TransactionReceipt) ?? new string[] { };
        }

        public virtual bool HasLogs()
        {
            return TransactionReceipt?.HasLogs() ?? false;
        }
    }
}
