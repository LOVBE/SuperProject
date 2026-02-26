using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EMT_API.Models;

[PrimaryKey("UserID", "VideoID")]
[Table("UserVideoProgress")]
public partial class UserVideoProgress
{
    [Key]
    public int UserID { get; set; }

    [Key]
    public int VideoID { get; set; }

    public DateTime WatchedAt { get; set; }

    public int? WatchDurationSec { get; set; }

    public bool IsCompleted { get; set; }

    [ForeignKey("UserID")]
    [InverseProperty("UserVideoProgresses")]
    public virtual Account User { get; set; } = null!;

    [ForeignKey("VideoID")]
    [InverseProperty("UserVideoProgresses")]
    public virtual CourseVideo Video { get; set; } = null!;
}
