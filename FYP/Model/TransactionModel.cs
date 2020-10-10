using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace FYP.Model
{
    public class TransactionModel
    {
        public TransactionModel(string id, DateTime date, string description = "Transaction", double? amount = 0,
            string type = "Other")
        {
            Id = id;
            Date = date;
            Description = description;
            Amount = amount;
            Type = type;

            AmountColour = type != "Income" ? Brushes.Black : Brushes.MediumSeaGreen;
        }

        public string Id { get; set; }

        public double? Amount { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public string Type { get; set; }

        public Brush AmountColour { get; set; }

        public static List<string> TransactionTypes { get; } = new List<string>
        {
            "Income",
            "Food",
            "Travel",
            "Clothes",
            "Entertainment",
            "Bills",
            "Home",
            "Transfer",
            "Work",
            "Other"
        };

        public static List<string> TransactionTypesForFilters { get; } = new List<string>
        {
            "No filter",
            "Income",
            "Food",
            "Travel",
            "Clothes",
            "Entertainment",
            "Bills",
            "Home",
            "Transfer",
            "Work",
            "Other"
        };
    }
}