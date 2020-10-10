using FYP.Model;
using System.Windows.Media;

namespace FYP.Helpers
{
    public static class ColourManager
    {
        internal static void BalanceColourCheck(AccountModel account)
        {
            if (account != null)
            account.BalanceColour = account.Balance < 0 ? Brushes.Salmon : Brushes.Black;
        }

        internal static void BalanceColourCheck(AccountModel account, AccountModel account2)
        {
            BalanceColourCheck(account);
            BalanceColourCheck(account2);
        }
    }
}