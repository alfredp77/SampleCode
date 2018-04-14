using System;

namespace VendingMachineApp
{
    public class DispensedItem
    {
        public static readonly DispensedItem Empty = new DispensedItem();

        public DispensedItem()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; }
        public string ItemId { get; set; }
        public decimal Change { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as DispensedItem;
            return other?.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}