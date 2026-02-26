namespace EMT_API.DTOs.Flashcard
{
    public class CreateFlashcardHistoryRequest
    {
        public int ItemId { get; set; }

        // 1 = View, 2 = Correct, 3 = Wrong, 4 = Mastered
        public byte ActionType { get; set; }
    }
}
