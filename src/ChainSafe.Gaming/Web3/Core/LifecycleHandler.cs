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
    public class LifecycleHandler
    {
        private readonly ILifecycleParticipant[] lifecycleParticipants;

        public LifecycleHandler(IEnumerable<ILifecycleParticipant> lifecycleParticipants)
        {
            // Arrange execution based on ExecutionOrder Attribute priority.
            this.lifecycleParticipants = lifecycleParticipants
                .OrderBy(p => p.GetType().GetCustomAttribute<ExecutionOrderAttribute>()?.Order ?? 0)
                .ToArray();
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
            }
            catch (Exception)
            {
                // If an exception was thrown, dispose of all initialized participants.
                foreach (var lifecycleParticipant in startedParticipants)
                {
                    await lifecycleParticipant.WillStopAsync();
                }

                // Rethrow the exception.
                throw;
            }
        }

        /// <summary>
        /// Stops all lifecycle participants.
        /// </summary>
        /// <returns>Awaitable Task.</returns>
        public async Task StopAsync()
        {
            List<Exception> exceptions = new List<Exception>();

            List<ILifecycleParticipant> stoppedParticipants = new List<ILifecycleParticipant>();

            await StopAllParticipantsAsync();

            if (exceptions.Count != 0)
            {
                throw new AggregateException(exceptions);
            }

            async Task StopAllParticipantsAsync()
            {
                try
                {
                    foreach (var participant in lifecycleParticipants.Where(p => !stoppedParticipants.Contains(p)))
                    {
                        stoppedParticipants.Add(participant);

                        await participant.WillStopAsync();
                    }
                }
                catch (Exception e)
                {
                    exceptions.Add(e);

                    await StopAllParticipantsAsync();
                }
            }
        }
    }
}