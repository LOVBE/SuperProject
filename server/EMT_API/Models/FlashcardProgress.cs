using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EMT_API.Models;

[PrimaryKey("UserID", "ItemID")]
[Table("FlashcardProgress")]
public partial class FlashcardProgress
{
    [Key]
    public int UserID { get; set; }

    [Key]
    public int ItemID { get; set; }

    public DateTime? FirstLearnedAt { get; set; }

    public DateTime? LastReviewedAt { get; set; }

    public int ReviewCount { get; set; }

    public bool IsMastered { get; set; }

    public DateTime? NextReviewAt { get; set; }

    [Column(TypeName = "decimal(4, 2)")]
    public decimal EaseFactor { get; set; }

    public int IntervalDays { get; set; }

    [ForeignKey("ItemID")]
    [InverseProperty("FlashcardProgresses")]
    public virtual FlashcardItem Item { get; set; } = null!;

    [ForeignKey("UserID")]
    [InverseProperty("FlashcardProgresses")]
    public virtual Account User { get; set; } = null!;
}
