using Avalonia.Controls;
using Avalonia.Interactivity;
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

        BtnLangEn.Classes.Set("active", LocalizationService.Current == AppLanguage.English);
        BtnLangRu.Classes.Set("active", LocalizationService.Current == AppLanguage.Russian);
    }

    private void LangEn_Click(object? sender, RoutedEventArgs e)
        => LocalizationService.SetLanguage(AppLanguage.English);

    private void LangRu_Click(object? sender, RoutedEventArgs e)
        => LocalizationService.SetLanguage(AppLanguage.Russian);

    private void Nav_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn) return;
        NavigateTo(btn.Tag?.ToString() ?? "Dashboard", btn);
    }

    private void NavigateTo(string page, Button? btn = null)
    {
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
