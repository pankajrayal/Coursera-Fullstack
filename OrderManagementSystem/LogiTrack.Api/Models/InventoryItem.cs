using System.ComponentModel.DataAnnotations;

namespace LogiTrack.Api.Models {
  public class InventoryItem {
    [Key]
    public int ItemId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public int Quantity { get; set; }

    public string Location { get; set; }
  }
}