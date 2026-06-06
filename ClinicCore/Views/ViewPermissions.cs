using Avalonia.Controls;
using ClinicCore.Auth;

namespace ClinicCore.Views;

public static class ViewPermissions
{
    public static void ApplyCrudButtons(string resource, Button btnAdd, Button btnEdit, Button btnDelete)
    {
        var canWrite = RolePermissions.CanWrite(AuthSession.CurrentUser?.Role, resource);
        btnAdd.IsVisible = canWrite;
        btnEdit.IsVisible = canWrite;
        btnDelete.IsVisible = canWrite;
    }
}
