using FYP.Model;
using System.Windows.Input;
using FYP.DataAccess;
using FYP.Services;
using FYP.ViewModel;
using System.Windows;
using System;

namespace FYP.Viewmodel
{
    internal class MainWindowViewModel : BaseViewModel
    {
        private int pageItemSelectedIndex;

        private bool orangeThemeSelected,
            blueThemeSelected,
            indigoThemeSelected,
            greenThemeSelected,
            brownThemeSelected;

        public MainWindowViewModel()
        {
            OnCloseWindowCommand = new RelayCommand(saveOnClose, param => true);

            PageItems = new[]
            {
                new PageItemModel("ACCOUNTS", "Bank", new AccountView()),
                new PageItemModel("STATISTICS", "ChartLine", new StatisticsView()),
                new PageItemModel("BUDGET", "Calculator", new BudgetView()),
            };
        }

        public PageItemModel[] PageItems { get; }

        public int PageItemSelectedIndex
        {
            get { return pageItemSelectedIndex; }
            set
            {
                if (value == pageItemSelectedIndex) return;
                pageItemSelectedIndex = value;
                NotifyPropertyChanged();
                reloadViews();
            }
        }

        public bool OrangeThemeSelected
        {
            get { return orangeThemeSelected; }
            set
            {
                if (value == orangeThemeSelected) return;
                orangeThemeSelected = value;
                NotifyPropertyChanged();
                changeThemeColour();
                disableThemes(ref orangeThemeSelected);
            }
        }

        public bool BlueThemeSelected
        {
            get { return blueThemeSelected; }
            set
            {
                if (value == blueThemeSelected) return;
                blueThemeSelected = value;
                NotifyPropertyChanged();
                changeThemeColour("bluegrey");
                disableThemes(ref blueThemeSelected);
            }
        }

        public bool IndigoThemeSelected 
        {
            get { return indigoThemeSelected; }
            set
            {
                if (value == indigoThemeSelected) return;
                indigoThemeSelected = value;
                NotifyPropertyChanged();
                changeThemeColour("indigo");
                disableThemes(ref indigoThemeSelected);
            }
        }

        public bool GreenThemeSelected
        {
            get { return greenThemeSelected; }
            set
            {
                if (value == greenThemeSelected) return;
                greenThemeSelected = value;
                NotifyPropertyChanged();
                changeThemeColour("green");
                disableThemes(ref greenThemeSelected);
            }
        }

        public bool BrownThemeSelected
        {
            get { return brownThemeSelected; }
            set
            {
                if (value == brownThemeSelected) return;
                brownThemeSelected = value;
                NotifyPropertyChanged();
                changeThemeColour("brown");
                disableThemes(ref brownThemeSelected);
            }
        }

        public ICommand OnCloseWindowCommand { get; set; }       

        private void saveOnClose(object o)
        {
            DataProvider.SaveAccountDictionary();
        }

        private void reloadViews()
        {
            PageItems[1].Content = new StatisticsView();
            PageItems[2].Content = new BudgetView();
        }

        private void changeThemeColour(string colour = "deeporange")
        {
            Application.Current.Resources.MergedDictionaries.RemoveAt(3);

            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source =
                    new Uri("pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor."+ colour +".xaml",
                        UriKind.Absolute) });
        }

        private void disableThemes(ref bool selectedTheme)
        {
            orangeThemeSelected = false;
            blueThemeSelected = false;
            indigoThemeSelected = false;
            greenThemeSelected = false;
            brownThemeSelected = false;
            selectedTheme = true;
        }
    }
}