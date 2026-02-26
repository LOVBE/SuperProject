namespace EMT_API.DTOs.AITest
{
    public class AISpeakingSubmitAudioRequest
    {
        public int PromptId { get; set; }        
        public IFormFile File { get; set; } = null!;
        public bool SendToTeacher { get; set; }
    }
}
