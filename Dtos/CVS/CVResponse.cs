namespace CVGeneratorAPI.Dtos;

public class CVResponse
{
    public string? Id { get; set; }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    public required string Postcode { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }
    public string? Photo { get; set; }
    public required string JobTitle { get; set; }
    public required string Summary { get; set; }

    public required List<string> Skills { get; set; } = new();
    public required List<WorkExperienceDto> WorkExperiences { get; set; } = new();
    public required List<EducationDto> Educations { get; set; } = new();
    public required List<LinkDto> Links { get; set; } = new();
}
