using Avalonia.Controls;
using Avalonia.Interactivity;
using ClinicCore.Database;
using ClinicCore.Localization;
using ClinicCore.Models;
using Npgsql;
using System.Collections.ObjectModel;
using System;

namespace ClinicCore.Views;

public partial class PatientsView : LocalizableUserControl
{
    private ObservableCollection<Patient> _patients = new();
    private int _editingId = -1;
    private int _patientCount;

    public PatientsView()
    {
        InitializeComponent();
        PatientsGrid.ItemsSource = _patients;
        ApplyLocalization();
        LoadPatients();
    }

    protected override void ApplyLocalization()
    {
        TxtTitle.Text = L.Get("Patients");
        BtnAdd.Content = L.Get("AddPatient");
        LblFullName.Text = L.Get("FullName");
        LblDOB.Text = L.Get("DateOfBirth");
        LblGender.Text = L.Get("Gender");
        LblPhone.Text = L.Get("Phone");
        LblAddress.Text = L.Get("Address");
        TxtName.PlaceholderText = L.Get("PlaceholderFullName");
        TxtDOB.PlaceholderText = L.Get("PlaceholderDOB");
        TxtPhone.PlaceholderText = L.Get("PlaceholderPhone");
        TxtAddress.PlaceholderText = L.Get("PlaceholderAddress");
        BtnCancel.Content = L.Get("Cancel");
        BtnSave.Content = L.Get("SavePatient");
        BtnEdit.Content = L.Get("EditSelected");
        BtnDelete.Content = L.Get("DeleteSelected");
        ColId.Text = L.Get("ColId");
        ColFullName.Text = L.Get("FullName");
        ColDOB.Text = L.Get("DateOfBirth");
        ColGender.Text = L.Get("Gender");
        ColPhone.Text = L.Get("Phone");
        ColAddress.Text = L.Get("Address");

        var genderIndex = CmbGender.SelectedIndex;
        CmbGender.Items.Clear();
        CmbGender.Items.Add(new ComboBoxItem { Content = L.Get("Male"), Tag = "Male" });
        CmbGender.Items.Add(new ComboBoxItem { Content = L.Get("Female"), Tag = "Female" });
        if (genderIndex >= 0 && genderIndex < CmbGender.Items.Count)
            CmbGender.SelectedIndex = genderIndex;

        FormTitle.Text = _editingId == -1 ? L.Get("FormAddPatient") : L.Get("FormEditPatient");
        if (_patientCount > 0)
            PatientCount.Text = L.Format("PatientsCount", _patientCount);

        if (_patients.Count > 0)
            LoadPatients();
    }

    private void LoadPatients()
    {
        _patients.Clear();
        try
        {
            using var conn = DBHelper.GetConnection();
            var cmd = new NpgsqlCommand(@"SELECT * FROM ""Patients"" ORDER BY ""PatientID"" DESC", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                _patients.Add(new Patient
                {
                    PatientID   = reader.GetInt32(0),
                    FullName    = reader.GetString(1),
                    DateOfBirth = reader["DateOfBirth"]?.ToString() ?? "",
                    Gender      = L.TranslateGender(reader["Gender"]?.ToString()),
                    Phone       = reader["Phone"]?.ToString() ?? "",
                    Address     = reader["Address"]?.ToString() ?? ""
                });
            }
            _patientCount = _patients.Count;
            PatientCount.Text = L.Format("PatientsCount", _patientCount);
        }
        catch
        {
            PatientCount.Text = L.Get("DbOffline");
        }
        PatientsGrid.ItemsSource = null;
        PatientsGrid.ItemsSource = _patients;
    }

    private void AddPatient_Click(object? s, RoutedEventArgs e)
    {
        _editingId = -1;
        FormTitle.Text = L.Get("FormAddPatient");
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
            NpgsqlCommand cmd;
            if (_editingId == -1)
            {
                cmd = new NpgsqlCommand(@"INSERT INTO ""Patients"" 
                    (""FullName"", ""DateOfBirth"", ""Gender"", ""Phone"", ""Address"")
                    VALUES (@n, @d, @g, @p, @a)", conn);
            }
            else
            {
                cmd = new NpgsqlCommand(@"UPDATE ""Patients"" SET 
                    ""FullName""=@n, ""DateOfBirth""=@d, ""Gender""=@g, ""Phone""=@p, ""Address""=@a
                    WHERE ""PatientID""=@id", conn);
                cmd.Parameters.AddWithValue("@id", _editingId);
            }
            var genderItem = CmbGender.SelectedItem as ComboBoxItem;
            var genderDb = genderItem?.Tag?.ToString() ?? "Male";

            cmd.Parameters.AddWithValue("@n", TxtName.Text);
            cmd.Parameters.AddWithValue("@d", TxtDOB.Text ?? "");
            cmd.Parameters.AddWithValue("@g", genderDb);
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
                Title = L.Get("Error"),
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
        FormTitle.Text = L.Get("FormEditPatient");
        TxtName.Text    = p.FullName;
        TxtDOB.Text     = p.DateOfBirth;
        TxtPhone.Text   = p.Phone;
        TxtAddress.Text = p.Address;
        CmbGender.SelectedIndex = p.Gender == L.Get("Female") ? 1 : 0;
        FormPanel.IsVisible = true;
    }

    private void DeletePatient_Click(object? s, RoutedEventArgs e)
    {
        if (PatientsGrid.SelectedItem is not Patient p) return;
        try
        {
            using var conn = DBHelper.GetConnection();
            var cmd = new NpgsqlCommand(@"DELETE FROM ""Patients"" WHERE ""PatientID""=@id", conn);
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
