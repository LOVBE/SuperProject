using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EMT_API.Models;

[Table("AIPrompt")]
public partial class AIPrompt
{
    [Key]
    public int PromptID { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string SkillType { get; set; } = null!;

    [StringLength(200)]
    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    [InverseProperty("Prompt")]
    public virtual ICollection<AISubmission> AISubmissions { get; set; } = new List<AISubmission>();
}
