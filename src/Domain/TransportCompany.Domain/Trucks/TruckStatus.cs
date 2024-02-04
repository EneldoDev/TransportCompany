namespace TransportCompany.Domain.Trucks
{
    public enum TruckStatus
    {
        OutOfService = -1,
        Loading = 1,
        ToJob = 2,
        AtJob = 3,
        Returning = 4
    }

    public static class TruckStatusTransitions
    {
        private static Dictionary<TruckStatus, ICollection<TruckStatus>> _statusTransitions => new Dictionary<TruckStatus, ICollection<TruckStatus>>
        {
            {
                TruckStatus.OutOfService,  new []{TruckStatus.Loading, TruckStatus.ToJob, TruckStatus.AtJob, TruckStatus.Returning }
            },
            {
                TruckStatus.Loading,  new []{TruckStatus.OutOfService, TruckStatus.ToJob}
            },
            {
                TruckStatus.ToJob,  new []{TruckStatus.OutOfService, TruckStatus.AtJob }
            },
            {
                TruckStatus.AtJob,  new []{TruckStatus.OutOfService, TruckStatus.Returning }
            },
            {
                TruckStatus.Returning,  new []{TruckStatus.OutOfService, TruckStatus.Loading}
            }
        };

        public static bool IsValidTransition(TruckStatus oldStatus, TruckStatus newStatus)
        {
            return _statusTransitions[oldStatus].Contains(newStatus);
        }
    }
}
