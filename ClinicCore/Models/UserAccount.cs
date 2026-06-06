namespace ClinicCore.Models;

public class UserAccount
{
    public int UserID { get; set; }
    public string Username { get; set; } = "";
    public string FullName { get; set; } = "";
    public string RoleName { get; set; } = "";
    public int RoleID { get; set; }
    public bool IsActive { get; set; }
    public string ActiveStatus { get; set; } = "";
    public string LastLoginAt { get; set; } = "";
}
