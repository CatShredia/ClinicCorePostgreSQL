using Avalonia.Controls;
using Avalonia.Interactivity;
using ClinicCore.Database;
using ClinicCore.Models;
using Npgsql;
using System.Collections.ObjectModel;

namespace ClinicCore.Views;

public partial class PrescriptionsView : UserControl
{
    private ObservableCollection<Prescription> _rxs = new();
    private int _editingId = -1;

    public PrescriptionsView()
    {
        InitializeComponent();
        RxGrid.ItemsSource = _rxs;
        LoadRxs();
    }

    private void LoadRxs()
    {
        _rxs.Clear();
        try
        {
            using var conn = DBHelper.GetConnection();
            var cmd = new NpgsqlCommand(@"SELECT * FROM ""Prescriptions"" ORDER BY ""PrescriptionID"" DESC", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                _rxs.Add(new Prescription
                {
                    PrescriptionID = reader.GetInt32(0),
                    PatientID      = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                    DoctorID       = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                    Medication     = reader["Medication"]?.ToString() ?? "",
                    Dosage         = reader["Dosage"]?.ToString() ?? "",
                    IssuedDate     = reader["IssuedDate"]?.ToString() ?? "",
                    Notes          = reader["Notes"]?.ToString() ?? ""
                });
            }
            RxCount.Text = $"{_rxs.Count} prescription(s) issued";
        }
        catch { RxCount.Text = "DB not connected"; }
        RxGrid.ItemsSource = null;
        RxGrid.ItemsSource = _rxs;
    }

    private void AddRx_Click(object? s, RoutedEventArgs e)
    {
        _editingId = -1;
        FormTitle.Text = "Add New Prescription";
        ClearForm();
        FormPanel.IsVisible = true;
    }

    private void CancelForm_Click(object? s, RoutedEventArgs e)
    {
        FormPanel.IsVisible = false;
        ClearForm();
    }

    private void SaveRx_Click(object? s, RoutedEventArgs e)
    {
        try
        {
            using var conn = DBHelper.GetConnection();
            NpgsqlCommand cmd;
            if (_editingId == -1)
                cmd = new NpgsqlCommand(@"INSERT INTO ""Prescriptions"" (""PatientID"",""DoctorID"",""Medication"",""Dosage"",""Notes"") VALUES (@p,@d,@m,@ds,@n)", conn);
            else
            {
                cmd = new NpgsqlCommand(@"UPDATE ""Prescriptions"" SET ""PatientID""=@p,""DoctorID""=@d,""Medication""=@m,""Dosage""=@ds,""Notes""=@n WHERE ""PrescriptionID""=@id", conn);
                cmd.Parameters.AddWithValue("@id", _editingId);
            }
            cmd.Parameters.AddWithValue("@p", int.TryParse(TxtPatientID.Text, out var pid) ? pid : 0);
            cmd.Parameters.AddWithValue("@d", int.TryParse(TxtDoctorID.Text, out var did) ? did : 0);
            cmd.Parameters.AddWithValue("@m", TxtMedication.Text ?? "");
            cmd.Parameters.AddWithValue("@ds", TxtDosage.Text ?? "");
            cmd.Parameters.AddWithValue("@n", TxtNotes.Text ?? "");
            cmd.ExecuteNonQuery();
        }
        catch { }
        FormPanel.IsVisible = false;
        ClearForm();
        LoadRxs();
    }

    private void EditRx_Click(object? s, RoutedEventArgs e)
    {
        if (RxGrid.SelectedItem is not Prescription rx) return;
        _editingId        = rx.PrescriptionID;
        FormTitle.Text    = "Edit Prescription";
        TxtPatientID.Text = rx.PatientID.ToString();
        TxtDoctorID.Text  = rx.DoctorID.ToString();
        TxtMedication.Text = rx.Medication;
        TxtDosage.Text    = rx.Dosage;
        TxtNotes.Text     = rx.Notes;
        FormPanel.IsVisible = true;
    }

    private void DeleteRx_Click(object? s, RoutedEventArgs e)
    {
        if (RxGrid.SelectedItem is not Prescription rx) return;
        try
        {
            using var conn = DBHelper.GetConnection();
            var cmd = new NpgsqlCommand(@"DELETE FROM ""Prescriptions"" WHERE ""PrescriptionID""=@id", conn);
            cmd.Parameters.AddWithValue("@id", rx.PrescriptionID);
            cmd.ExecuteNonQuery();
        }
        catch { }
        LoadRxs();
    }

    private void ClearForm()
    {
        TxtPatientID.Text = TxtDoctorID.Text = TxtMedication.Text = TxtDosage.Text = TxtNotes.Text = "";
        _editingId = -1;
    }
}
