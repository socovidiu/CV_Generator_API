using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace CVGeneratorAPI.Models;

public class CVModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string? Id { get; set; }
    public required string? Name { get; set; }
    public required string? Email { get; set; }
    public required string? Phone { get; set; }
    public required string? Education { get; set; }
    public required string? Experience { get; set; }
    public required string? Skills { get; set; }
}
