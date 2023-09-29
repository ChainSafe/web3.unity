using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Network;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Web3.Environment;
using ChainSafe.Gaming.Web3.Evm.EventPoller;
using Nethereum.Hex.HexTypes;

namespace ChainSafe.Gaming.Web3.Core.Evm.EventPoller
{
    internal class EventPoller : IEvmEvents
    {
        private readonly EventPollerConfiguration config;
        private readonly IRpcProvider rpcProvider;
        private readonly Web3Environment environment;

        private IEvmEvents.ChainChangedDelegate chainChanged;
        private IEvmEvents.PollDelegate poll;
        private IEvmEvents.NewBlockDelegate newBlock;

        private CancellationTokenSource pollLoopCts;
        private ulong nextPollId = 1;

        private ulong blockNumber;
        private ulong reportedBlock;
        private DateTime blockUpdateTime;

        public EventPoller(EventPollerConfiguration config, IRpcProvider rpcProvider, Web3Environment environment)
        {
            this.config = config;
            this.rpcProvider = rpcProvider;
            this.environment = environment;
        }

        public event IEvmEvents.ChainChangedDelegate ChainChanged
        {
            add
            {
                chainChanged += value;
                PollableHandlerAdded();
            }

            remove
            {
                chainChanged -= value;
                PollableHandlerRemoved();
            }
        }

        public event IEvmEvents.PollErrorDelegate PollError;

        public event IEvmEvents.PollDelegate Poll
        {
            add
            {
                poll += value;
                PollableHandlerAdded();
            }

            remove
            {
                poll -= value;
                PollableHandlerRemoved();
            }
        }

        public event IEvmEvents.NewBlockDelegate NewBlock
        {
            add
            {
                newBlock += value;
                PollableHandlerAdded();
            }

            remove
            {
                newBlock -= value;
                PollableHandlerRemoved();
            }
        }

        private MulticastDelegate[] AllPollableDelegates() =>
            new MulticastDelegate[]
            {
                chainChanged,
                poll,
                newBlock,
            };

        private void PollableHandlerAdded()
        {
            if (AllPollableDelegates().Any(d => d != null && d.GetInvocationList().Length > 0))
            {
                SetPollLoopState(true);
            }
        }

        private void PollableHandlerRemoved()
        {
            if (AllPollableDelegates().All(d => d != null && d.GetInvocationList().Length == 0))
            {
                SetPollLoopState(false);
            }
        }

        private void SetPollLoopState(bool enabled)
        {
            switch (enabled)
            {
                case true when pollLoopCts == null:
                    environment.LogWriter.Log("Starting event poll loop");
                    pollLoopCts = new();
                    RunPollLoop(pollLoopCts.Token);
                    break;

                case false when pollLoopCts != null:
                    // This will eventually cause the poll loop to stop.
                    // Note that restarting the poll loop will make a new task
                    // with a new cancellation token source and will not interfere
                    // with the one we're stopping here.
                    environment.LogWriter.Log("Stopping event poll loop");
                    pollLoopCts.Cancel();
                    pollLoopCts = null;
                    break;
            }
        }

        private async void RunPollLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // Since Poll is async, we can't just wait one second.
                // Also, we can't just ignore the result of Poll, since
                // we don't want to have multiple poll operations happening
                // at the same time if the network is unstable and polls
                // take longer than PollInterval. So we measure the time
                // we would like the next poll to happen before starting
                // the current poll.
                var nextPollTime = DateTime.Now + config.PollInterval;

                await DoPoll();

                var now = DateTime.Now;
                if (now < nextPollTime)
                {
                    await Task.Delay(nextPollTime - now);
                }
            }
        }

        private async Task<ulong> GetBlockNumber(TimeSpan maxAge)
        {
            // Allowing stale data up to maxAge old
            if (maxAge > TimeSpan.Zero && blockUpdateTime > DateTime.MinValue)
            {
                if (DateTime.Now - blockUpdateTime <= maxAge)
                {
                    return blockNumber;
                }
            }

            var newBlock = (await rpcProvider.GetBlockNumber()).ToUlong();
            if (newBlock < blockNumber)
            {
                newBlock = blockNumber;
            }

            blockUpdateTime = DateTime.Now;

            return newBlock;
        }

        private async Task DoPoll()
        {
            var pollId = nextPollId++;

            ulong blockNumber;
            Network newNetwork = null;
            try
            {
                blockNumber = await GetBlockNumber(config.PollInterval / 2);

                var lastNetwork = rpcProvider.LastKnownNetwork;
                var currentNetwork = await rpcProvider.RefreshNetwork();
                if (lastNetwork.ChainId != currentNetwork.ChainId)
                {
                    newNetwork = currentNetwork;
                }
            }
            catch (Exception e)
            {
                environment.LogWriter.LogError(e.ToString());
                PollError?.Invoke(e);
                return;
            }

            this.blockNumber = blockNumber;

            poll?.Invoke(pollId, blockNumber);

            if (newNetwork != null)
            {
                chainChanged?.Invoke(newNetwork.ChainId);
            }

            if (reportedBlock == 0)
            {
                reportedBlock = blockNumber;
                newBlock?.Invoke(reportedBlock);
            }
            else if (reportedBlock > blockNumber || reportedBlock < blockNumber - 1000)
            {
                PollError?.Invoke(new Exception("network block skew detected"));
                newBlock?.Invoke(blockNumber);
            }
            else
            {
                for (var i = reportedBlock + 1; i <= blockNumber; i++)
                {
                    newBlock?.Invoke(i);
                }
            }
        }
    }
}
