using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CVGeneratorAPI.Models;

public class UserModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
}