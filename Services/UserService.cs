using CVGeneratorAPI.Models;
using CVGeneratorAPI.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CVGeneratorAPI.Services;

public class UserService
{
    private readonly IMongoCollection<UserModel> _userCollection;

    public UserService(IOptions<MongoDBSettings> mongoDBSettings)
    {
        var client = new MongoClient(mongoDBSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _userCollection = database.GetCollection<UserModel>(mongoDBSettings.Value.UsersCollectionName);
    }

    public async Task<UserModel?> GetByUsernameAsync(string username) =>
        await _userCollection.Find(u => u.Username == username).FirstOrDefaultAsync();

    public async Task<UserModel?> GetByEmailAsync(string email) =>
        await _userCollection.Find(u => u.Email == email).FirstOrDefaultAsync();

    public async Task CreateUserAsync(UserModel user) =>
        await _userCollection.InsertOneAsync(user);

    public async Task UpdateUserAsync(string id, UserModel user) =>
        await _userCollection.ReplaceOneAsync(u => u.Id == id, user);

    public async Task DeleteUserAsync(string id) =>
        await _userCollection.DeleteOneAsync(u => u.Id == id);
}