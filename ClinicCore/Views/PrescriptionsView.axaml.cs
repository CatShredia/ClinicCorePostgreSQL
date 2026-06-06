using Avalonia.Controls;
using Avalonia.Interactivity;
using ClinicCore.Auth;
using ClinicCore.Database;
using ClinicCore.Localization;
using ClinicCore.Models;
using Npgsql;
using System.Collections.ObjectModel;

namespace ClinicCore.Views;

public partial class PrescriptionsView : LocalizableUserControl
{
    private ObservableCollection<Prescription> _rxs = new();
    private int _editingId = -1;
    private int _rxCount;

    public PrescriptionsView()
    {
        InitializeComponent();
        RxGrid.ItemsSource = _rxs;
        ApplyLocalization();
        LoadRxs();
    }

    protected override void ApplyLocalization()
    {
        TxtTitle.Text = L.Get("Prescriptions");
        BtnAdd.Content = L.Get("AddPrescription");
        LblPatientId.Text = L.Get("PatientId");
        LblDoctorId.Text = L.Get("DoctorId");
        LblMedication.Text = L.Get("Medication");
        LblDosage.Text = L.Get("Dosage");
        LblNotes.Text = L.Get("Notes");
        TxtPatientID.PlaceholderText = L.Get("PlaceholderPatientId");
        TxtDoctorID.PlaceholderText = L.Get("PlaceholderDoctorId");
        TxtMedication.PlaceholderText = L.Get("PlaceholderMedication");
        TxtDosage.PlaceholderText = L.Get("PlaceholderDosage");
        TxtNotes.PlaceholderText = L.Get("PlaceholderNotes");
        BtnCancel.Content = L.Get("Cancel");
        BtnSave.Content = L.Get("SavePrescription");
        BtnEdit.Content = L.Get("EditSelected");
        BtnDelete.Content = L.Get("DeleteSelected");
        ColId.Text = L.Get("ColId");
        ColPatientId.Text = L.Get("PatientId");
        ColDoctorId.Text = L.Get("DoctorId");
        ColMedication.Text = L.Get("Medication");
        ColDosage.Text = L.Get("Dosage");
        ColDate.Text = L.Get("IssuedDate");
        ColNotes.Text = L.Get("Notes");
        FormTitle.Text = _editingId == -1 ? L.Get("FormAddPrescription") : L.Get("FormEditPrescription");
        if (_rxCount > 0)
            RxCount.Text = L.Format("RxCount", _rxCount);

        ViewPermissions.ApplyCrudButtons("Prescriptions", BtnAdd, BtnEdit, BtnDelete);
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
            _rxCount = _rxs.Count;
            RxCount.Text = L.Format("RxCount", _rxCount);
        }
        catch { RxCount.Text = L.Get("DbNotConnectedShort"); }
        RxGrid.ItemsSource = null;
        RxGrid.ItemsSource = _rxs;
    }

    private void AddRx_Click(object? s, RoutedEventArgs e)
    {
        _editingId = -1;
        FormTitle.Text = L.Get("FormAddPrescription");
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
        FormTitle.Text    = L.Get("FormEditPrescription");
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
