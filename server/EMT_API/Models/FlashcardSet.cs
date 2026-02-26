using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EMT_API.Models;

[Table("FlashcardSet")]
public partial class FlashcardSet
{
    [Key]
    public int SetID { get; set; }

    public int? CourseID { get; set; }

    public int? ChapterID { get; set; }

    [StringLength(200)]
    public string Title { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    [ForeignKey("ChapterID")]
    [InverseProperty("FlashcardSets")]
    public virtual CourseChapter? Chapter { get; set; }

    [ForeignKey("CourseID")]
    [InverseProperty("FlashcardSets")]
    public virtual Course? Course { get; set; }

    [InverseProperty("Set")]
    public virtual ICollection<FlashcardItem> FlashcardItems { get; set; } = new List<FlashcardItem>();
}
