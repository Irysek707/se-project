using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haulage.Model.Helpers
{
    static class OtherHelpers
    {

    public static bool CheckLongitudeAndLatitude(double  longitude, double latitude)
        {
            if (longitude < -180 || longitude > 180)
            {
                throw new ArgumentException("Longitude must be between -180 and 180");
            }
            if (latitude < -90 || latitude > 90)
            {
                throw new ArgumentException("Latitude must be between -90 and 90");
            }
            return true;
        }

        public static TimeSpan CalculateTripDuration(double startLongitude, double startLatitude, List<TripStop> stops)
        {
            /// Placeholder here, to leave space in future to implement integration with maps or similar
            return new TimeSpan(2, 20, 0);
        }
    }
}
