using System;
using System.Collections.Generic;
using EMT_API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMT_API.Data;

public partial class EMTDbContext : DbContext
{
    public EMTDbContext()
    {
    }

    public EMTDbContext(DbContextOptions<EMTDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AIPrompt> AIPrompts { get; set; }

    public virtual DbSet<AISubmission> AISubmissions { get; set; }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<AnswerAIReview> AnswerAIReviews { get; set; }

    public virtual DbSet<AnswerTeacherReview> AnswerTeacherReviews { get; set; }

    public virtual DbSet<Asset> Assets { get; set; }

    public virtual DbSet<Attempt> Attempts { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseChapter> CourseChapters { get; set; }

    public virtual DbSet<CourseVideo> CourseVideos { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<FlashcardHistory> FlashcardHistories { get; set; }

    public virtual DbSet<FlashcardItem> FlashcardItems { get; set; }

    public virtual DbSet<FlashcardProgress> FlashcardProgresses { get; set; }

    public virtual DbSet<FlashcardSet> FlashcardSets { get; set; }

    public virtual DbSet<Option> Options { get; set; }

    public virtual DbSet<PaymentOrder> PaymentOrders { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionGroup> QuestionGroups { get; set; }

    public virtual DbSet<Quiz> Quizzes { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<RequestLog> RequestLogs { get; set; }

    public virtual DbSet<SubmissionHistory> SubmissionHistories { get; set; }

    public virtual DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<UserDetail> UserDetails { get; set; }

    public virtual DbSet<UserMembership> UserMemberships { get; set; }

    public virtual DbSet<UserVideoProgress> UserVideoProgresses { get; set; }

    public virtual DbSet<WebhookEvent> WebhookEvents { get; set; }

    public virtual DbSet<vUserHasActiveMembership> vUserHasActiveMemberships { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AIPrompt>(entity =>
        {
            entity.HasKey(e => e.PromptID).HasName("PK__AIPrompt__456CA7738653124B");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<AISubmission>(entity =>
        {
            entity.HasKey(e => e.SubmissionID).HasName("PK__AISubmis__449EE105D6C1C4BC");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Prompt).WithMany(p => p.AISubmissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AISubmission_Prompt");

            entity.HasOne(d => d.User).WithMany(p => p.AISubmissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AISubmission_User");
        });

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountID).HasName("PK__Account__349DA586E914E1F3");

            entity.HasIndex(e => e.GoogleSub, "UX_Account_GoogleSub")
                .IsUnique()
                .HasFilter("([GoogleSub] IS NOT NULL)");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Role).HasDefaultValue("STUDENT");
            entity.Property(e => e.Status).HasDefaultValue("ACTIVE");
        });

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.AnswerID).HasName("PK__Answer__D48250242B31B938");

            entity.Property(e => e.AnsweredAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Attempt).WithMany(p => p.Answers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Answer_Attempt");

            entity.HasOne(d => d.Option).WithMany(p => p.Answers).HasConstraintName("FK_Answer_Option");

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Answer_Question");
        });

        modelBuilder.Entity<AnswerAIReview>(entity =>
        {
            entity.HasKey(e => e.AIReviewID).HasName("PK__AnswerAI__D0B4D52A3DA8DD51");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Submission).WithMany(p => p.AnswerAIReviews).HasConstraintName("FK_AIReview_Submission");
        });

        modelBuilder.Entity<AnswerTeacherReview>(entity =>
        {
            entity.HasKey(e => e.TeacherReviewID).HasName("PK__AnswerTe__0908B30C4A33924A");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Teacher).WithMany(p => p.AnswerTeacherReviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeacherReview_Teacher");
        });

        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(e => e.AssetID).HasName("PK__Asset__4349237245445389");
        });

        modelBuilder.Entity<Attempt>(entity =>
        {
            entity.HasKey(e => e.AttemptID).HasName("PK__Attempt__891A68864B13599A");

            entity.Property(e => e.StartedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Status).HasDefaultValue("in_progress");

            entity.HasOne(d => d.Quiz).WithMany(p => p.Attempts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attempt_Quiz");

            entity.HasOne(d => d.User).WithMany(p => p.Attempts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attempt_User");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseID).HasName("PK__Course__C92D718727497EF0");

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Courses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Course_Teacher");
        });

        modelBuilder.Entity<CourseChapter>(entity =>
        {
            entity.HasKey(e => e.ChapterID).HasName("PK__CourseCh__0893A34A5ECD2B1C");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseChapters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Chapter_Course");
        });

        modelBuilder.Entity<CourseVideo>(entity =>
        {
            entity.HasKey(e => e.VideoID).HasName("PK__CourseVi__BAE5124ACCF9BFAF");

            entity.HasOne(d => d.Chapter).WithMany(p => p.CourseVideos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Video_Chapter");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseVideos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Video_Course");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackID).HasName("PK__Feedback__6A4BEDF68952B373");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsVisible).HasDefaultValue(true);

            entity.HasOne(d => d.Course).WithMany(p => p.Feedbacks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Feedback_Course");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Feedback_User");
        });

        modelBuilder.Entity<FlashcardHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryID).HasName("PK__Flashcar__4D7B4ADDB234FCA3");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Item).WithMany(p => p.FlashcardHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UFH_Item");

            entity.HasOne(d => d.User).WithMany(p => p.FlashcardHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UFH_User");
        });

        modelBuilder.Entity<FlashcardItem>(entity =>
        {
            entity.HasKey(e => e.ItemID).HasName("PK__Flashcar__727E83EBAA1FE0D8");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Set).WithMany(p => p.FlashcardItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Flashcard__SetID__41EDCAC5");
        });

        modelBuilder.Entity<FlashcardProgress>(entity =>
        {
            entity.HasKey(e => new { e.UserID, e.ItemID }).HasName("PK__Flashcar__B0AF2492FD275D82");

            entity.Property(e => e.EaseFactor).HasDefaultValue(2.5m);
            entity.Property(e => e.IntervalDays).HasDefaultValue(1);

            entity.HasOne(d => d.Item).WithMany(p => p.FlashcardProgresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UFP_Item");

            entity.HasOne(d => d.User).WithMany(p => p.FlashcardProgresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UFP_User");
        });

        modelBuilder.Entity<FlashcardSet>(entity =>
        {
            entity.HasKey(e => e.SetID).HasName("PK__Flashcar__7E08473D39DBDC16");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Chapter).WithMany(p => p.FlashcardSets).HasConstraintName("FK__Flashcard__Chapt__3E1D39E1");

            entity.HasOne(d => d.Course).WithMany(p => p.FlashcardSets).HasConstraintName("FK__Flashcard__Cours__3D2915A8");
        });

        modelBuilder.Entity<Option>(entity =>
        {
            entity.HasKey(e => e.OptionID).HasName("PK__Option__92C7A1DFD0F8CB95");

            entity.HasOne(d => d.Question).WithMany(p => p.Options)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Option_Question");
        });

        modelBuilder.Entity<PaymentOrder>(entity =>
        {
            entity.HasKey(e => e.OrderID).HasName("PK__PaymentO__C3905BAF3023A4DE");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Status).HasDefaultValue("PENDING");

            entity.HasOne(d => d.Buyer).WithMany(p => p.PaymentOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PO_Buyer");

            entity.HasOne(d => d.Plan).WithMany(p => p.PaymentOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PO_Plan");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionID).HasName("PK__Question__0DC06F8C8994EEBA");

            entity.Property(e => e.QuestionOrder).HasDefaultValue(1);
            entity.Property(e => e.ScoreWeight).HasDefaultValue(1.00m);

            entity.HasOne(d => d.Group).WithMany(p => p.Questions).HasConstraintName("FK_Question_QGroup");

            entity.HasOne(d => d.Quiz).WithMany(p => p.Questions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Question_Quiz");
        });

        modelBuilder.Entity<QuestionGroup>(entity =>
        {
            entity.HasKey(e => e.GroupID).HasName("PK__Question__149AF30ABAD3200C");

            entity.HasOne(d => d.Quiz).WithMany(p => p.QuestionGroups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QGroup_Quiz");
        });

        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(e => e.QuizID).HasName("PK__Quiz__8B42AE6EFF47484F");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Course).WithMany(p => p.Quizzes).HasConstraintName("FK_Quiz_Course");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.RequestID).HasName("PK__Request__33A8519AFA1B282F");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Status).HasDefaultValue("PENDING");

            entity.HasOne(d => d.Course).WithMany(p => p.Requests).HasConstraintName("FK_Request_Course");

            entity.HasOne(d => d.ProcessedByNavigation).WithMany(p => p.RequestProcessedByNavigations).HasConstraintName("FK_Request_Admin");

            entity.HasOne(d => d.User).WithMany(p => p.RequestUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Request_User");
        });

        modelBuilder.Entity<RequestLog>(entity =>
        {
            entity.HasKey(e => e.LogID).HasName("PK__RequestL__5E5499A83F866F19");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<SubmissionHistory>(entity =>
        {
            entity.HasKey(e => e.SubmissionID).HasName("PK__Submissi__449EE1053EE0FBE0");

            entity.HasOne(d => d.Attempt).WithMany(p => p.SubmissionHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SH_Attempt");

            entity.HasOne(d => d.User).WithMany(p => p.SubmissionHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SH_User");
        });

        modelBuilder.Entity<SubscriptionPlan>(entity =>
        {
            entity.HasKey(e => e.PlanID).HasName("PK__Subscrip__755C22D7E60DD832");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherID).HasName("PK__Teacher__EDF2594405080490");

            entity.Property(e => e.TeacherID).ValueGeneratedNever();
            entity.Property(e => e.JoinAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.TeacherNavigation).WithOne(p => p.Teacher)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Teacher_Account");
        });

        modelBuilder.Entity<UserDetail>(entity =>
        {
            entity.HasKey(e => e.AccountID).HasName("PK__UserDeta__349DA586A40F688A");

            entity.Property(e => e.AccountID).ValueGeneratedNever();

            entity.HasOne(d => d.Account).WithOne(p => p.UserDetail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserDetail_Account");
        });

        modelBuilder.Entity<UserMembership>(entity =>
        {
            entity.HasKey(e => e.MembershipID).HasName("PK__UserMemb__92A7859936ED55B6");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Status).HasDefaultValue("ACTIVE");

            entity.HasOne(d => d.User).WithMany(p => p.UserMemberships)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UM_User");
        });

        modelBuilder.Entity<UserVideoProgress>(entity =>
        {
            entity.HasKey(e => new { e.UserID, e.VideoID }).HasName("PK__UserVide__AC269D888F6F1DF6");

            entity.Property(e => e.WatchedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.User).WithMany(p => p.UserVideoProgresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UVP_User");

            entity.HasOne(d => d.Video).WithMany(p => p.UserVideoProgresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UVP_Video");
        });

        modelBuilder.Entity<WebhookEvent>(entity =>
        {
            entity.HasKey(e => e.WebhookID).HasName("PK__WebhookE__238C71D183DB5693");

            entity.Property(e => e.ReceivedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Order).WithMany(p => p.WebhookEvents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Webhook_Order");
        });

        modelBuilder.Entity<vUserHasActiveMembership>(entity =>
        {
            entity.ToView("vUserHasActiveMembership");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
