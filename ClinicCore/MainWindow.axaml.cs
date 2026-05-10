using Avalonia.Controls;
using Avalonia.Interactivity;
using ClinicCore.Views;

namespace ClinicCore;

public partial class MainWindow : Window
{
    private Button? _activeBtn;

    public MainWindow()
    {
        InitializeComponent();
        // Dashboard is the default screen
        MainContent.Content = new DashboardView();
    }

    private void Nav_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn) return;

        // Remove active from previous button
        if (_activeBtn != null)
            _activeBtn.Classes.Remove("active");

        // Set new active button
        btn.Classes.Add("active");
        _activeBtn = btn;

        // Switch content based on sidebar click
        MainContent.Content = btn.Tag?.ToString() switch
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