using System;
using CQRSButDifferent.Messages.Commands;
using NServiceBus;

namespace CQRSButDifferent.SnapshotEndpoint
{
    public class ScheduleSnapshot : IWantToRunWhenBusStartsAndStops
    {
        private readonly Schedule schedule;
        private readonly IBus bus;

        public ScheduleSnapshot(Schedule schedule, IBus bus)
        {
            this.schedule = schedule;
            this.bus = bus;
        }

        public void Start()
        {
            schedule.Every(TimeSpan.FromMinutes(1), () => bus.SendLocal(new CreateSnapshotForProductQuantity()));
        }

        public void Stop()
        {
        }
    }
}
