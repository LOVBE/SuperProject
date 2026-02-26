using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EMT_API.Models;

[Table("SpeakingSubmission")]
public partial class SpeakingSubmission
{
    [Key]
    public long SpeakingID { get; set; }

    public int AnswerID { get; set; }

    [StringLength(1000)]
    public string AudioUrl { get; set; } = null!;

    public string? Transcript { get; set; }

    public int? DurationSec { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("AnswerID")]
    [InverseProperty("SpeakingSubmissions")]
    public virtual Answer Answer { get; set; } = null!;
}
