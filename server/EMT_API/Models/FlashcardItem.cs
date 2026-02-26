using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EMT_API.Models;

[Table("FlashcardItem")]
public partial class FlashcardItem
{
    [Key]
    public int ItemID { get; set; }

    public int SetID { get; set; }

    [StringLength(500)]
    public string FrontText { get; set; } = null!;

    [StringLength(500)]
    public string BackText { get; set; } = null!;

    [StringLength(500)]
    public string? Example { get; set; }

    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<FlashcardHistory> FlashcardHistories { get; set; } = new List<FlashcardHistory>();

    [InverseProperty("Item")]
    public virtual ICollection<FlashcardProgress> FlashcardProgresses { get; set; } = new List<FlashcardProgress>();

    [ForeignKey("SetID")]
    [InverseProperty("FlashcardItems")]
    public virtual FlashcardSet Set { get; set; } = null!;
}
