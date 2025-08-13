using CVGeneratorAPI.Dtos;
using CVGeneratorAPI.Models;

namespace CVGeneratorAPI.Mappers;

public static class CvMappings
{
    public static CVModel ToModel(this CreateCVRequest dto, string userId) => new()
    {
        UserId = userId,
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        City = dto.City,
        Country = dto.Country,
        Postcode = dto.Postcode,
        Phone = dto.Phone,
        Email = dto.Email,
        Photo = dto.Photo,
        JobTitle = dto.JobTitle,
        Summary = dto.Summary,
        Skills = dto.Skills.ToList(),
        WorkExperiences = dto.WorkExperiences.Select(w => new WorkExperience
        {
            Position = w.Position,
            Company = w.Company,
            StartDate = w.StartDate,
            EndDate = w.EndDate,
            Description = w.Description
        }).ToList(),
        Educations = dto.Educations.Select(e => new Education
        {
            Degree = e.Degree,
            School = e.School,
            StartDate = e.StartDate,
            EndDate = e.EndDate
        }).ToList(),
        Links = dto.Links.Select(l => new Link
        {
            Type = l.Type,
            Url = l.Url
        }).ToList()
    };

    public static void ApplyToModel(this UpdateCVRequest dto, CVModel model)
    {
        model.FirstName = dto.FirstName;
        model.LastName = dto.LastName;
        model.City = dto.City;
        model.Country = dto.Country;
        model.Postcode = dto.Postcode;
        model.Phone = dto.Phone;
        model.Email = dto.Email;
        model.Photo = dto.Photo;
        model.JobTitle = dto.JobTitle;
        model.Summary = dto.Summary;
        model.Skills = dto.Skills.ToList();
        model.WorkExperiences = dto.WorkExperiences.Select(w => new WorkExperience
        {
            Position = w.Position,
            Company = w.Company,
            StartDate = w.StartDate,
            EndDate = w.EndDate,
            Description = w.Description
        }).ToList();
        model.Educations = dto.Educations.Select(e => new Education
        {
            Degree = e.Degree,
            School = e.School,
            StartDate = e.StartDate,
            EndDate = e.EndDate
        }).ToList();
        model.Links = dto.Links.Select(l => new Link
        {
            Type = l.Type,
            Url = l.Url
        }).ToList();
    }

    public static CVResponse ToResponse(this CVModel model) => new()
    {
        Id = model.Id,
        FirstName = model.FirstName,
        LastName = model.LastName,
        City = model.City,
        Country = model.Country,
        Postcode = model.Postcode,
        Phone = model.Phone,
        Email = model.Email,
        Photo = model.Photo,
        JobTitle = model.JobTitle,
        Summary = model.Summary,
        Skills = model.Skills.ToList(),
        WorkExperiences = model.WorkExperiences.Select(w => new WorkExperienceDto
        {
            Position = w.Position,
            Company = w.Company,
            StartDate = w.StartDate,
            EndDate = w.EndDate,
            Description = w.Description
        }).ToList(),
        Educations = model.Educations.Select(e => new EducationDto
        {
            Degree = e.Degree,
            School = e.School,
            StartDate = e.StartDate,
            EndDate = e.EndDate
        }).ToList(),
        Links = model.Links.Select(l => new LinkDto
        {
            Type = l.Type,
            Url = l.Url
        }).ToList()
    };
}
