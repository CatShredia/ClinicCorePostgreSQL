namespace ClinicCore.Models;
public class Prescription
{
    public int PrescriptionID { get; set; }
    public int PatientID { get; set; }
    public int DoctorID { get; set; }
    public string Medication { get; set; } = "";
    public string Dosage { get; set; } = "";
    public string IssuedDate { get; set; } = "";
    public string Notes { get; set; } = "";
}
