using Avalonia.Controls;
using Avalonia.Interactivity;
using ClinicCore.Database;
using ClinicCore.Models;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System;

namespace ClinicCore.Views;

public partial class AppointmentsView : UserControl
{
    private ObservableCollection<Appointment> _appts = new();
    private int _editingId = -1;

    public AppointmentsView()
    {
        InitializeComponent();
        ApptsGrid.ItemsSource = _appts;
        LoadAppts();
    }

    private void LoadAppts()
    {
        _appts.Clear();
        try
        {
            using var conn = DBHelper.GetConnection();
            var cmd = new SqlCommand("SELECT * FROM Appointments ORDER BY AppointmentID DESC", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                _appts.Add(new Appointment
                {
                    AppointmentID   = reader.GetInt32(0),
                    PatientID       = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                    DoctorID        = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                    AppointmentDate = reader["AppointmentDate"]?.ToString() ?? "",
                    Status          = reader["Status"]?.ToString() ?? "",
                    Notes           = reader["Notes"]?.ToString() ?? ""
                });
            }
            ApptCount.Text = $"{_appts.Count} appointment(s) scheduled";
        }
        catch { ApptCount.Text = "DB not connected"; }
        ApptsGrid.ItemsSource = null;
        ApptsGrid.ItemsSource = _appts;
    }

    private void AddAppt_Click(object? s, RoutedEventArgs e)
    {
        _editingId = -1;
        FormTitle.Text = "Add New Appointment";
        ClearForm();
        FormPanel.IsVisible = true;
    }

    private void CancelForm_Click(object? s, RoutedEventArgs e)
    {
        FormPanel.IsVisible = false;
        ClearForm();
    }

    private void SaveAppt_Click(object? s, RoutedEventArgs e)
    {
        try
        {
            using var conn = DBHelper.GetConnection();
            SqlCommand cmd;
            if (_editingId == -1)
                cmd = new SqlCommand("INSERT INTO Appointments (PatientID,DoctorID,AppointmentDate,Status,Notes) VALUES (@p,@d,@dt,@s,@n)", conn);
            else
            {
                cmd = new SqlCommand("UPDATE Appointments SET PatientID=@p,DoctorID=@d,AppointmentDate=@dt,Status=@s,Notes=@n WHERE AppointmentID=@id", conn);
                cmd.Parameters.AddWithValue("@id", _editingId);
            }
            cmd.Parameters.AddWithValue("@p", int.TryParse(TxtPatientID.Text, out var pid) ? pid : 0);
            cmd.Parameters.AddWithValue("@d", int.TryParse(TxtDoctorID.Text, out var did) ? did : 0);
            cmd.Parameters.AddWithValue("@dt", TxtDate.Text ?? "");
            cmd.Parameters.AddWithValue("@s", "Scheduled");
            cmd.Parameters.AddWithValue("@n", TxtNotes.Text ?? "");
            cmd.ExecuteNonQuery();
        }
        catch { }
        FormPanel.IsVisible = false;
        ClearForm();
        LoadAppts();
    }

    private void EditAppt_Click(object? s, RoutedEventArgs e)
    {
        if (ApptsGrid.SelectedItem is not Appointment a) return;
        _editingId = a.AppointmentID;
        FormTitle.Text    = "Edit Appointment";
        TxtPatientID.Text = a.PatientID.ToString();
        TxtDoctorID.Text  = a.DoctorID.ToString();
        TxtDate.Text      = a.AppointmentDate;
        TxtNotes.Text     = a.Notes;
        FormPanel.IsVisible = true;
    }

    private void DeleteAppt_Click(object? s, RoutedEventArgs e)
    {
        if (ApptsGrid.SelectedItem is not Appointment a) return;
        try
        {
            using var conn = DBHelper.GetConnection();
            var cmd = new SqlCommand("DELETE FROM Appointments WHERE AppointmentID=@id", conn);
            cmd.Parameters.AddWithValue("@id", a.AppointmentID);
            cmd.ExecuteNonQuery();
        }
        catch { }
        LoadAppts();
    }

    private void ClearForm()
    {
        TxtPatientID.Text = TxtDoctorID.Text = TxtDate.Text = TxtNotes.Text = "";
        _editingId = -1;
    }
}
