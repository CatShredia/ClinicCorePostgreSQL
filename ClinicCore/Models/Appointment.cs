namespace ClinicCore.Models;
public class Appointment
{
    public int AppointmentID { get; set; }
    public int PatientID { get; set; }
    public int DoctorID { get; set; }
    public string AppointmentDate { get; set; } = "";
    public string Status { get; set; } = "";
    public string Notes { get; set; } = "";
}
