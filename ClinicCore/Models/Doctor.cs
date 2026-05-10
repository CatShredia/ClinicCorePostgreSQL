namespace ClinicCore.Models;

public class Doctor
{
    public int DoctorID { get; set; }
    public string FullName { get; set; } = "";
    public string Specialty { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Email { get; set; } = "";
}