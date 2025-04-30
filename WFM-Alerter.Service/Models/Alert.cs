using System.ComponentModel.DataAnnotations;

namespace WFM_Alerter.Service.Models;
public class Alert
{
    // Constructor to initialize the Guid property
    public Alert() => Guid = Guid.NewGuid();

    [Key, Required]
    public Guid Guid { get; set; }

    [Required]
    public required string Email { get; set; }

    [Required]
    public required string ItemName { get; set; }

    public int? ItemRank { get; set; }

    [Required]
    public required int Price { get; set; }
}