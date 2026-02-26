using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EMT_API.Models;

[Table("AnswerTeacherReview")]
public partial class AnswerTeacherReview
{
    [Key]
    public long TeacherReviewID { get; set; }

    public long AIReviewID { get; set; }

    public int TeacherID { get; set; }

    [Column(TypeName = "decimal(4, 2)")]
    public decimal? ScoreOverall { get; set; }

    public string Feedback { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "decimal(4, 2)")]
    public decimal? ScoreTask { get; set; }

    [Column(TypeName = "decimal(4, 2)")]
    public decimal? ScoreCoherence { get; set; }

    [Column(TypeName = "decimal(4, 2)")]
    public decimal? ScoreLexial { get; set; }

    [Column(TypeName = "decimal(4, 2)")]
    public decimal? ScoreGrammar { get; set; }

    [Column(TypeName = "decimal(4, 2)")]
    public decimal? ScorePronunciation { get; set; }

    [Column(TypeName = "decimal(4, 2)")]
    public decimal? ScoreFluency { get; set; }

    [ForeignKey("TeacherID")]
    [InverseProperty("AnswerTeacherReviews")]
    public virtual Teacher Teacher { get; set; } = null!;
}
