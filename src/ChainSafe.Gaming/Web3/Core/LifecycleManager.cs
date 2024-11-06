using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ChainSafe.Gaming.Web3.Core
{
    /// <summary>
    /// Handles the lifecycle of <see cref="ILifecycleParticipant"/> instances.
    /// </summary>
    public class LifecycleManager
    {
        private readonly ILifecycleParticipant[] lifecycleParticipants;

        public LifecycleManager(IEnumerable<ILifecycleParticipant> lifecycleParticipants)
        {
            // Arrange execution based on ExecutionOrder Attribute priority.
            this.lifecycleParticipants = lifecycleParticipants.OrderBy(p => p.ExecutionOrder).ToArray();
        }

        /// <summary>
        /// Starts all lifecycle participants.
        /// </summary>
        /// <returns>Awaitable Task.</returns>
        public async Task StartAsync()
        {
            List<ILifecycleParticipant> startedParticipants = new List<ILifecycleParticipant>();

            try
            {
                foreach (var lifecycleParticipant in lifecycleParticipants)
                {
                    await lifecycleParticipant.WillStartAsync();

                    startedParticipants.Add(lifecycleParticipant);
                }

                startedParticipants.Clear();
            }
            finally
            {
                // If an exception was thrown, dispose of all initialized participants.
                if (startedParticipants.Count != 0)
                {
                    await StopLifecycleParticipantsAsync(startedParticipants);
                }
            }
        }

        /// <summary>
        /// Stops all lifecycle participants.
        /// </summary>
        /// <returns>Awaitable Task.</returns>
        public async Task StopAsync()
        {
            await StopLifecycleParticipantsAsync(lifecycleParticipants);
        }

        private async Task StopLifecycleParticipantsAsync(IEnumerable<ILifecycleParticipant> participants)
        {
            Queue<ILifecycleParticipant> stoppageQueue =
                new Queue<ILifecycleParticipant>(participants.OrderByDescending(p => p.ExecutionOrder));

            List<Exception> exceptions = new List<Exception>();

            while (stoppageQueue.Count != 0)
            {
                ILifecycleParticipant participant = stoppageQueue.Dequeue();

                try
                {
                    await participant.WillStopAsync();
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }

            if (exceptions.Count != 0)
            {
                throw new AggregateException(exceptions);
            }
        }
    }
}