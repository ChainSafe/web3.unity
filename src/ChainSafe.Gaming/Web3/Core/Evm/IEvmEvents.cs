using System;
using System.Collections.Generic;
using System.Text;

namespace ChainSafe.Gaming.Web3.Core.Evm
{
    public interface IEvmEvents
    {
        /// <summary>
        /// Represents a delegate for handling chain change events.
        /// </summary>
        /// <param name="chainID">The chain ID of the new chain.</param>
        public delegate void ChainChangedDelegate(ulong chainID);

        /// <summary>
        /// Represents a delegate for handling poll error events.
        /// </summary>
        /// <param name="exception">The exception representing the poll error.</param>
        public delegate void PollErrorDelegate(Exception exception);

        /// <summary>
        /// Represents a delegate for handling poll events.
        /// </summary>
        /// <param name="pollID">The ID of the poll event.</param>
        /// <param name="blockNumber">The block number associated with the poll event.</param>
        public delegate void PollDelegate(ulong pollID, ulong blockNumber);

        /// <summary>
        /// Represents a delegate for handling new block events.
        /// </summary>
        /// <param name="blockNumber">The block number of the new block.</param>
        public delegate void NewBlockDelegate(ulong blockNumber);

        /// <summary>
        /// Occurs when the chain is changed.
        /// </summary>
        public event ChainChangedDelegate ChainChanged;

        /// <summary>
        /// Occurs when an error is encountered during polling.
        /// </summary>
        public event PollErrorDelegate PollError;

        /// <summary>
        /// Occurs when a poll event is triggered.
        /// </summary>
        public event PollDelegate Poll;

        /// <summary>
        /// Occurs when a new block is detected.
        /// </summary>
        public event NewBlockDelegate NewBlock;
    }
}
