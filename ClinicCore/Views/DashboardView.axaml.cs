using System;
using Avalonia.Controls;
using ClinicCore.Auth;
using ClinicCore.Database;
using ClinicCore.Localization;
using Npgsql;

namespace ClinicCore.Views;

public partial class DashboardView : LocalizableUserControl
{
    private bool _connected;
    private int _patients, _doctors, _appointments, _prescriptions;

    public DashboardView()
    {
        InitializeComponent();
        ApplyLocalization();
        LoadStats();
    }

    protected override void ApplyLocalization()
    {
        TxtTitle.Text = L.Get("DashboardTitle");
        TxtWelcome.Text = L.Get("Welcome");
        LblPatients.Text = L.Get("Patients");
        LblDoctors.Text = L.Get("Doctors");
        LblAppointments.Text = L.Get("Appointments");
        LblPrescriptions.Text = L.Get("Prescriptions");
        LblPatientsSub.Text = L.Get("TotalRegistered");
        LblDoctorsSub.Text = L.Get("TotalRegistered");
        LblAppointmentsSub.Text = L.Get("TotalScheduled");
        LblPrescriptionsSub.Text = L.Get("TotalIssued");
        LblSystemInfo.Text = L.Get("SystemInfo");

        if (_connected)
            UpdateInfoText();
        else if (TxtInfo.Text != L.Get("Loading"))
            TxtInfo.Text = L.Get("DbNotConnected");

        TxtPatients.Text = _connected ? _patients.ToString() : "?";
        TxtDoctors.Text = _connected ? _doctors.ToString() : "?";
        TxtAppointments.Text = _connected ? _appointments.ToString() : "?";
        TxtPrescriptions.Text = _connected ? _prescriptions.ToString() : "?";
    }

    private void LoadStats()
    {
        try
        {
            using var conn = DBHelper.GetConnection();
            _patients      = GetCount(conn, @"SELECT COUNT(*) FROM ""Patients""");
            _doctors       = GetCount(conn, @"SELECT COUNT(*) FROM ""Doctors""");
            _appointments  = GetCount(conn, @"SELECT COUNT(*) FROM ""Appointments""");
            _prescriptions = GetCount(conn, @"SELECT COUNT(*) FROM ""Prescriptions""");
            _connected = true;

            TxtPatients.Text      = _patients.ToString();
            TxtDoctors.Text       = _doctors.ToString();
            TxtAppointments.Text  = _appointments.ToString();
            TxtPrescriptions.Text = _prescriptions.ToString();
            UpdateInfoText();
        }
        catch
        {
            _connected = false;
            TxtPatients.Text = TxtDoctors.Text = TxtAppointments.Text = TxtPrescriptions.Text = "?";
            TxtInfo.Text = L.Get("DbNotConnected");
        }
    }

    private void UpdateInfoText()
    {
        var dbInfo = L.Format("DbInfo", DBHelper.DatabaseLabel, DBHelper.ServerLabel, L.Get("Connected"));
        if (AuthSession.CurrentUser != null)
        {
            var authInfo = L.Format("AuthInfo",
                AuthSession.CurrentUser.FullName,
                L.TranslateRole(AuthSession.CurrentUser.Role));
            TxtInfo.Text = dbInfo + Environment.NewLine + authInfo;
        }
        else
        {
            TxtInfo.Text = dbInfo;
        }
    }

    private int GetCount(System.Data.Common.DbConnection conn, string sql)
    {
        var cmd = new NpgsqlCommand(sql, (NpgsqlConnection)conn);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }
}
