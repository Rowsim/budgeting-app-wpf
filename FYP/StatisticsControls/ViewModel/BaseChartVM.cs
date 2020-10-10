using System;
using System.Linq;

namespace FYP.StatisticsControls.ViewModel
{
    internal class BaseChartVm
    {
        protected static DateTime[] CalculateDateTimeSpan(int dateRangeInMonths)
        {
            TimeSpan rangeTimeSpan;
            DateTime timeEndDate = DateTime.Today;
            DateTime timeStartDate;

            if (dateRangeInMonths <= 0) //Date range in the past
            {
                timeStartDate = timeEndDate.AddMonths(dateRangeInMonths);
                rangeTimeSpan = timeEndDate.Subtract(timeStartDate);
            }
            else //Date range in the future
            {
                timeStartDate = timeEndDate.AddMonths(dateRangeInMonths);
                rangeTimeSpan = timeEndDate.Subtract(timeStartDate).Negate();
            }

            var rangeTimeArray = new DateTime[rangeTimeSpan.Days];


            for (int i = 0; i < rangeTimeSpan.Days; i++)
            {
                timeStartDate = dateRangeInMonths <= 0 ? timeStartDate.AddDays(1) : timeStartDate.AddDays(-1);
                rangeTimeArray[i] = timeStartDate;
            }
            return dateRangeInMonths <= 0 ? rangeTimeArray : rangeTimeArray.Reverse().ToArray();
        }

        protected static string GenerateMonthTitle(int dateRangeInMonths)
        {
            if (dateRangeInMonths <= 0)
            {
                return DateTime.Today.AddMonths(dateRangeInMonths).ToLongDateString() +
                       "                    -                    " + DateTime.Today.ToLongDateString();
            }
            return DateTime.Today.ToLongDateString() + "                    -                    " +
                   DateTime.Today.AddMonths(dateRangeInMonths).ToLongDateString();
        }
    }
}