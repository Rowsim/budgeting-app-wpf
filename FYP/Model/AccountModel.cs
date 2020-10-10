using System;
using System.Collections.Generic;
using System.Windows.Media;
using FYP.Helpers;

namespace FYP.Model
{
    internal class AccountModel
    {
        public AccountModel(string id = "DIT123456", string name = "Dit", double? balance = -1,
            Dictionary<string, TransactionModel> transactionDictionary = null)
        {
            Id = id;
            Name = name;
            Balance = balance;
            TransactionDictionary = transactionDictionary;
            BalanceColour = balance < 0 ? Brushes.Salmon : Brushes.Black;
        }

        public string Name { get; set; }

        public double? Balance { get; set; }

        public string Id { get; set; }

        public Brush BalanceColour { get; set; }

        public Dictionary<string, TransactionModel> TransactionDictionary { get; set; }

        internal static Dictionary<string, AccountModel> AccountDictionary { get; set; }

        internal void AddTransaction(DateTime date, string description, double? amount, string type)
        {
            string transactionId = AccountDictionary[Id].Name + "TR" + RandomGenerator.GetRandomIntMaxValue();
            TransactionModel transaction = new TransactionModel(transactionId, date, description, amount, type);
            if (AccountDictionary[Id].TransactionDictionary != null)
            {
                AccountDictionary[Id].TransactionDictionary.Add(transactionId, transaction);
            }
            else
            {
                AccountDictionary[Id].TransactionDictionary =
                    new Dictionary<string, TransactionModel> {{transactionId, transaction}};
            }
        }

        internal void DeleteTransaction(string transactionId)
        {
            TransactionDictionary.Remove(transactionId);
        }

        internal static void UpdateBalance(AccountModel account, double? amount, string type)
        {
            if (type == "Income")
            {
                AccountDictionary[account.Id].Balance += amount;
            }
            else
            {
                AccountDictionary[account.Id].Balance -= amount;
            }
        }

        /// <summary>
        /// Logic for transfering money between accounts
        /// </summary>
        /// <param name="account1">Account to transfer from</param>
        /// <param name="account2">Account to transfer to</param>
        /// <param name="transferAmount">Amount to transfer</param>
        /// <param name="account1NewBalance">New balance of account money was transfered from</param>
        /// <param name="account2NewBalance">New balance of account money was transfered to</param>
        internal static void TransferBetweenAccounts(AccountModel account1, AccountModel account2,
            double? transferAmount,
            out double? account1NewBalance, out double? account2NewBalance)
        {
            account1NewBalance = account1.Balance - transferAmount;
            account2NewBalance = account2.Balance + transferAmount;
        }
    }
}