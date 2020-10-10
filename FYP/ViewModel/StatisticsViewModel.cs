using FYP.Model;
using FYP.StatisticsControls.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FYP.ViewModel
{
    internal class StatisticsViewModel : BaseViewModel
    {
        private ObservableCollection<AccountModel> obcStatisticsAccounts;
        private AccountModel selectedStatisticsAccount;
        private string selectedGraph, selectedTransactionTypeFilter;

        private bool statisticsVisibility,
            invalidAccountVisibility,
            pieChartVisibility,
            lineChartTransactionsVisibility,
            barChartAccountsVisibility,
            filterControlsVisibility;

        private string accountIncomeLabel,
            accountExpenditureLabel,
            commonTransactionTypeLabel,
            commonTransactionDayLabel;

        private DateTime startDate = DateTime.Today;
        private DateTime endDate;

        public StatisticsViewModel()
        {
            obcStatisticsAccounts = new ObservableCollection<AccountModel>(GetAccountsWithTransactions());
            StatisticsAccountsForCharts = new List<AccountModel>((GetAccountsWithTransactions()));
            if (obcStatisticsAccounts.Count >= 1)
            {
                SelectedStatisticsAccount = selectAccount();
                if (selectedStatisticsAccount.TransactionDictionary != null)
                {
                    StatisticsVisibility = true;
                    endDate = startDate.AddMonths(-1);
                    updateAtAGlanceLabels();
                    GraphList = new List<string>
                    {
                        "Amount Spent Per Category",
                        "Amount Spent Over Time",
                        "Compare Accounts"
                    };
                    TransactionTypeFilterList = new List<string>();
                    TransactionTypeFilterList.AddRange(TransactionModel.TransactionTypesForFilters);
                }
                else InvalidAccountVisibility = true;
            }
            else InvalidAccountVisibility = true;
        }

        #region public vars

        public AccountModel SelectedStatisticsAccount
        {
            get { return selectedStatisticsAccount; }
            set
            {
                if (value == selectedStatisticsAccount) return;
                selectedStatisticsAccount = value;
                NotifyPropertyChanged();
                updateGraphs();
                updateAtAGlanceLabels();
            }
        }

        public ObservableCollection<AccountModel> ObcStatisticsAccounts
        {
            get { return obcStatisticsAccounts; }
            set
            {
                if (value == obcStatisticsAccounts) return;
                obcStatisticsAccounts = value;
                NotifyPropertyChanged();
            }
        }

        public List<string> GraphList { get; }

        public string SelectedGraph
        {
            get { return selectedGraph; }
            set
            {
                if (value == selectedGraph) return;
                selectedGraph = value;
                NotifyPropertyChanged();
                updateGraphs();
            }
        }

        public bool StatisticsVisibility
        {
            get { return statisticsVisibility; }
            set
            {
                if (value == statisticsVisibility) return;
                statisticsVisibility = value;
                NotifyPropertyChanged();
            }
        }

        public bool InvalidAccountVisibility
        {
            get { return invalidAccountVisibility; }
            set
            {
                if (value == invalidAccountVisibility) return;
                invalidAccountVisibility = value;
                NotifyPropertyChanged();
            }
        }

        public bool PieChartVisibility
        {
            get { return pieChartVisibility; }
            set
            {
                if (value == pieChartVisibility) return;
                pieChartVisibility = value;
                NotifyPropertyChanged();
            }
        }

        public bool LineChartTransactionsVisibility
        {
            get { return lineChartTransactionsVisibility; }
            set
            {
                if (value == lineChartTransactionsVisibility) return;
                lineChartTransactionsVisibility = value;
                NotifyPropertyChanged();
            }
        }

        public bool BarChartAccountsVisibility
        {
            get { return barChartAccountsVisibility; }
            set
            {
                if (value == barChartAccountsVisibility) return;
                barChartAccountsVisibility = value;
                NotifyPropertyChanged();
            }
        }

        public bool FilterControlsVisibility
        {
            get { return filterControlsVisibility; }
            set
            {
                if (value == filterControlsVisibility) return;
                filterControlsVisibility = value;
                NotifyPropertyChanged();
            }
        }

        public List<string> TransactionTypeFilterList { get; }

        public string SelectedTransactionTypeFilter
        {
            get { return selectedTransactionTypeFilter; }
            set
            {
                if (value == selectedTransactionTypeFilter) return;
                selectedTransactionTypeFilter = value;
                NotifyPropertyChanged();
                updateGraphs();
            }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                if (value == startDate) return;
                startDate = value;
                NotifyPropertyChanged();
                updateGraphs();
                updateAtAGlanceLabels();
            }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                if (value == endDate) return;
                endDate = value;
                NotifyPropertyChanged();
                updateGraphs();
                updateAtAGlanceLabels();
            }
        }

        public static List<AccountModel> StatisticsAccountsForCharts { get; private set; }

        public string AccountIncomeLabel
        {
            get { return accountIncomeLabel; }
            set
            {
                if (value == accountIncomeLabel) return;
                accountIncomeLabel = value;
                NotifyPropertyChanged();
            }
        }

        public string AccountExpenditureLabel
        {
            get { return accountExpenditureLabel; }
            set
            {
                if (value == accountExpenditureLabel) return;
                accountExpenditureLabel = value;
                NotifyPropertyChanged();
            }
        }

        public string CommonTransactionTypeLabel
        {
            get { return commonTransactionTypeLabel; }
            set
            {
                if (value == commonTransactionTypeLabel) return;
                commonTransactionTypeLabel = value;
                NotifyPropertyChanged();
            }
        }

        public string CommonTransactionDayLabel
        {
            get { return commonTransactionDayLabel; }
            set
            {
                if (value == commonTransactionDayLabel) return;
                commonTransactionDayLabel = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        private AccountModel selectAccount()
        {
            var account = AccountViewModel.SelectedAccountForStats;
            return account?.TransactionDictionary?.Values.Count > 0 ? account : obcStatisticsAccounts[0];
        }

        private void updateGraphs()
        {
            switch (selectedGraph)
            {
                case "Amount Spent Per Category":
                    TransactionsTypePieChartVm.UpdatePieChartValues(selectedStatisticsAccount, startDate, endDate);
                    PieChartVisibility = true;
                    LineChartTransactionsVisibility = false;
                    BarChartAccountsVisibility = false;
                    FilterControlsVisibility = false;
                    break;

                case "Amount Spent Over Time":
                    RecentTransactionsLineChartVm.UpdateRecentTransactionsLineChart(
                        selectedStatisticsAccount, startDate, endDate, selectedTransactionTypeFilter);
                    PieChartVisibility = false;
                    LineChartTransactionsVisibility = true;
                    BarChartAccountsVisibility = false;
                    FilterControlsVisibility = true;
                    break;

                case "Compare Accounts":
                    AccountsBarChartVm.UpdateAccountsBarChart(StatisticsAccountsForCharts, startDate, endDate,
                        selectedTransactionTypeFilter);
                    PieChartVisibility = false;
                    LineChartTransactionsVisibility = false;
                    BarChartAccountsVisibility = true;
                    FilterControlsVisibility = true;
                    break;

                default:
                    TransactionsTypePieChartVm.UpdatePieChartValues(selectedStatisticsAccount, startDate, endDate);
                    PieChartVisibility = true;
                    LineChartTransactionsVisibility = false;
                    barChartAccountsVisibility = false;
                    FilterControlsVisibility = false;
                    break;
            }
        }

        private void updateAtAGlanceLabels()
        {
            createIncomeAndExpenditureLabels();
            createCommonTransactionTypeLabel();
            createCommonTransactionDayLabel();
        }

        private void createIncomeAndExpenditureLabels()
        {
            if (startDate > endDate)
            {
                var incomeTransactionsMatchingDate = selectedStatisticsAccount.TransactionDictionary
                    .Values
                    .Where(x => x.Date <= startDate && x.Date >= endDate && x.Type == "Income").ToList();
                AccountIncomeLabel = incomeTransactionsMatchingDate.Select(x => x.Amount).Sum().ToString();

                var expenditureTransactionsMatchingDate = selectedStatisticsAccount
                    .TransactionDictionary.Values
                    .Where(x => x.Date <= startDate && x.Date >= endDate && x.Type != "Income").ToList();
                AccountExpenditureLabel = expenditureTransactionsMatchingDate.Select(x => x.Amount).Sum().ToString();
            }
            else
            {
                var incomeTransactionsMatchingDate = selectedStatisticsAccount.TransactionDictionary
                    .Values
                    .Where(x => x.Date >= startDate && x.Date <= endDate && x.Type == "Income").ToList();
                AccountIncomeLabel = incomeTransactionsMatchingDate.Select(x => x.Amount).Sum().ToString();

                var expenditureTransactionsMatchingDate = selectedStatisticsAccount
                    .TransactionDictionary.Values
                    .Where(x => x.Date >= startDate && x.Date <= endDate && x.Type != "Income").ToList();
                AccountExpenditureLabel = expenditureTransactionsMatchingDate.Select(x => x.Amount).Sum().ToString();
            }
        }

        private void createCommonTransactionTypeLabel()
        {
            if (startDate > endDate)
            {
                CommonTransactionTypeLabel = selectedStatisticsAccount.TransactionDictionary.Values
                    .Where(x => x.Date <= startDate && x.Date >= endDate)
                    .GroupBy(x => x.Type)
                    .OrderByDescending(i => i.Count())
                    .Select(s => s.Key)
                    .First();
            }
            else
            {
                CommonTransactionTypeLabel = selectedStatisticsAccount.TransactionDictionary.Values
                    .Where(x => x.Date >= startDate && x.Date <= endDate)
                    .GroupBy(x => x.Type)
                    .OrderByDescending(i => i.Count())
                    .Select(s => s.Key)
                    .First();
            }
 
        }

        private void createCommonTransactionDayLabel()
        {
            if (startDate > endDate)
            {
                CommonTransactionDayLabel = selectedStatisticsAccount.TransactionDictionary.Values
                                                .Where(x => x.Date <= startDate && x.Date >= endDate)
                                                .GroupBy(x => x.Date.DayOfWeek)
                                                .OrderByDescending(i => i.Count())
                                                .Select(s => s.Key)
                                                .First() + "s";
            }
            else
            {
                CommonTransactionDayLabel = selectedStatisticsAccount.TransactionDictionary.Values
                                                .Where(x => x.Date >= startDate && x.Date <= endDate)
                                                .GroupBy(x => x.Date.DayOfWeek)
                                                .OrderByDescending(i => i.Count())
                                                .Select(s => s.Key)
                                                .First() + "s";
            }
        }
    }
}