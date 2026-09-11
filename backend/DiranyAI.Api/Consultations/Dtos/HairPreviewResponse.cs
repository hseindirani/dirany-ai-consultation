namespace DiranyAI.Api.Consultations.Dtos
{
    public class HairPreviewResponse
    {
        public long ImageId { get; init; }

        public long HairCandidateId { get; init; }

        public string StoragePath { get; init; } = string.Empty;
    }
}
