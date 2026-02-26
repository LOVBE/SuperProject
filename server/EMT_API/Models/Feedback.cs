using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EMT_API.Models;

[Table("Feedback")]
[Index("CourseID", "CreatedAt", Name = "IX_Feedback_Course", IsDescending = new[] { false, true })]
[Index("UserID", "CreatedAt", Name = "IX_Feedback_User", IsDescending = new[] { false, true })]
public partial class Feedback
{
    [Key]
    public int FeedbackID { get; set; }

    public int UserID { get; set; }

    public int CourseID { get; set; }

    public byte Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsVisible { get; set; }

    [ForeignKey("CourseID")]
    [InverseProperty("Feedbacks")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("UserID")]
    [InverseProperty("Feedbacks")]
    public virtual Account User { get; set; } = null!;
}
