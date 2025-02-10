using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

public class UserProfile
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Bio { get; set; }
    public string ProfilePicturePath { get; set; }

    private static string filePath = Path.Combine(FileSystem.AppDataDirectory, "profile.json");

    public static async Task<UserProfile> LoadProfileAsync()
    {
        if (File.Exists(filePath))
        {
            string json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<UserProfile>(json) ?? new UserProfile();
        }
        return new UserProfile();
    }

    public async Task SaveProfileAsync()
    {
        string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, json);
    }
}
