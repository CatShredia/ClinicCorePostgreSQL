using Avalonia.Controls;
using Avalonia.Interactivity;
using ClinicCore.Database;
using ClinicCore.Models;
using Npgsql;
using System.Collections.ObjectModel;
using System;

namespace ClinicCore.Views;

public partial class DoctorsView : UserControl
{
    private ObservableCollection<Doctor> _doctors = new();
    private int _editingId = -1;

    public DoctorsView()
    {
        InitializeComponent();
        DoctorsGrid.ItemsSource = _doctors;
        LoadDoctors();
    }

    private void LoadDoctors()
    {
        _doctors.Clear();
        try
        {
            using var conn = DBHelper.GetConnection();
            var cmd = new NpgsqlCommand(@"SELECT * FROM ""Doctors"" ORDER BY ""DoctorID"" DESC", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                _doctors.Add(new Doctor
                {
                    DoctorID  = reader.GetInt32(0),
                    FullName  = reader.GetString(1),
                    Specialty = reader["Specialty"]?.ToString() ?? "",
                    Phone     = reader["Phone"]?.ToString() ?? "",
                    Email     = reader["Email"]?.ToString() ?? ""
                });
            }
            DoctorCount.Text = $"{_doctors.Count} doctor(s) registered";
        }
        catch { DoctorCount.Text = "DB not connected"; }
        DoctorsGrid.ItemsSource = null;
        DoctorsGrid.ItemsSource = _doctors;
    }

    private void AddDoctor_Click(object? s, RoutedEventArgs e)
    {
        _editingId = -1;
        FormTitle.Text = "Add New Doctor";
        ClearForm();
        FormPanel.IsVisible = true;
    }

    private void CancelForm_Click(object? s, RoutedEventArgs e)
    {
        FormPanel.IsVisible = false;
        ClearForm();
    }

    private void SaveDoctor_Click(object? s, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtName.Text)) return;
        try
        {
            using var conn = DBHelper.GetConnection();
            NpgsqlCommand cmd;
            if (_editingId == -1)
                cmd = new NpgsqlCommand(@"INSERT INTO ""Doctors"" (""FullName"",""Specialty"",""Phone"",""Email"") VALUES (@n,@s,@p,@e)", conn);
            else
            {
                cmd = new NpgsqlCommand(@"UPDATE ""Doctors"" SET ""FullName""=@n,""Specialty""=@s,""Phone""=@p,""Email""=@e WHERE ""DoctorID""=@id", conn);
                cmd.Parameters.AddWithValue("@id", _editingId);
            }
            cmd.Parameters.AddWithValue("@n", TxtName.Text);
            cmd.Parameters.AddWithValue("@s", TxtSpecialty.Text ?? "");
            cmd.Parameters.AddWithValue("@p", TxtPhone.Text ?? "");
            cmd.Parameters.AddWithValue("@e", TxtEmail.Text ?? "");
            cmd.ExecuteNonQuery();
        }
        catch { }
        FormPanel.IsVisible = false;
        ClearForm();
        LoadDoctors();
    }

    private void EditDoctor_Click(object? s, RoutedEventArgs e)
    {
        if (DoctorsGrid.SelectedItem is not Doctor d) return;
        _editingId = d.DoctorID;
        FormTitle.Text   = "Edit Doctor";
        TxtName.Text     = d.FullName;
        TxtSpecialty.Text = d.Specialty;
        TxtPhone.Text    = d.Phone;
        TxtEmail.Text    = d.Email;
        FormPanel.IsVisible = true;
    }

    private void DeleteDoctor_Click(object? s, RoutedEventArgs e)
    {
        if (DoctorsGrid.SelectedItem is not Doctor d) return;
        try
        {
            using var conn = DBHelper.GetConnection();
            var cmd = new NpgsqlCommand(@"DELETE FROM ""Doctors"" WHERE ""DoctorID""=@id", conn);
            cmd.Parameters.AddWithValue("@id", d.DoctorID);
            cmd.ExecuteNonQuery();
        }
        catch { }
        LoadDoctors();
    }

    private void ClearForm()
    {
        TxtName.Text = TxtSpecialty.Text = TxtPhone.Text = TxtEmail.Text = "";
        _editingId = -1;
    }
}
