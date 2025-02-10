using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace MauiApp38;

public class ProfileViewModel : BindableObject
{
    private UserProfile _profile;

    public string Name
    {
        get => _profile.Name;
        set { _profile.Name = value; OnPropertyChanged(); }
    }

    public string Surname
    {
        get => _profile.Surname;
        set { _profile.Surname = value; OnPropertyChanged(); }
    }

    public string Email
    {
        get => _profile.Email;
        set { _profile.Email = value; OnPropertyChanged(); }
    }

    public string Bio
    {
        get => _profile.Bio;
        set { _profile.Bio = value; OnPropertyChanged(); }
    }

    public string ProfilePicturePath
    {
        get => _profile.ProfilePicturePath;
        set { _profile.ProfilePicturePath = value; OnPropertyChanged(); }
    }

    public ICommand SaveCommand { get; }
    public ICommand ChooseImageCommand { get; }

    public ProfileViewModel()
    {
        _profile = new UserProfile();
        LoadProfile();

        SaveCommand = new Command(async () => await SaveProfileAsync());
        ChooseImageCommand = new Command(async () => await ChooseProfileImageAsync());
    }

    private async void LoadProfile()
    {
        _profile = await UserProfile.LoadProfileAsync();
        OnPropertyChanged(nameof(Name));
        OnPropertyChanged(nameof(Surname));
        OnPropertyChanged(nameof(Email));
        OnPropertyChanged(nameof(Bio));
        OnPropertyChanged(nameof(ProfilePicturePath));
    }

    private async Task SaveProfileAsync()
    {
        await _profile.SaveProfileAsync();
        await Application.Current.MainPage.DisplayAlert("Success", "Profile saved!", "OK");
    }

    private async Task ChooseProfileImageAsync()
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            FileTypes = FilePickerFileType.Images,
            PickerTitle = "Select a Profile Picture"
        });

        if (result != null)
        {
            string newFilePath = Path.Combine(FileSystem.AppDataDirectory, result.FileName);
            using var stream = await result.OpenReadAsync();
            using var newStream = File.OpenWrite(newFilePath);
            await stream.CopyToAsync(newStream);

            ProfilePicturePath = newFilePath;
        }
    }
}
