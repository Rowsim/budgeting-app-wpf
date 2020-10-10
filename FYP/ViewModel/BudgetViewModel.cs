using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using FYP.Model;

namespace FYP.ViewModel
{
    internal class BudgetViewModel : BaseViewModel
    {
        private double? income, expenditure, savings, dailySpendAmount, weeklySpendAmount, monthlySpendAmount;
        private string savingPercentage;
        private ObservableCollection<AccountModel> obcAccounts;
        private AccountModel selectedAccount;

        public BudgetViewModel()
        {
            obcAccounts = new ObservableCollection<AccountModel>(GetAccountsWithTransactions());
            SavingPercentage = "?";
        }

        public string SavingPercentage
        {
            get { return savingPercentage; }
            set
            {
                if (value == savingPercentage) return;
                savingPercentage = value;
                NotifyPropertyChanged();
            }
        }

        public double? Income
        {
            get { return income; }
            set
            {
                if (value == income) return;
                income = value;
                NotifyPropertyChanged();
                updateBudgetAmounts();
            }
        }

        public double? Expenditure
        {
            get { return expenditure; }
            set
            {
                if (value == expenditure) return;
                expenditure = value;
                NotifyPropertyChanged();
                updateBudgetAmounts();
            }
        }

        public double? Savings
        {
            get { return savings; }
            set
            {
                if (value == savings) return;
                savings = value;
                NotifyPropertyChanged();
                updateBudgetAmounts();
            }
        }

        public double? MonthlySpendAmount
        {
            get { return monthlySpendAmount; }
            set
            {
                if (value == monthlySpendAmount) return;
                monthlySpendAmount = value;
                NotifyPropertyChanged();
            }
        }


        public double? WeeklySpendAmount
        {
            get { return weeklySpendAmount; }
            set
            {
                if (value == weeklySpendAmount) return;
                weeklySpendAmount = value;
                NotifyPropertyChanged();
            }
        }

        public double? DailySpendAmount
        {
            get { return dailySpendAmount; }
            set
            {
                if (value == dailySpendAmount) return;
                dailySpendAmount = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<AccountModel> ObcAccounts
        {
            get { return obcAccounts; }
            set
            {
                if (value == obcAccounts) return;
                obcAccounts = value;
                NotifyPropertyChanged();
            }
        }

        public AccountModel SelectedAccount
        {
            get { return selectedAccount; }
            set
            {
                if (value == selectedAccount) return;
                selectedAccount = value;
                NotifyPropertyChanged();
                getAverageIncomeAndExpenditureForAccountInPastMonth();
            }
        }

        private void updateBudgetAmounts()
        {
            income = nullToZero(income);
            expenditure = nullToZero(expenditure);
            savings = nullToZero(savings);

            if (savings == 0 || income == 0)
            {
                SavingPercentage = "% 0";
            }
            else
            {
                SavingPercentage = "% " + Math.Round((double)(savings / (income - expenditure)) * 100);
                SavingPercentage = SavingPercentage.Length > 6
                    ? SavingPercentage.Remove(6)
                    : SavingPercentage;
            }

            MonthlySpendAmount = income - expenditure - savings;
            WeeklySpendAmount = MonthlySpendAmount / 4;
            DailySpendAmount = WeeklySpendAmount / 7;
        }

        private void getAverageIncomeAndExpenditureForAccountInPastMonth()
        {
            updateBudgetAmounts();
            DateTime dateOneMonthAgo = DateTime.Now.AddMonths(-1);

            var incomeTransactions = selectedAccount.TransactionDictionary
                .Values
                .Where(x => x.Type == "Income" && x.Date < DateTime.Now && x.Date > dateOneMonthAgo).ToList();
            Income = incomeTransactions.Count > 0
                ? incomeTransactions.Select(x => x.Amount).Sum()
                : 0;

            var expenditureTransactions = selectedAccount.TransactionDictionary
                .Values
                .Where(x => x.Type != "Income" && x.Type != "Transfers" && x.Date < DateTime.Now &&
                            x.Date > dateOneMonthAgo).ToList();
            Expenditure = expenditureTransactions.Count > 0
                ? expenditureTransactions.Select(x => x.Amount).Sum()
                : 0;
        }

        private double? nullToZero(double? value)
        {
            if (value == null)
            {
                value = 0;
                return value;
            }
            return value;
        }
    }
}
