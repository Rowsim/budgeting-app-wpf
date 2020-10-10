using System;
using LiveCharts;
using FYP.Model;
using System.Linq;
using System.Collections.Generic;

namespace FYP.StatisticsControls.ViewModel
{
    internal class TransactionsTypePieChartVm : BaseChartVm
    {
        private static ChartValues<double> incomeValue,
            foodValue,
            travelValue,
            clothesValue,
            entertainmentValue,
            billsValue,
            homeValue,
            transferValue,
            workValue,
            otherValue;

        private static readonly List<string> transactionTypes = TransactionModel.TransactionTypes;

        public TransactionsTypePieChartVm()
        {
            PointLabel = chartPoint =>
                $"£{chartPoint.Y} ({chartPoint.Participation:P}) ";
        }

        #region bindings

        public Func<ChartPoint, string> PointLabel { get; set; }

        public static ChartValues<double> IncomeValue
        {
            get { return incomeValue; }
            set
            {
                if (value == incomeValue) return;
                incomeValue = value;
                IncomeValueChanged(null, EventArgs.Empty);
            }
        }

        public static ChartValues<double> FoodValue
        {
            get { return foodValue; }
            set
            {
                if (value == foodValue) return;
                foodValue = value;
                FoodValueChanged(null, EventArgs.Empty);
            }
        }

        public static ChartValues<double> TravelValue
        {
            get { return travelValue; }
            set
            {
                if (value == travelValue) return;
                travelValue = value;
                TravelValueChanged(null, EventArgs.Empty);
            }
        }

        public static ChartValues<double> ClothesValue
        {
            get { return clothesValue; }
            set
            {
                if (value == clothesValue) return;
                clothesValue = value;
                ClothesValueChanged(null, EventArgs.Empty);
            }
        }

        public static ChartValues<double> EntertainmentValue
        {
            get { return entertainmentValue; }
            set
            {
                if (value == entertainmentValue) return;
                entertainmentValue = value;
                EntertainmentValueChanged(null, EventArgs.Empty);
            }
        }

        public static ChartValues<double> BillsValue
        {
            get { return billsValue; }
            set
            {
                if (value == billsValue) return;
                billsValue = value;
                BillsValueChanged(null, EventArgs.Empty);
            }
        }

        public static ChartValues<double> HomeValue
        {
            get { return homeValue; }
            set
            {
                if (value == homeValue) return;
                homeValue = value;
                HomeValueChanged(null, EventArgs.Empty);
            }
        }

        public static ChartValues<double> TransferValue
        {
            get { return transferValue; }
            set
            {
                if (value == transferValue) return;
                transferValue = value;
                TransferValueChanged(null, EventArgs.Empty);
            }
        }

        public static ChartValues<double> WorkValue
        {
            get { return workValue; }
            set
            {
                if (value == workValue) return;
                workValue = value;
                WorkValueChanged(null, EventArgs.Empty);
            }
        }

        public static ChartValues<double> OtherValue
        {
            get { return otherValue; }
            set
            {
                if (value == otherValue) return;
                otherValue = value;
                OtherValueChanged(null, EventArgs.Empty);
            }
        }

        #endregion

        public static void UpdatePieChartValues(AccountModel selectedAccountForPieChart, DateTime startDate, DateTime endDate)
        {
            if (selectedAccountForPieChart == null) return;
            IncomeValue = new ChartValues<double>
            {
                amountSpentOnTransactionByType(selectedAccountForPieChart, transactionTypes[0], startDate, endDate)
            };
            FoodValue = new ChartValues<double>
            {
                amountSpentOnTransactionByType(selectedAccountForPieChart, transactionTypes[1], startDate, endDate)
            };
            TravelValue = new ChartValues<double>
            {
                amountSpentOnTransactionByType(selectedAccountForPieChart, transactionTypes[2], startDate, endDate)
            };
            ClothesValue = new ChartValues<double>
            {
                amountSpentOnTransactionByType(selectedAccountForPieChart, transactionTypes[3], startDate, endDate)
            };
            EntertainmentValue = new ChartValues<double>
            {
                amountSpentOnTransactionByType(selectedAccountForPieChart, transactionTypes[4], startDate, endDate)
            };
            BillsValue = new ChartValues<double>
            {
                amountSpentOnTransactionByType(selectedAccountForPieChart, transactionTypes[5], startDate, endDate)
            };
            HomeValue = new ChartValues<double>
            {
                amountSpentOnTransactionByType(selectedAccountForPieChart, transactionTypes[6], startDate, endDate)
            };
            TransferValue = new ChartValues<double>
            {
                amountSpentOnTransactionByType(selectedAccountForPieChart, transactionTypes[7], startDate, endDate)
            };
            WorkValue = new ChartValues<double>
            {
                amountSpentOnTransactionByType(selectedAccountForPieChart, transactionTypes[8], startDate, endDate)
            };
            OtherValue = new ChartValues<double>
            {
                amountSpentOnTransactionByType(selectedAccountForPieChart, transactionTypes[9], startDate, endDate)
            };
        }

        private static bool meetsTransactionTypeFilterRequirements(TransactionModel item, string type)
        {
            return item.Type == type;
        }

        private static int amountSpentOnTransactionByType(AccountModel selectedAccountForPieChart, string type, DateTime startDate,
            DateTime endDate)
        {
            if (selectedAccountForPieChart.TransactionDictionary == null) return 0;

            if (startDate > endDate)
            {
                return (int) selectedAccountForPieChart.TransactionDictionary.Values
                    .Where(x => meetsTransactionTypeFilterRequirements(x, type)
                                && x.Date <= startDate && x.Date >= endDate)
                    .Select(x => x.Amount).Sum();
            }
            return (int) selectedAccountForPieChart.TransactionDictionary.Values
                .Where(x => meetsTransactionTypeFilterRequirements(x, type)
                            && x.Date >= startDate && x.Date <= endDate)
                .Select(x => x.Amount).Sum();
        }

        #region Static EventHandlers

        public static event EventHandler IncomeValueChanged = delegate { };
        public static event EventHandler FoodValueChanged = delegate { };
        public static event EventHandler TravelValueChanged = delegate { };
        public static event EventHandler ClothesValueChanged = delegate { };
        public static event EventHandler EntertainmentValueChanged = delegate { };
        public static event EventHandler BillsValueChanged = delegate { };
        public static event EventHandler HomeValueChanged = delegate { };
        public static event EventHandler TransferValueChanged = delegate { };
        public static event EventHandler WorkValueChanged = delegate { };
        public static event EventHandler OtherValueChanged = delegate { };

        #endregion
    }
}