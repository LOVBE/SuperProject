using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EMT_API.Models;

[Table("FlashcardHistory")]
public partial class FlashcardHistory
{
    [Key]
    public long HistoryID { get; set; }

    public int UserID { get; set; }

    public int ItemID { get; set; }

    public byte ActionType { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("ItemID")]
    [InverseProperty("FlashcardHistories")]
    public virtual FlashcardItem Item { get; set; } = null!;

    [ForeignKey("UserID")]
    [InverseProperty("FlashcardHistories")]
    public virtual Account User { get; set; } = null!;
}
