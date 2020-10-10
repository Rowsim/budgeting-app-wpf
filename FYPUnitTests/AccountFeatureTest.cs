using System.Linq;
using FYP.Model;
using FYP.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FYPUnitTests
{
    [TestClass]
    public class AccountFeatureTest
    {
        private readonly AccountViewModel testSubject = new AccountViewModel();
        
        [TestInitialize]
        public void BeforeEachTest()
        {
            AccountModel.AccountDictionary.Clear();
            testSubject.AccountName = "test";
            testSubject.AccountBalance = 0;
        }

        [TestMethod]
        public void ShouldNotCreateNewAccountWithEmptyName()
        {
            //Given
            const string someBadAccountName = "";

            //When
            testSubject.AccountName = someBadAccountName;
            testSubject.CmdCreateAccount.Execute(null);

            //Then
            Assert.IsTrue(AccountModel.AccountDictionary.Count == 0);
        }

        [TestMethod]
        public void ShouldCreateNewAccountWithCorrectName()
        {
            //Given
            const string expectedAccountName = "SOMEACCOUNTNAME";

            //When
            testSubject.AccountName = expectedAccountName;
            testSubject.CmdCreateAccount.Execute(null);

            //Then
            Assert.IsTrue(AccountModel.AccountDictionary.First().Value.Name == expectedAccountName);
        }

        [TestMethod]
        public void ShouldNotCreateAccountWithNullBalance()
        {
            //When
            testSubject.AccountBalance = null;
            testSubject.CmdCreateAccount.Execute(null);

            //Then
            Assert.IsTrue(AccountModel.AccountDictionary.Count == 0);
        }

        [TestMethod]
        public void ShouldCreateAccountWithCorrectBalance()
        {
            //Given
            double? expectedBalance = 1234.50;

            //When
            testSubject.AccountBalance = expectedBalance;
            testSubject.CmdCreateAccount.Execute(null);

            //Then
            Assert.IsTrue(AccountModel.AccountDictionary.First().Value.Balance == expectedBalance);
        }

        [TestMethod]
        public void ShouldDeleteSelectedAccount()
        {
            //Given
            testSubject.AccountName = "JOHN";
            testSubject.CmdCreateAccount.Execute(null);

            testSubject.AccountName = "DOE";
            testSubject.AccountBalance = 0;
            testSubject.CmdCreateAccount.Execute(null);

            testSubject.AccountName = "SMITH";
            testSubject.AccountBalance = 0;
            testSubject.CmdCreateAccount.Execute(null);

            //When
            testSubject.SelectedAccount = AccountModel.AccountDictionary.Values.ElementAt(0);
            testSubject.CmdDeleteAccount.Execute(null);

            //Then
            Assert.IsTrue(AccountModel.AccountDictionary.First().Value.Name == "DOE");
        }

        [TestMethod]
        public void ShouldUpdateAccountBalanceWithCorrectValue()
        {
            //Given
            double? initialBalance = 4000;
            double? amountToUpdateBalanceBy = 995;
            double? expectedNewBalance = initialBalance - amountToUpdateBalanceBy;

            testSubject.AccountBalance = initialBalance;
            testSubject.CmdCreateAccount.Execute(null);

            //When
            testSubject.UpdateBalanceAmount = amountToUpdateBalanceBy;
            testSubject.CmdUpdateBalance.Execute(null);

            //Then
            Assert.IsTrue(AccountModel.AccountDictionary.First().Value.Balance == expectedNewBalance);
        }

        [TestMethod]
        public void ShouldNotUpdateBalanceWhenNull()
        {
            //Given
            double? expectedBalance = 10;
            testSubject.AccountBalance = expectedBalance;
            testSubject.CmdCreateAccount.Execute(null);

            //When
            testSubject.UpdateBalanceAmount = null;
            testSubject.CmdUpdateBalance.Execute(null);

            //Then
            Assert.IsTrue(AccountModel.AccountDictionary.First().Value.Balance == expectedBalance);
        }

        [TestMethod]
        public void ShouldTransferCorrectAmountBetweenAcounts()
        {
            //Given
            testSubject.AccountBalance = 50;
            testSubject.CmdCreateAccount.Execute(null);
            testSubject.AccountBalance = 10;
            testSubject.AccountName = "second";
            testSubject.CmdCreateAccount.Execute(null);

            testSubject.TransferFrom = AccountModel.AccountDictionary.First().Value;
            testSubject.TransferTo = AccountModel.AccountDictionary.Last().Value;
            testSubject.TransferAmount = 50;
            
            //When
            testSubject.CmdSaveTransfer.Execute(null);

            //Then
            Assert.IsTrue(AccountModel.AccountDictionary.Last().Value.Balance == 60);
            Assert.IsTrue(AccountModel.AccountDictionary.First().Value.Balance == 0);
        }
    }
}
