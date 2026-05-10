using Avalonia.Controls;
using Avalonia.Interactivity;
using ClinicCore.Database;
using ClinicCore.Models;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System;

namespace ClinicCore.Views;

public partial class PatientsView : UserControl
{
    private ObservableCollection<Patient> _patients = new();
    private int _editingId = -1;

    public PatientsView()
    {
        InitializeComponent();
        PatientsGrid.ItemsSource = _patients;
        LoadPatients();
    }

    private void LoadPatients()
    {
        _patients.Clear();
        try
        {
            using var conn = DBHelper.GetConnection();
            var cmd = new SqlCommand("SELECT * FROM Patients ORDER BY PatientID DESC", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                _patients.Add(new Patient
                {
                    PatientID   = reader.GetInt32(0),
                    FullName    = reader.GetString(1),
                    DateOfBirth = reader["DateOfBirth"]?.ToString() ?? "",
                    Gender      = reader["Gender"]?.ToString() ?? "",
                    Phone       = reader["Phone"]?.ToString() ?? "",
                    Address     = reader["Address"]?.ToString() ?? ""
                });
            }
            PatientCount.Text = $"{_patients.Count} patient(s) registered";
        }
        catch
        {
            PatientCount.Text = "DB not connected — showing offline mode";
        }
        PatientsGrid.ItemsSource = null;
    PatientsGrid.ItemsSource = _patients;
    }

    private void AddPatient_Click(object? s, RoutedEventArgs e)
    {
        _editingId = -1;
        FormTitle.Text = "Add New Patient";
        ClearForm();
        FormPanel.IsVisible = true;
    }

    private void CancelForm_Click(object? s, RoutedEventArgs e)
    {
        FormPanel.IsVisible = false;
        ClearForm();
    }

    private async void SavePatient_Click(object? s, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtName.Text)) return;
        try
        {
            using var conn = DBHelper.GetConnection();
            SqlCommand cmd;
            if (_editingId == -1)
            {
                cmd = new SqlCommand(@"INSERT INTO Patients 
                    (FullName, DateOfBirth, Gender, Phone, Address)
                    VALUES (@n, @d, @g, @p, @a)", conn);
            }
            else
            {
                cmd = new SqlCommand(@"UPDATE Patients SET 
                    FullName=@n, DateOfBirth=@d, Gender=@g, Phone=@p, Address=@a
                    WHERE PatientID=@id", conn);
                cmd.Parameters.AddWithValue("@id", _editingId);
            }
            cmd.Parameters.AddWithValue("@n", TxtName.Text);
            cmd.Parameters.AddWithValue("@d", TxtDOB.Text ?? "");
            cmd.Parameters.AddWithValue("@g", (CmbGender.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "");
            cmd.Parameters.AddWithValue("@p", TxtPhone.Text ?? "");
            cmd.Parameters.AddWithValue("@a", TxtAddress.Text ?? "");
            cmd.ExecuteNonQuery();
            FormPanel.IsVisible = false;
            ClearForm();
            LoadPatients();
        }
        catch (Exception ex)
        {
            var box = new Window
            {
                Title = "Error",
                Width = 500,
                Height = 200,
                Content = new TextBlock
                {
                    Text = ex.Message,
                    Margin = new Avalonia.Thickness(20),
                    TextWrapping = Avalonia.Media.TextWrapping.Wrap
                }
            };
            box.Show();
        }
    }

    private void EditPatient_Click(object? s, RoutedEventArgs e)
    {
        if (PatientsGrid.SelectedItem is not Patient p) return;
        _editingId = p.PatientID;
        FormTitle.Text = "Edit Patient";
        TxtName.Text    = p.FullName;
        TxtDOB.Text     = p.DateOfBirth;
        TxtPhone.Text   = p.Phone;
        TxtAddress.Text = p.Address;
        FormPanel.IsVisible = true;
    }

    private void DeletePatient_Click(object? s, RoutedEventArgs e)
    {
        if (PatientsGrid.SelectedItem is not Patient p) return;
        try
        {
            using var conn = DBHelper.GetConnection();
            var cmd = new SqlCommand("DELETE FROM Patients WHERE PatientID=@id", conn);
            cmd.Parameters.AddWithValue("@id", p.PatientID);
            cmd.ExecuteNonQuery();
        }
        catch { }
        LoadPatients();
    }

    private void ClearForm()
    {
        TxtName.Text = TxtDOB.Text = TxtPhone.Text = TxtAddress.Text = "";
        CmbGender.SelectedIndex = -1;
        _editingId = -1;
    }
}