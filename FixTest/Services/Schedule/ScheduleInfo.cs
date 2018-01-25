using System;

namespace FixTest.Services.Schedule
{
    public class ScheduleInfo
    {
        public long Key { get; set; }

        public long Interval { get; set; }

        public DateTimeOffset NextRun { get; set; }
    }
}