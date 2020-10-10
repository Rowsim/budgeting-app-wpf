using System;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using LiveCharts.Defaults;
using FYP.Model;
using System.Linq;

namespace FYP.StatisticsControls.ViewModel
{
    internal class RecentTransactionsLineChartVm : BaseChartVm
    {
        private static List<DateTimePoint> dateTimePoints;
        private static SeriesCollection seriesCollection;
        private static string labelMonthDate;
        private static string[] dateLabels;


        public RecentTransactionsLineChartVm()
        {
            dateTimePoints = new List<DateTimePoint>();
            YFormatter = value => value.ToString("C");
            Separator = new Separator {Step = 1, IsEnabled = false};
        }

        #region bindings

        public static SeriesCollection SeriesCollection
        {
            get { return seriesCollection; }
            set
            {
                if (value == seriesCollection) return;
                seriesCollection = value;
                SeriesCollectionChanged(null, EventArgs.Empty);
            }
        }

        public static string[] DateLabels
        {
            get { return dateLabels; }
            set
            {
                if (value == dateLabels) return;
                dateLabels = value;
                DateLabelsChanged(null, EventArgs.Empty);
            }
        }

        public static string LabelMonthDate
        {
            get { return labelMonthDate; }
            set
            {
                if (value == labelMonthDate) return;
                labelMonthDate = value;
                LabelMonthDateChanged(null, EventArgs.Empty);
            }
        }

        public Func<double, string> YFormatter { get; set; }
        public Separator Separator { get; set; }

        #endregion

        public static void UpdateRecentTransactionsLineChart(AccountModel selectedAccount, DateTime startDate,
            DateTime endDate, string typeFilter = "No filter")
        {
            if (selectedAccount != null)
            {
                setDateTimePoints(selectedAccount, startDate, endDate, typeFilter);

                SeriesCollection = new SeriesCollection
                {
                    new LineSeries
                    {
                        Stroke = System.Windows.Media.Brushes.OrangeRed,
                        OpacityMask = System.Windows.Media.Brushes.OrangeRed,

                        Values = new ChartValues<double>(dateTimePointValuesToArray()),
                        Title = "",
                        LabelPoint = point => getTransactionsForLabelPoint(point.X, selectedAccount)
                    }
                };

                DateLabels = dateTimePointDatesToArray();
                LabelMonthDate = startDate.ToLongDateString() + "   -   " + endDate.ToLongDateString();
                //LabelMonthDate = dateTimePointDatesToArray().First() + "   -   " + dateTimePointDatesToArray().Last();
            }
        }

        private static string getTransactionsForLabelPoint(double labelPoint, AccountModel selectedAccount)
        {
            var transactionsMatchingDates = dateTimePoints.Select(dtp =>
                selectedAccount.TransactionDictionary.Values.Where(x => x.Date == dtp.DateTime).ToList()).ToList();
            var transactions = transactionsMatchingDates[(int) labelPoint];
            return transactions.Aggregate<TransactionModel, string>(null,
                (current, t) => current + (t.Type + ": £" + t.Amount) + Environment.NewLine);
        }

        private static void setDateTimePoints(AccountModel selectedAccount, DateTime startDate, DateTime endDate, string typeFilter)
        {
            int dateRangeInMonths = (startDate.Year - endDate.Year) * 12 + startDate.Month - endDate.Month;
            dateTimePoints.Clear();
            DateTime[] rangeTimeArray = CalculateDateTimeSpan(dateRangeInMonths);

            foreach (DateTime date in rangeTimeArray)
            {
                List<TransactionModel> transactionsMatchingDate;
                if (typeFilter == "No filter")
                {
                    transactionsMatchingDate = selectedAccount.TransactionDictionary.Values.Where(
                        x => x.Date == date).ToList();
                }
                else
                {
                    transactionsMatchingDate = selectedAccount.TransactionDictionary.Values.Where(
                        x => x.Date == date
                             && x.Type == typeFilter).ToList();
                }

                double amountSpentOnDate = transactionsMatchingDate.Sum(transaction => (double) transaction.Amount);

                if (amountSpentOnDate > 0)
                    dateTimePoints.Add(new DateTimePoint(date, amountSpentOnDate));
            }
        }

        private static IEnumerable<double> dateTimePointValuesToArray()
        {
            return dateTimePoints.Select(dtp => dtp.Value).ToArray();
        }

        private static string[] dateTimePointDatesToArray()
        {
            return dateTimePoints.Select(dtp => dtp.DateTime.ToString("MMM yy").ToUpper()).ToArray();
        }

        #region Static EventHandlers

        public static event EventHandler SeriesCollectionChanged = delegate { };
        public static event EventHandler DateLabelsChanged = delegate { };
        public static event EventHandler LabelMonthDateChanged = delegate { };

        #endregion
    }
}