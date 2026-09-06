namespace DiranyAI.Api.Styles;

public class HairStyle
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string AiPromptHint { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}