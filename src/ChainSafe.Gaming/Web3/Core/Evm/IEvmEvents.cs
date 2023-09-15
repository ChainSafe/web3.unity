using System;
using System.Collections.Generic;
using System.Text;

namespace ChainSafe.Gaming.Web3.Core.Evm
{
    public interface IEvmEvents
    {
        public delegate void ChainChangedDelegate(ulong chainID);

        public delegate void PollErrorDelegate(Exception exception);

        public delegate void PollDelegate(ulong pollID, ulong blockNumber);

        public delegate void NewBlockDelegate(ulong blockNumber);

        public event ChainChangedDelegate ChainChanged;

        public event PollErrorDelegate PollError;

        public event PollDelegate Poll;

        public event NewBlockDelegate NewBlock;
    }
}
