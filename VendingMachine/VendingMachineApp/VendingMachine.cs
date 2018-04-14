using System.Collections.Generic;

namespace VendingMachineApp
{
    public class VendingMachine
    {
        private readonly Dictionary<string, Item> _items = new Dictionary<string, Item>();

        public void Initialize(Dictionary<string, Item> items)
        {
            _items.Clear();
            foreach (var item in items)
            {
                _items.Add(item.Key, item.Value);
            }
        }

        public string Message { get; private set; }
        public string SelectedItem { get; private set; }
        public void SelectItem(string itemId)
        {
            if (_items.ContainsKey(itemId))
            {
                SelectedItem = itemId;
                Message = null;
            }
            else
            {
                Message = "Please select a valid item.";
            }
        }

        public decimal TotalDeposit { get; private set; }
        public void Deposit(decimal amount)
        {
            TotalDeposit += amount;
        }

        public DispensedItem Dispense()
        {
            if (string.IsNullOrEmpty(SelectedItem))
            {
                Message = "Please select a valid item.";
                return DispensedItem.Empty;
            }

            var item = _items[SelectedItem];
            if (item.Price > TotalDeposit)
            {
                Message = $"Please deposit another ${item.Price - TotalDeposit} to get {item.Id}.";
                return DispensedItem.Empty;
            }

            var change = TotalDeposit - item.Price;
            TotalDeposit = 0;
            SelectedItem = null;
            Message = $"{item.Id} is dispensed. Enjoy!";
            return new DispensedItem
            {
                ItemId = item.Id,
                Change = change
            };
        }
    }
}
