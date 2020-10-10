using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FYP.DataAccess;
using FYP.Helpers;
using FYP.Model;
using FYP.Services;

namespace FYP.ViewModel
{
    internal class AccountViewModel : BaseViewModel
    {
        private AccountModel account;
        private string accountName;
        private double? accountBalance, updateBalanceAmount;
        private AccountModel updateBalanceSelectedAcc;
        private bool canActOnAccount;
        private bool canTransfer;
        private bool canActOnTransaction;
        private bool activateSnackbar;
        private bool sortByAmount;
        private bool highestRadioButtonChecked = true;
        private string snackbarMessage;
        private AccountModel transferFrom;
        private AccountModel transferTo;
        private AccountModel selectedAccount;
        private TransactionModel selectedTransaction;
        private List<string> transactionTypes;
        private string selectedTransactionType;
        private string selectedTransactionTypeFilter;
        private string transactionDescription;

        private ObservableCollection<TransactionModel> obcTransactions;
        private ObservableCollection<AccountModel> obcAccounts;
        private DateTime selectedDate;


        public AccountViewModel()
        {
            string username = Environment.GetEnvironmentVariable("UserName");

            if (DataProvider.LoadAccountDictionary())
            {
                accountSnackbarMessage(4500, "Welcome back, " + username);
                SelectedAccount = AccountModel.AccountDictionary.ElementAt(0).Value;
            }
            else
            {
                accountSnackbarMessage(5000, "Hi, " + username + ". Data will be automatically saved when the app is closed.");
            }

            obcAccounts = new ObservableCollection<AccountModel>(AccountModel.AccountDictionary.Values);

            if (SelectedAccount != null)
            {
                if (SelectedAccount.TransactionDictionary != null)
                {
                    obcTransactions =
                        new ObservableCollection<TransactionModel>(SelectedAccount.TransactionDictionary.Values);
                }
                CanActOnTransaction =
                    Validator.TransactionIsSelectedAndHasIdCheck(obcTransactions.Count, SelectedTransaction);
            }

            createTransactionTypeFilterList();
            selectedDate = DateTime.Today;

            int accountCount = obcAccounts.Count;
            CanActOnAccount = Validator.AccountIsSelectedCheck(accountCount, SelectedAccount);
            CanTransfer = Validator.CanTransferBtnEnabledCheck(accountCount);

            CmdCreateAccount = new RelayCommand(createAccount, param => true);
            CmdCreateTestAccounts = new RelayCommand(createTestAccounts, param => true);
            CmdSaveTransfer = new RelayCommand(transferBetweenAccounts, param => true);
            CmdDeleteAccount = new RelayCommand(deleteAccount, param => true);
            CmdDeleteTransaction = new RelayCommand(deleteTransaction, param => true);
            CmdUpdateBalance = new RelayCommand(updateBalance, param => true);
        }

        #region  commands

        public ICommand CmdCreateAccount { get; set; }

        public ICommand CmdCreateTestAccounts { get; set; }

        public ICommand CmdSaveTransfer { get; set; }

        public ICommand CmdDeleteAccount { get; set; }

        public ICommand CmdUpdateBalance { get; set; }

        public ICommand CmdDeleteTransaction { get; set; }

        #endregion

        #region bindings      

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

        public ObservableCollection<TransactionModel> ObcTransactions
        {
            get { return obcTransactions; }
            set
            {
                if (value == obcTransactions) return;
                obcTransactions = value;
                NotifyPropertyChanged();
            }
        }

        public string AccountName
        {
            get { return accountName; }
            set
            {
                if (value == accountName) return;
                accountName = value;
                NotifyPropertyChanged();
            }
        }

        public double? AccountBalance
        {
            get { return accountBalance; }
            set
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (value == accountBalance) return;
                accountBalance = value;
                NotifyPropertyChanged();
            }
        }

        public bool CanTransfer
        {
            get { return canTransfer; }
            set
            {
                if (value == canTransfer) return;
                canTransfer = value;
                NotifyPropertyChanged();
            }
        }

        public bool CanActOnAccount
        {
            get { return canActOnAccount; }
            set
            {
                if (value == canActOnAccount) return;
                canActOnAccount = value;
                NotifyPropertyChanged();
            }
        }

        public bool CanActOnTransaction
        {
            get { return canActOnTransaction; }
            set
            {
                if (value == canActOnTransaction) return;
                canActOnTransaction = value;
                NotifyPropertyChanged();
            }
        }

        public AccountModel TransferFrom
        {
            get { return transferFrom; }
            set
            {
                if (value == transferFrom) return;
                transferFrom = value;
                NotifyPropertyChanged();
            }
        }

        public AccountModel TransferTo
        {
            get { return transferTo; }
            set
            {
                if (value == transferTo) return;
                transferTo = value;
                NotifyPropertyChanged();
            }
        }

        public static AccountModel SelectedAccountForStats { get; set; }

        public AccountModel SelectedAccount
        {
            get { return selectedAccount; }
            set
            {
                if (value == selectedAccount) return;
                selectedAccount = value;
                NotifyPropertyChanged();
                refreshTransactions();
                UpdateBalanceSelectedAcc = selectedAccount;
                SelectedAccountForStats = selectedAccount;
            }
        }

        public TransactionModel SelectedTransaction
        {
            get { return selectedTransaction; }
            set
            {
                if (value == selectedTransaction) return;
                selectedTransaction = value;
                NotifyPropertyChanged();
            }
        }

        public double? TransferAmount { get; set; }

        public bool ActivateSnackbar
        {
            get { return activateSnackbar; }
            set
            {
                if (value == activateSnackbar) return;
                activateSnackbar = value;
                NotifyPropertyChanged();
            }
        }

        public string SnackbarMessage
        {
            get { return snackbarMessage; }
            set
            {
                if (value == snackbarMessage) return;
                snackbarMessage = value;
                NotifyPropertyChanged();
            }
        }

        public double? UpdateBalanceAmount
        {
            get { return updateBalanceAmount; }
            set
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (value == updateBalanceAmount) return;
                updateBalanceAmount = value;
                NotifyPropertyChanged();
            }
        }

        public AccountModel UpdateBalanceSelectedAcc
        {
            get { return updateBalanceSelectedAcc; }
            set
            {
                if (value == updateBalanceSelectedAcc) return;
                updateBalanceSelectedAcc = value;
                NotifyPropertyChanged();
            }
        }

        public List<string> TransactionTypes
        {
            get { return transactionTypes; }
            set
            {
                if (value == transactionTypes) return;
                transactionTypes = value;
                NotifyPropertyChanged();
            }
        } //Won't need a setter if i'm never changing transaction types

        public List<string> TransactionTypesFilter { get; private set; }

        public string SelectedTransactionType
        {
            get { return selectedTransactionType; }
            set
            {
                if (value == selectedTransactionType) return;
                selectedTransactionType = value;
                NotifyPropertyChanged();
            }
        }

        public string SelectedTransactionTypeFilter
        {
            get { return selectedTransactionTypeFilter; }
            set
            {
                if (value == selectedTransactionTypeFilter) return;
                selectedTransactionTypeFilter = value;
                NotifyPropertyChanged();
                refreshTransactions();
            }
        }

        public string TransactionDescription
        {
            get { return transactionDescription; }
            set
            {
                if (value == transactionDescription) return;
                transactionDescription = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set
            {
                if (value == selectedDate) return;
                selectedDate = value;
                NotifyPropertyChanged();
            }
        }

        public bool HighestRadioButtonChecked
        {
            get { return highestRadioButtonChecked; }
            set
            {
                if (value == highestRadioButtonChecked) return;
                highestRadioButtonChecked = value;
                NotifyPropertyChanged();
                refreshTransactions();
            }
        }

        public bool SortByAmount
        {
            get { return sortByAmount; }
            set
            {
                if (value == sortByAmount) return;
                sortByAmount = value;
                NotifyPropertyChanged();
                refreshTransactions();
            }
        }

        #endregion

        private void createAccount(object o)
        {
            if (Validator.ValidAccountCreationName(accountName) && Validator.ValidDouble(accountBalance))
            {
                string name = accountName.ToUpper().Trim();
                string accountId = RandomGenerator.GenerateAccountId(name);

                account = new AccountModel(accountId, name, accountBalance);
                AccountModel.AccountDictionary.Add(accountId, account);
                obcAccounts.Add(account);

                SelectedAccount = account;
                AccountName = null;
                AccountBalance = null;
            }
            else
            {
                accountSnackbarMessage(4500, "Please enter a valid account name and balance.");
            }
            int accountCount = obcAccounts.Count;
            CanTransfer = Validator.CanTransferBtnEnabledCheck(accountCount);
            CanActOnAccount = Validator.AccountIsSelectedCheck(accountCount, SelectedAccount);
            refreshAll();
        }

        private void deleteAccount(object o)
        {
            if (!(CanActOnAccount = Validator.AccountIsSelectedCheck(obcAccounts.Count, SelectedAccount))) return;
            AccountModel.AccountDictionary.Remove(SelectedAccount.Id);
            ObcAccounts = new ObservableCollection<AccountModel>(AccountModel.AccountDictionary.Values);
            CanTransfer = Validator.CanTransferBtnEnabledCheck(obcAccounts.Count);
            if (ObcAccounts.Count >= 1)
            {
                SelectedAccount = ObcAccounts[0];
                CanActOnAccount = Validator.AccountIsSelectedCheck(obcAccounts.Count, SelectedAccount);
                CanActOnTransaction =
                    Validator.TransactionIsSelectedAndHasIdCheck(obcTransactions.Count, SelectedTransaction);
            }
            else
            {
                ObcTransactions = null;
                CanActOnAccount = false;
                CanActOnTransaction = false;
            }
        }

        private void deleteTransaction(object o)
        {
            // ReSharper disable once AssignmentInConditionalExpression
            if (CanActOnTransaction =
                Validator.TransactionIsSelectedAndHasIdCheck(obcTransactions.Count, SelectedTransaction)
                && Validator.AccountIsSelectedCheck(obcAccounts.Count, SelectedAccount))
            {
                account = AccountModel.AccountDictionary[selectedAccount.Id];
                AccountModel.UpdateBalance(account, -selectedTransaction.Amount, selectedTransaction.Type);
                account.DeleteTransaction(selectedTransaction.Id);
                refreshAll();
            }
            else
            {
                accountSnackbarMessage(4500, "Please choose a transaction to remove");
            }
        }

        private void updateBalance(object o)
        {
            if (Validator.ValidDouble(updateBalanceAmount))
            {
                account = AccountModel.AccountDictionary[updateBalanceSelectedAcc.Id];
                AccountModel.UpdateBalance(account, updateBalanceAmount, selectedTransactionType);
                account.AddTransaction(selectedDate, transactionDescription, updateBalanceAmount,
                    selectedTransactionType);

                UpdateBalanceSelectedAcc = null;
                UpdateBalanceSelectedAcc = account;
                UpdateBalanceAmount = null;
                refreshAll();
                SelectedAccount = updateBalanceSelectedAcc;
            }
            else
            {
                accountSnackbarMessage(4500, "Please enter a valid number");
            }
        }

        private void transferBetweenAccounts(object o)
        {
            if (transferFrom != null && transferTo != null)
            {
                AccountModel account1 = AccountModel.AccountDictionary[transferFrom.Id];
                AccountModel account2 = AccountModel.AccountDictionary[transferTo.Id];
                if (Validator.AccountsShouldNotMatchCheck(account1, account2))
                    if (Validator.ValidDouble(TransferAmount))
                    {
                        {
                            double? newFromAccountBalance;
                            double? newToAccountBalance;
                            AccountModel.TransferBetweenAccounts(account1, account2, TransferAmount,
                                out newFromAccountBalance, out newToAccountBalance);
                            account1.AddTransaction(DateTime.Today, "Transfer to: " + account2.Name, TransferAmount,
                                "Transfer");
                            account2.AddTransaction(DateTime.Today, "Transfer from: " + account1.Name, TransferAmount,
                                "Transfer");


                            //Update Databinds
                            account1.Balance = newFromAccountBalance;
                            account2.Balance = newToAccountBalance;
                            ColourManager.BalanceColourCheck(account1, account2);
                            TransferFrom = new AccountModel(transferFrom.Id, transferFrom.Name, newFromAccountBalance);
                            TransferTo = new AccountModel(transferTo.Id, transferTo.Name, newToAccountBalance);
                            refreshAll();
                        }
                    }
                    else
                    {
                        accountSnackbarMessage(4500, "Please enter a valid number.");
                    }
                else
                {
                    accountSnackbarMessage(6000, "You can't transfer to the same account.");
                }
            } else { accountSnackbarMessage(6000, "Please select two accounts to make a transfer between."); }
           
        }

        /// <summary>
        /// Displays message in snackbar at bottom of window
        /// </summary>
        /// <param name="displayTimeInMs">Amount of time to display message for in millieseconds</param>
        /// <param name="message">Message to display</param>
        private async void accountSnackbarMessage(int displayTimeInMs, string message)
        {
            SnackbarMessage = message;
            ActivateSnackbar = true;
            await Task.Delay(displayTimeInMs);
            ActivateSnackbar = false;
        }

        private void refreshAll()
        {
            ColourManager.BalanceColourCheck(account);
            ObcAccounts = new ObservableCollection<AccountModel>(AccountModel.AccountDictionary.Values);
            refreshTransactions();
        }

        private void refreshTransactions()
        {
            if (SelectedAccount == null) return;
            if (SelectedAccount.TransactionDictionary != null)
            {
                createFilteredTransactionsCollection();
            }
            else
            {
                ObcTransactions = new ObservableCollection<TransactionModel>
                {
                    new TransactionModel(null, DateTime.Today, "No transactions", null, "")
                };
            }
            if (ObcTransactions.Count >= 1)
            {
                SelectedTransaction = ObcTransactions[0];
            }
            CanActOnTransaction =
                Validator.TransactionIsSelectedAndHasIdCheck(obcTransactions.Count, SelectedTransaction);
        }

        private void createTransactionTypeFilterList()
        {
            TransactionTypesFilter = new List<string> {"No filter", "Ex. Income"};
            TransactionTypesFilter.AddRange(TransactionModel.TransactionTypes);
            transactionTypes = TransactionModel.TransactionTypes;
        }

        private void createFilteredTransactionsCollection()
        {
            ObcTransactions = new ObservableCollection<TransactionModel>
                (SelectedAccount.TransactionDictionary.Values.Where(meetsTransactionTypeFilterRequirements));
            if (SortByAmount)
            {
                if (HighestRadioButtonChecked)
                {
                    ObcTransactions = new ObservableCollection<TransactionModel>
                    (from i in obcTransactions
                        orderby i.Amount descending
                        select i);
                }
                else
                {
                    ObcTransactions = new ObservableCollection<TransactionModel>
                    (from i in obcTransactions
                        orderby i.Amount
                        select i);
                }
            }
            else
            {
                ObcTransactions = new ObservableCollection<TransactionModel>
                (from i in obcTransactions
                    orderby i.Date descending
                    select i);
            }
        }

        private bool meetsTransactionTypeFilterRequirements(TransactionModel item)
        {
            if (SelectedTransactionTypeFilter == "No filter") return true;
            if (SelectedTransactionTypeFilter != "Ex. Income") return item.Type == SelectedTransactionTypeFilter;
            return item.Type != "Income";
        }

        /// <summary>
        /// Create test accounts and transactions.
        /// </summary>
        private void createTestAccounts(object o)
        {
            const int numberOfTestAccounts = 8;
            const int numberOfTestTransactions = 320;
            const int numberOfDays = 365;


            for (int i = 0; i < numberOfTestAccounts; i++)
            {
                if (obcAccounts.Count < 1)
                {
                    accountName = "Dit Lexaragez";
                }
                else accountName = RandomGenerator.GetRandomStringFromPath().Remove(8) + " Test";
                accountBalance = 0;

                createAccount(o);

                for (int k = 0; k < numberOfTestTransactions; k++)
                {
                    string randomTransactionType =
                        TransactionModel.TransactionTypes[
                            RandomGenerator.GetRandomIntMaxValue(TransactionModel.TransactionTypes.Count)];
                    int randomTransactionAmount = RandomGenerator.GetRandomIntMaxValue(999);
                    if (randomTransactionType != "Income")
                    {
                        account.Balance -= randomTransactionAmount;
                    }
                    else
                    {
                        randomTransactionAmount *= 9; //Make the Income transactions bigger
                        account.Balance += randomTransactionAmount;
                    }
                    account.AddTransaction(RandomGenerator.GenerateRandomDate(numberOfDays), "Test transaction " + k,
                        randomTransactionAmount, randomTransactionType);
                    ColourManager.BalanceColourCheck(account);
                }
                SelectedAccount = account;
            }
        }
    }
}