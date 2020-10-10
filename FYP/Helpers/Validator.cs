using FYP.Model;

namespace FYP.Helpers
{
    public static class Validator
    {
        internal static bool CanTransferBtnEnabledCheck(int accountCount)
        {
            return accountCount >= 2;
        }

        internal static bool AccountIsSelectedCheck(int accountCount, AccountModel selectedAccount)
        {
            return accountCount >= 1 && selectedAccount != null;
        }

        internal static bool TransactionIsSelectedAndHasIdCheck(int transactionCount,
            TransactionModel selectedTransaction)
        {
            return transactionCount >= 1 && selectedTransaction?.Id != null;
        }

        internal static bool AccountsShouldNotMatchCheck(AccountModel account1, AccountModel account2)
        {
            return account1 != account2;
        }

        internal static bool ValidDouble(double? value)
        {
            return value != null;
        }

        internal static bool ValidAccountCreationName(string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}