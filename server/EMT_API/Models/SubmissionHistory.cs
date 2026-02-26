using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EMT_API.Models;

[Table("SubmissionHistory")]
public partial class SubmissionHistory
{
    [Key]
    public long SubmissionID { get; set; }

    public int AttemptID { get; set; }

    public int UserID { get; set; }

    public DateTime SubmittedAt { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal? AutoScore { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal? ManualScore { get; set; }

    [ForeignKey("AttemptID")]
    [InverseProperty("SubmissionHistories")]
    public virtual Attempt Attempt { get; set; } = null!;

    [ForeignKey("UserID")]
    [InverseProperty("SubmissionHistories")]
    public virtual Account User { get; set; } = null!;
}
