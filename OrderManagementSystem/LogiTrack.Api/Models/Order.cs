using System.ComponentModel.DataAnnotations;

namespace LogiTrack.Api.Models {
  public class Order {
    [Key]
    public int OrderId { get; set; }

    [Required]
    public string CustomerName { get; set; }

    [Required]
    public DateTime DatePlaced { get; set; } = DateTime.UtcNow;

    public List<InventoryItem> Items { get; set; } = new List<InventoryItem>();

    public void AddItem(InventoryItem item) {
      Items.Add(item);
    }

    public void RemoveItem(int itemId) {
      Items.RemoveAll(item => item.ItemId == itemId);
    }

    public string GetOrderSummary() {
      return $"Order #{OrderId} for {CustomerName} | Items: {Items.Count} | Placed: {DatePlaced:MM/dd/yyyy}";
    }
  }
}