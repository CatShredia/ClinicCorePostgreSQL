using Avalonia.Controls;
using Avalonia.Interactivity;
using ClinicCore.Auth;
using ClinicCore.Database;
using ClinicCore.Localization;
using ClinicCore.Models;
using Npgsql;
using System.Collections.ObjectModel;
using System;

namespace ClinicCore.Views;

public partial class AppointmentsView : LocalizableUserControl
{
    private ObservableCollection<Appointment> _appts = new();
    private int _editingId = -1;
    private int _apptCount;

    public AppointmentsView()
    {
        InitializeComponent();
        ApptsGrid.ItemsSource = _appts;
        ApplyLocalization();
        LoadAppts();
    }

    protected override void ApplyLocalization()
    {
        TxtTitle.Text = L.Get("Appointments");
        BtnAdd.Content = L.Get("AddAppointment");
        LblPatientId.Text = L.Get("PatientId");
        LblDoctorId.Text = L.Get("DoctorId");
        LblDate.Text = L.Get("DateTime");
        LblNotes.Text = L.Get("Notes");
        TxtPatientID.PlaceholderText = L.Get("PlaceholderPatientId");
        TxtDoctorID.PlaceholderText = L.Get("PlaceholderDoctorId");
        TxtDate.PlaceholderText = L.Get("PlaceholderDateTime");
        TxtNotes.PlaceholderText = L.Get("PlaceholderNotes");
        BtnCancel.Content = L.Get("Cancel");
        BtnSave.Content = L.Get("SaveAppointment");
        BtnEdit.Content = L.Get("EditSelected");
        BtnDelete.Content = L.Get("DeleteSelected");
        ColId.Text = L.Get("ColId");
        ColPatientId.Text = L.Get("PatientId");
        ColDoctorId.Text = L.Get("DoctorId");
        ColDate.Text = L.Get("Date");
        ColStatus.Text = L.Get("Status");
        ColNotes.Text = L.Get("Notes");
        FormTitle.Text = _editingId == -1 ? L.Get("FormAddAppointment") : L.Get("FormEditAppointment");
        if (_apptCount > 0)
            ApptCount.Text = L.Format("ApptsCount", _apptCount);

        if (_appts.Count > 0)
            LoadAppts();

        ViewPermissions.ApplyCrudButtons("Appointments", BtnAdd, BtnEdit, BtnDelete);
    }

    private void LoadAppts()
    {
        _appts.Clear();
        try
        {
            using var conn = DBHelper.GetConnection();
            var cmd = new NpgsqlCommand(@"SELECT * FROM ""Appointments"" ORDER BY ""AppointmentID"" DESC", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                _appts.Add(new Appointment
                {
                    AppointmentID   = reader.GetInt32(0),
                    PatientID       = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                    DoctorID        = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                    AppointmentDate = reader["AppointmentDate"]?.ToString() ?? "",
                    Status          = L.TranslateStatus(reader["Status"]?.ToString()),
                    Notes           = reader["Notes"]?.ToString() ?? ""
                });
            }
            _apptCount = _appts.Count;
            ApptCount.Text = L.Format("ApptsCount", _apptCount);
        }
        catch { ApptCount.Text = L.Get("DbNotConnectedShort"); }
        ApptsGrid.ItemsSource = null;
        ApptsGrid.ItemsSource = _appts;
    }

    private void AddAppt_Click(object? s, RoutedEventArgs e)
    {
        _editingId = -1;
        FormTitle.Text = L.Get("FormAddAppointment");
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
            NpgsqlCommand cmd;
            if (_editingId == -1)
                cmd = new NpgsqlCommand(@"INSERT INTO ""Appointments"" (""PatientID"",""DoctorID"",""AppointmentDate"",""Status"",""Notes"") VALUES (@p,@d,@dt,@s,@n)", conn);
            else
            {
                cmd = new NpgsqlCommand(@"UPDATE ""Appointments"" SET ""PatientID""=@p,""DoctorID""=@d,""AppointmentDate""=@dt,""Status""=@s,""Notes""=@n WHERE ""AppointmentID""=@id", conn);
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
        FormTitle.Text    = L.Get("FormEditAppointment");
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
            var cmd = new NpgsqlCommand(@"DELETE FROM ""Appointments"" WHERE ""AppointmentID""=@id", conn);
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
