using System.Linq;
using FYP.Model;
using FYP.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FYPUnitTests
{
    [TestClass]
    public class TransactionTest
    {
        private readonly AccountViewModel testSubject = new AccountViewModel();

        [TestInitialize]
        public void BeforeEachTest()
        {
            AccountModel.AccountDictionary.Clear();
            testSubject.AccountName = "test";
            testSubject.AccountBalance = 0;
            testSubject.CmdCreateAccount.Execute(null);

            testSubject.AccountName = "test2";
            testSubject.AccountBalance = 0;
            testSubject.CmdCreateAccount.Execute(null);

            testSubject.TransferAmount = 200;
        }

        [TestMethod]
        public void ShouldAddTransactionWithCorrectDetails()
        {
            //Given
            testSubject.SelectedAccount = AccountModel.AccountDictionary.First().Value;
            testSubject.UpdateBalanceAmount = 200;
            testSubject.TransactionDescription = "Test";
            testSubject.SelectedTransactionType = "TestType";


            //When
            testSubject.CmdUpdateBalance.Execute(null);

            //Then
            Assert.IsTrue(AccountModel.AccountDictionary.First().Value
                .TransactionDictionary.First().Value.Amount == 200);
            Assert.IsTrue(AccountModel.AccountDictionary.First().Value
                              .TransactionDictionary.First().Value.Description == "Test");
            Assert.IsTrue(AccountModel.AccountDictionary.First().Value
                              .TransactionDictionary.First().Value.Type == "TestType");
        }

        [TestMethod]
        public void ShouldNotAddTransactionWithInvalidAmount()
        {
            //Given
            testSubject.UpdateBalanceAmount = null;

            //When
            testSubject.CmdUpdateBalance.Execute(null);

            //Then
            Assert.IsNull(AccountModel.AccountDictionary.First().Value
                              .TransactionDictionary);
        }

        [TestMethod]
        public void ShouldNotMakeTransferBetweenTheSameAccount()
        {
            //Given
            testSubject.TransferFrom = AccountModel.AccountDictionary.First().Value;
            testSubject.TransferTo = AccountModel.AccountDictionary.First().Value;
                 
            //When
            testSubject.CmdSaveTransfer.Execute(null);

            //Then
            Assert.IsTrue(AccountModel.AccountDictionary.First().Value.Balance == 0);
        }

        [TestMethod]
        public void ShouldNotMakeTransferWhenNoAccountIsSelected()
        {
            //Given
            testSubject.TransferFrom = AccountModel.AccountDictionary.First().Value;
            testSubject.TransferTo = null;

            //When
            testSubject.CmdSaveTransfer.Execute(null);

            //Then
            Assert.IsTrue(AccountModel.AccountDictionary.First().Value.Balance == 0);
        }

        [TestMethod]
        public void ShouldNotMakeTransferWithInvalidAmount()
        {
            //Given
            testSubject.TransferFrom = AccountModel.AccountDictionary.First().Value;
            testSubject.TransferTo = AccountModel.AccountDictionary.Last().Value;
            testSubject.TransferAmount = null;

            //When
            testSubject.CmdSaveTransfer.Execute(null);

            //Then
            Assert.IsTrue(AccountModel.AccountDictionary.First().Value.Balance == 0);
        }

        [TestMethod]
        public void ShouldTransferCorrectAmountBetweenAccounts()
        {
            //Given
            testSubject.TransferFrom = AccountModel.AccountDictionary.First().Value;
            testSubject.TransferTo = AccountModel.AccountDictionary.Last().Value;

            //When
            testSubject.CmdSaveTransfer.Execute(null);

            //Then
            Assert.IsTrue(AccountModel.AccountDictionary.First().Value.Balance == -200);
            Assert.IsTrue(AccountModel.AccountDictionary.Last().Value.Balance == 200);
        }
    }
}
