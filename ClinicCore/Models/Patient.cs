namespace ClinicCore.Models;

public class Patient
{
    public int PatientID { get; set; }
    public string FullName { get; set; } = "";
    public string DateOfBirth { get; set; } = "";
    public string Gender { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Address { get; set; } = "";
    public string CreatedAt { get; set; } = "";
}
