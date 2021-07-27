namespace entities {
	using System.Collections.Generic;
	
	public abstract class InventoryItem: Item {}
	
	public class Inventory {
		public List<InventoryItem> items;

		public Inventory() {
			this.items = new List<InventoryItem>();
		}
		
		public Inventory(List<InventoryItem> items) {
			this.items = items;
		}
	}
}
