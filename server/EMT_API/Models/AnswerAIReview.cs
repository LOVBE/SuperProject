using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EMT_API.Models;

[Table("AnswerAIReview")]
public partial class AnswerAIReview
{
    [Key]
    public long AIReviewID { get; set; }

    public long SubmissionID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ModelName { get; set; }

    [Column(TypeName = "decimal(4, 2)")]
    public decimal ScoreOverall { get; set; }

    [Column(TypeName = "decimal(4, 2)")]
    public decimal? ScoreTask { get; set; }

    [Column(TypeName = "decimal(4, 2)")]
    public decimal? ScoreCoherence { get; set; }

    [Column(TypeName = "decimal(4, 2)")]
    public decimal? ScoreLexical { get; set; }

    [Column(TypeName = "decimal(4, 2)")]
    public decimal? ScoreGrammar { get; set; }

    [Column(TypeName = "decimal(4, 2)")]
    public decimal? ScorePronunciation { get; set; }

    public string Feedback { get; set; } = null!;

    public bool IsSentToTeacher { get; set; }

    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "decimal(4, 2)")]
    public decimal? ScoreFluency { get; set; }

    [ForeignKey("SubmissionID")]
    [InverseProperty("AnswerAIReviews")]
    public virtual AISubmission Submission { get; set; } = null!;
}
