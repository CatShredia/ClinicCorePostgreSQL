using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using ClinicCore.Auth;
using ClinicCore.Localization;

namespace ClinicCore;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        LocalizationService.LanguageChanged += ApplyLocalization;
        ApplyLocalization();
    }

    private void ApplyLocalization()
    {
        Title = L.Get("LoginTitle");
        TxtSubtitle.Text = L.Get("LoginSubtitle");
        LblUsername.Text = L.Get("Username");
        LblPassword.Text = L.Get("Password");
        TxtUsername.PlaceholderText = L.Get("PlaceholderUsername");
        TxtPassword.PlaceholderText = L.Get("PlaceholderPassword");
        BtnLogin.Content = L.Get("Login");
        LblHint.Text = L.Get("TestAccounts");
        TxtHintAdmin.Text = L.Get("TestAccountAdmin");
        TxtHintDoctor.Text = L.Get("TestAccountDoctor");
        TxtHintReception.Text = L.Get("TestAccountReception");
        TxtHintStaff.Text = L.Get("TestAccountStaff");
    }

    private void LangEn_Click(object? sender, RoutedEventArgs e)
        => LocalizationService.SetLanguage(AppLanguage.English);

    private void LangRu_Click(object? sender, RoutedEventArgs e)
        => LocalizationService.SetLanguage(AppLanguage.Russian);

    private void Login_Click(object? sender, RoutedEventArgs e)
    {
        TxtError.IsVisible = false;
        BtnLogin.IsEnabled = false;

        var result = AuthService.Login(TxtUsername.Text ?? "", TxtPassword.Text ?? "");
        if (result == null)
        {
            TxtError.Text = L.Get("LoginFailed");
            TxtError.IsVisible = true;
            BtnLogin.IsEnabled = true;
            return;
        }

        AuthSession.SetSession(result);

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var main = new MainWindow();
            desktop.MainWindow = main;
            main.Show();
            Close();
        }
    }
}
