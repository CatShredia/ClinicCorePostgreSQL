using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using ClinicCore.Auth;
using ClinicCore.Localization;
using ClinicCore.Views;

namespace ClinicCore;

public partial class MainWindow : Window
{
    private Button? _activeBtn;
    private string _currentPage = "Dashboard";

    public MainWindow()
    {
        InitializeComponent();
        LocalizationService.LanguageChanged += ApplyLocalization;
        ApplyLocalization();
        NavigateTo("Dashboard");
    }

    private void ApplyLocalization()
    {
        Title = L.Get("WindowTitle");
        TxtSubtitle.Text = L.Get("Subtitle");
        TxtMenu.Text = L.Get("Menu");
        TxtLanguageLabel.Text = L.Get("Language");
        BtnDashboard.Content = L.Get("NavDashboard");
        BtnPatients.Content = L.Get("NavPatients");
        BtnDoctors.Content = L.Get("NavDoctors");
        BtnAppointments.Content = L.Get("NavAppointments");
        BtnPrescriptions.Content = L.Get("NavPrescriptions");
        BtnLogout.Content = L.Get("Logout");

        if (AuthSession.CurrentUser != null)
        {
            TxtUserName.Text = AuthSession.CurrentUser.FullName;
            TxtUserRole.Text = L.Format("UserRole", L.TranslateRole(AuthSession.CurrentUser.Role));
        }

        BtnLangEn.Classes.Set("active", LocalizationService.Current == AppLanguage.English);
        BtnLangRu.Classes.Set("active", LocalizationService.Current == AppLanguage.Russian);
    }

    private void LangEn_Click(object? sender, RoutedEventArgs e)
        => LocalizationService.SetLanguage(AppLanguage.English);

    private void LangRu_Click(object? sender, RoutedEventArgs e)
        => LocalizationService.SetLanguage(AppLanguage.Russian);

    private void Logout_Click(object? sender, RoutedEventArgs e)
    {
        AuthSession.Logout();
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var login = new LoginWindow();
            desktop.MainWindow = login;
            login.Show();
            Close();
        }
    }

    private void Nav_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn) return;
        NavigateTo(btn.Tag?.ToString() ?? "Dashboard", btn);
    }

    private void NavigateTo(string page, Button? btn = null)
    {
        if (!AuthSession.IsAuthenticated)
        {
            Logout_Click(null, new RoutedEventArgs());
            return;
        }

        _currentPage = page;

        if (btn == null)
            btn = page switch
            {
                "Patients"      => BtnPatients,
                "Doctors"       => BtnDoctors,
                "Appointments"  => BtnAppointments,
                "Prescriptions" => BtnPrescriptions,
                _               => BtnDashboard
            };

        if (_activeBtn != null)
            _activeBtn.Classes.Remove("active");
        btn.Classes.Add("active");
        _activeBtn = btn;

        MainContent.Content = page switch
        {
            "Dashboard"     => new DashboardView(),
            "Patients"      => new PatientsView(),
            "Doctors"       => new DoctorsView(),
            "Appointments"  => new AppointmentsView(),
            "Prescriptions" => new PrescriptionsView(),
            _               => new DashboardView()
        };
    }
}
