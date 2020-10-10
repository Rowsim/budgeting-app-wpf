using System;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using FYP.Model;
using System.Linq;

namespace FYP.StatisticsControls.ViewModel
{
    internal class AccountsBarChartVm : BaseChartVm
    {
        private static SeriesCollection seriesCollection;
        private static string[] accountLabels;
        private static string dateRangeTitle;

        public AccountsBarChartVm()
        {
            Formatter = value => value.ToString("C");
            Separator = new Separator {Step = 1, IsEnabled = false};
        }

        public static void UpdateAccountsBarChart(List<AccountModel> accounts, DateTime startDate,
            DateTime endDate, string typeFilter = "No filter")
        {
            int dateRangeInMonths = (startDate.Year - endDate.Year) * 12 + startDate.Month - endDate.Month;
            //DateRangeTitle = GenerateMonthTitle(dateRangeInMonths);
            DateRangeTitle = startDate.ToLongDateString() + "   -   " + endDate.ToLongDateString();
            if (accounts != null)
            {
                List<string> accountNames = new List<string>(accounts.Select(x => x.Name.Remove(3)));

                SeriesCollection = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "",
                        Fill = System.Windows.Media.Brushes.OrangeRed,
                        Values = new ChartValues<double>(setChartValues(accounts, dateRangeInMonths, typeFilter))
                    }
                };

                AccountLabels = accountNames.ToArray();
            }
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

        public static string[] AccountLabels
        {
            get { return accountLabels; }
            set
            {
                if (value == accountLabels) return;
                accountLabels = value;
                AccountLabelsChanged(null, EventArgs.Empty);
            }
        }

        public static string DateRangeTitle
        {
            get { return dateRangeTitle; }
            set
            {
                if (value == dateRangeTitle) return;
                dateRangeTitle = value;
                DateRangeTitleChanged(null, EventArgs.Empty);
            }
        }

        public Func<double, string> Formatter { get; set; }

        public Separator Separator { get; set; }

        #endregion

        private static IEnumerable<double> setChartValues(List<AccountModel> accounts, int dateRangeInMonths,
            string typeFilter)
        {
            double[] result = new double[accounts.Count];
            DateTime[] dateRange = CalculateDateTimeSpan(dateRangeInMonths);
            List<TransactionModel> transactionsMatchingDate;
            int accountIndexCounter = 0;

            if (typeFilter == "No filter")
            {
                foreach (AccountModel account in accounts)
                {
                    if (account.TransactionDictionary != null)
                    {
                        foreach (DateTime date in dateRange)
                        {
                            transactionsMatchingDate =
                                account.TransactionDictionary.Values.Where(x => x.Date == date).ToList();
                            result[accountIndexCounter] +=
                                // ReSharper disable once PossibleInvalidOperationException
                                (double) transactionsMatchingDate.Select(x => x.Amount).Sum();
                        }
                    }
                    else result[accountIndexCounter] = 0;

                    accountIndexCounter++;
                }
            }
            else
            {
                foreach (AccountModel account in accounts)
                {
                    if (account.TransactionDictionary != null)
                    {
                        foreach (DateTime date in dateRange)
                        {
                            transactionsMatchingDate = account.TransactionDictionary.Values.Where(
                                x => x.Date == date && x.Type == typeFilter).ToList();
                            result[accountIndexCounter] +=
                                // ReSharper disable once PossibleInvalidOperationException
                                (double) transactionsMatchingDate.Select(x => x.Amount).Sum();
                        }
                    }
                    else result[accountIndexCounter] = 0;

                    accountIndexCounter++;
                }
            }
            return result;
        }


        #region Static EventHandlers

        public static event EventHandler SeriesCollectionChanged = delegate { };
        public static event EventHandler AccountLabelsChanged = delegate { };
        public static event EventHandler DateRangeTitleChanged = delegate { };

        #endregion
    }
}