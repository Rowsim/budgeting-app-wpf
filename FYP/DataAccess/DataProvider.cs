using System.Collections.Generic;
using System.IO;
using FYP.Model;
using Newtonsoft.Json;

namespace FYP.DataAccess
{
    public class DataProvider
    {
        private const string AccountFilePath = "accounts.txt";

        public static bool SaveAccountDictionary()
        {
            try
            {
                var accountDictionaryAsJson = JsonConvert.SerializeObject(AccountModel.AccountDictionary);
                var encryptedOutput = accountDictionaryAsJson.Encrypt();
                File.WriteAllText(AccountFilePath, encryptedOutput);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool LoadAccountDictionary()
        {
            AccountModel.AccountDictionary = new Dictionary<string, AccountModel>();

            if (File.Exists(AccountFilePath))
            {
                var accountDictionarAsJson = File.ReadAllText(AccountFilePath).Decrypt();

                if (accountDictionarAsJson == null || accountDictionarAsJson.Length <= 2) return false;
                AccountModel.AccountDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, AccountModel>>(accountDictionarAsJson);
                return true;
            }
            return false;
        }
    }
}