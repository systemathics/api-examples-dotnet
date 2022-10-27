// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstraintsHelpers.cs" company="Systemathics SAS">
//   Copyright (c) Systemathics (rd@systemathics.com)
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Shared
{
    #region Usings

    using Systemathics.Apis.Type.Shared.V1;

    using Google.Type;

    #endregion

    /// <summary>
    /// The constraints helpers.
    /// </summary>
    public static class ConstraintsHelpers
    {
        #region Public Methods

        /// <summary>
        /// The last n days.
        /// </summary>
        /// <param name="days">
        /// The days.
        /// </param>
        /// <returns>
        /// The <see cref="Constraints"/>.
        /// </returns>
        public static Constraints LastNDays(int days)
        {
            // Create time intervals
            var yesterday = DateTime.Today.AddDays(-1);
            var start = yesterday.AddDays(-days);

            // Build the bars request date interval (we are using Google date time format : we have to cast the dates)
            var dateIntervals = new DateInterval
                                    {
                                        StartDate = new Date { Year = start.Year, Month = start.Month, Day = start.Day },
                                        EndDate = new Date { Year = yesterday.Year, Month = yesterday.Month, Day = yesterday.Day }
                                    };

            // Build the bars request time interval (we are using Google date time format : we have to cast the dates)
            // var timeInterval = new TimeInterval
            // {
            // StartTime = new TimeOfDay { Hours = 14, Minutes = 00, Seconds = 00 },
            // EndTime = new TimeOfDay { Hours = 20, Minutes = 00, Seconds = 00 }
            // };

            // Generate constraints based on the previous space selection:
            var constraints = new Constraints();
            constraints.DateIntervals.Add(dateIntervals);

            // constraints.TimeIntervals.Add(timeInterval);
            return constraints;
        }

        #endregion
    }
}