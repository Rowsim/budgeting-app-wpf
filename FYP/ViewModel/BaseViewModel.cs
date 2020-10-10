using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FYP.Model;

namespace FYP.ViewModel
{
    internal class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static event PropertyChangedEventHandler PropertyChangedStatic;

        public List<AccountModel> GetAccountsWithTransactions()
        {
            return AccountModel.AccountDictionary.Values.Where(x => x.TransactionDictionary != null
                                                                    && x.TransactionDictionary.Values.Count > 0)
                .ToList();
        }

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static void NotifyPropertyChangedStatic([CallerMemberName] string propertyName = "")
        {
            PropertyChangedStatic?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }
    }
}