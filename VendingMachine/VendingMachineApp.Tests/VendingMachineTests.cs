using System.Linq;
using NUnit.Framework;

namespace VendingMachineApp.Tests
{
    [TestFixture]
    public class VendingMachineTests
    {
        private VendingMachine _machine;
        private Item _item1, _item2;

        [SetUp]
        public void Setup()
        {
            _machine = new VendingMachine();
            _item1 = new Item {Id = "snack1", Price = 2, Quantity = 100};
            _item2 = new Item {Id = "snack2", Price = 2.5m, Quantity = 100};
            _machine.Initialize(new[]{_item1, _item2}.ToDictionary(k => k.Id));
        }

        [Test]
        public void Should_Dispense_Item_Only_When_Enough_Money_Is_Deposited()
        {           
            _machine.SelectItem(_item1.Id);
            _machine.Deposit(1);
            Assert.That(_machine.Dispense(), Is.EqualTo(DispensedItem.Empty));
            
            _machine.Deposit(1);
            var dispensed = _machine.Dispense();
            Assert.That(dispensed.ItemId, Is.EqualTo(_item1.Id));
        }

        [Test]
        public void Should_Not_Dispense_Item_When_Not_Enough_Money_Is_Deposited()
        {           
            _machine.SelectItem(_item1.Id);
            _machine.Deposit(1);
            Assert.That(_machine.Dispense(), Is.EqualTo(DispensedItem.Empty));
            Assert.That(_machine.Message, Is.EqualTo($"Please deposit another $1 to get {_item1.Id}."));
        }

        [Test]
        public void Should_Dispense_Item_When_Enough_Money_Is_Deposited()
        {           
            _machine.SelectItem(_item1.Id);
            _machine.Deposit(2);

            var dispensed = _machine.Dispense();
            Assert.That(dispensed.ItemId, Is.EqualTo(_item1.Id));
        }

        [Test]
        public void Should_Dispense_Item_And_Change_When_Deposited_Money_Is_More_Than_Item_Price()
        {           
            _machine.SelectItem(_item1.Id);
            _machine.Deposit(3);

            var result = _machine.Dispense();
            Assert.That(result.ItemId, Is.EqualTo(_item1.Id));
            Assert.That(result.Change, Is.EqualTo(1));
            Assert.That(_machine.Message, Is.EqualTo($"{_item1.Id} is dispensed. Enjoy!"));
        }

        [Test]
        public void Should_Be_Able_To_Change_Selection()
        {
            _machine.SelectItem(_item2.Id);
            _machine.Deposit(2);
            _machine.SelectItem(_item1.Id);

            var dispensedItem = _machine.Dispense();
            Assert.That(dispensedItem.ItemId, Is.EqualTo(_item1.Id));
            Assert.That(dispensedItem.Change, Is.EqualTo(0m));
            Assert.That(_machine.Message, Is.EqualTo($"{_item1.Id} is dispensed. Enjoy!"));
        }

        [Test]
        public void Should_Not_Dispense_Anything_When_No_Item_Is_Selected()
        {            
            _machine.Deposit(3);
            var dispensedItem = _machine.Dispense();
            Assert.That(dispensedItem, Is.EqualTo(DispensedItem.Empty));
            Assert.That(_machine.Message, Is.EqualTo("Please select a valid item."));
        }

        [Test]
        public void Should_Not_Be_Able_To_Select_Invalid_Item()
        {            
            _machine.SelectItem("snack3");
            Assert.That(_machine.Message, Is.EqualTo("Please select a valid item."));
            Assert.That(_machine.Dispense(), Is.EqualTo(DispensedItem.Empty));            
        }

        [Test]
        public void Should_Clear_Message_When_Valid_Item_Is_Selected_After_An_Invalid_Item_Is_Selected()
        {
            _machine.SelectItem("snack3");
            _machine.SelectItem(_item1.Id);
            Assert.That(_machine.Message, Is.Null);
        }

        [Test]
        public void Should_Reset_Vending_Machine_State_After_Dispensing()
        {           
            _machine.SelectItem(_item1.Id);
            _machine.Deposit(2);
            var firstDispensedItem =_machine.Dispense();
            Assert.That(firstDispensedItem.ItemId, Is.EqualTo(_item1.Id));
            Assert.That(_machine.TotalDeposit, Is.EqualTo(0));
            Assert.That(_machine.SelectedItem, Is.Null);
            Assert.That(_machine.Dispense(), Is.EqualTo(DispensedItem.Empty));
        }

        [Test]
        public void Should_Not_Allow_Selecting_Item_With_No_Stock()
        {
            _item1.Quantity = 0;
            _machine.SelectItem(_item1.Id);

            Assert.That(_machine.Message, Is.EqualTo($"{_item1.Id} is out of stock."));
        }
    }
}
