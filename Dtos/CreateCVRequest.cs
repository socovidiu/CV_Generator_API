namespace CVGeneratorAPI.Dtos;

public class CreateCVRequest
{
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

public class WorkExperienceDto
{
    public required string Position { get; set; }
    public required string Company { get; set; }
    public required DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public required string Description { get; set; }
}

public class EducationDto
{
    public required string Degree { get; set; }
    public required string School { get; set; }
    public required DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class LinkDto
{
    public required string Type { get; set; } // "LinkedIn" | "GitHub" | "Website"
    public required string Url { get; set; }
}
