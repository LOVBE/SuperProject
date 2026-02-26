using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EMT_API.Models;

[Table("AISubmission")]
public partial class AISubmission
{
    [Key]
    public long SubmissionID { get; set; }

    public int PromptID { get; set; }

    public int UserID { get; set; }

    public string? AnswerText { get; set; }

    [StringLength(1000)]
    public string? AudioUrl { get; set; }

    public string? Transcript { get; set; }

    public DateTime CreatedAt { get; set; }

    [InverseProperty("Submission")]
    public virtual ICollection<AnswerAIReview> AnswerAIReviews { get; set; } = new List<AnswerAIReview>();

    [ForeignKey("PromptID")]
    [InverseProperty("AISubmissions")]
    public virtual AIPrompt Prompt { get; set; } = null!;

    [ForeignKey("UserID")]
    [InverseProperty("AISubmissions")]
    public virtual Account User { get; set; } = null!;
}
