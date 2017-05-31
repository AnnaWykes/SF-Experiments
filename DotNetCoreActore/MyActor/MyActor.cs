using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using ActorBackendService.Interfaces;

namespace ActorBackendService
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class MyActor : Actor, IMyActor, IRemindable
    {
        /// <summary>
        /// Initializes a new instance of MyActor
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        private const string ReminderName = "Reminder";
        private const string StateName = "Count";

        /// <summary>
        /// Initializes a new instance of ActorBackendService
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public MyActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        public async Task StartProcessingAsync(CancellationToken cancellationToken)
        {
            try
            {
                this.GetReminder(ReminderName);

                bool added = await this.StateManager.TryAddStateAsync<long>(StateName, 0);

                if (!added)
                {
                    // value already exists, which means processing has already started.
                    throw new InvalidOperationException("Processing for this actor has already started.");
                }
            }
            catch (ReminderNotFoundException)
            {
                await this.RegisterReminderAsync(ReminderName, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(10));
            }
        }

        public async Task ReceiveReminderAsync(string reminderName, byte[] context, TimeSpan dueTime, TimeSpan period)
        {
            if (reminderName.Equals(ReminderName, StringComparison.OrdinalIgnoreCase))
            {
                long currentValue = await this.StateManager.GetStateAsync<long>(StateName);

                ActorEventSource.Current.ActorMessage(this, $"Processing. Current value: {currentValue}");

                await this.StateManager.SetStateAsync<long>(StateName, ++currentValue);
            }
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override async Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");
           // StateManager.TryAddStateAsync<>



            // The StateManager is this actor's private state store.
            // Data stored in the StateManager will be replicated for high-availability for actors that use volatile or persisted state storage.
            // Any serializable object can be saved in the StateManager.
            // For more information, see https://aka.ms/servicefabricactorsstateserialization
            await base.OnActivateAsync();
        }
    }
}
