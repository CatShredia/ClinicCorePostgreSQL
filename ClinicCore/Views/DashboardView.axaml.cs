using System;
using Avalonia.Controls;
using ClinicCore.Database;
using Npgsql;

namespace ClinicCore.Views;

public partial class DashboardView : UserControl
{
    public DashboardView()
    {
        InitializeComponent();
        LoadStats();
    }

    private void LoadStats()
    {
        try
        {
            using var conn = DBHelper.GetConnection();
            var patients      = GetCount(conn, @"SELECT COUNT(*) FROM ""Patients""");
            var doctors       = GetCount(conn, @"SELECT COUNT(*) FROM ""Doctors""");
            var appointments  = GetCount(conn, @"SELECT COUNT(*) FROM ""Appointments""");
            var prescriptions = GetCount(conn, @"SELECT COUNT(*) FROM ""Prescriptions""");

            TxtPatients.Text      = patients.ToString();
            TxtDoctors.Text       = doctors.ToString();
            TxtAppointments.Text  = appointments.ToString();
            TxtPrescriptions.Text = prescriptions.ToString();
            TxtInfo.Text = "Database: postgres  |  Server: localhost:5432  |  Status: Connected";
        }
        catch
        {
            TxtPatients.Text = TxtDoctors.Text = TxtAppointments.Text = TxtPrescriptions.Text = "?";
            TxtInfo.Text = "Database not connected";
        }
    }

    private int GetCount(System.Data.Common.DbConnection conn, string sql)
    {
        var cmd = new NpgsqlCommand(sql, (NpgsqlConnection)conn);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }
}
