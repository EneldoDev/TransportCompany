using TransportCompany.Domain.Trucks;
using TransportCompany.TestCommon.Constants;

namespace TransportCompanu.Domain.UnitTests.Trucks
{
    public static class TruckFactory
    {
        public static Truck CreateTruck(
            string code = TruckConstants.TruckCode,
            string name = TruckConstants.TruckName,
            string? description = TruckConstants.TruckDescription)
        {
            return new Truck(code, name, description);
        }
    }
}
