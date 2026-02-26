namespace EMT_API.DTOs.Progress
{
    public class UpdateFlashcardProgressRequest
    {
        public int ItemId { get; set; }
        public bool IsMastered { get; set; }
        public int ReviewCount { get; set; }
        public DateTime? NextReviewAt { get; set; }
    }
}
