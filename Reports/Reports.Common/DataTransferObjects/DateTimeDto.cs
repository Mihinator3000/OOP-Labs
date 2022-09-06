using System;

namespace Reports.Common.DataTransferObjects
{
    public class DateTimeDto
    {
        public DateTime Time { get; set; }

        public static implicit operator DateTimeDto(DateTime time)
        {
            return new DateTimeDto {Time = time};
        }

        public static implicit operator DateTime(DateTimeDto time)
        {
            return time.Time;
        }
    }
}