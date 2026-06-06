using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ClinicCore.Auth;
using ClinicCore.Localization;
using ClinicCore.Models;
using ClinicCore.Services;
using System.Collections.ObjectModel;

namespace ClinicCore.Views;

public partial class UsersView : LocalizableUserControl
{
    private ObservableCollection<UserAccount> _users = new();
    private List<RoleItem> _roles = new();
    private int _editingId = -1;
    private int _userCount;

    public UsersView()
    {
        InitializeComponent();
        UsersGrid.ItemsSource = _users;
        _roles = UserService.GetRoles();
        ApplyLocalization();
        LoadUsers();
    }

    protected override void ApplyLocalization()
    {
        TxtTitle.Text = L.Get("Users");
        BtnAdd.Content = L.Get("AddUser");
        LblUsername.Text = L.Get("Username");
        LblPassword.Text = L.Get("Password");
        LblFullName.Text = L.Get("FullName");
        LblRole.Text = L.Get("UserRoleLabel");
        ChkActive.Content = L.Get("Active");
        LblPasswordHint.Text = _editingId == -1 ? "" : L.Get("LeaveBlankToKeep");
        TxtUsername.PlaceholderText = L.Get("PlaceholderUsername");
        TxtPassword.PlaceholderText = L.Get("PlaceholderPassword");
        TxtFullName.PlaceholderText = L.Get("PlaceholderFullName");
        BtnCancel.Content = L.Get("Cancel");
        BtnSave.Content = L.Get("SaveUser");
        BtnEdit.Content = L.Get("EditSelected");
        BtnDelete.Content = L.Get("DeleteSelected");
        ColId.Text = L.Get("ColId");
        ColUsername.Text = L.Get("Username");
        ColFullName.Text = L.Get("FullName");
        ColRole.Text = L.Get("UserRoleLabel");
        ColActive.Text = L.Get("Active");
        ColLastLogin.Text = L.Get("LastLogin");
        FormTitle.Text = _editingId == -1 ? L.Get("FormAddUser") : L.Get("FormEditUser");

        var roleIndex = CmbRole.SelectedIndex;
        CmbRole.Items.Clear();
        foreach (var role in _roles)
            CmbRole.Items.Add(new ComboBoxItem { Content = L.TranslateRole(role.RoleName), Tag = role.RoleID });
        if (roleIndex >= 0 && roleIndex < CmbRole.Items.Count)
            CmbRole.SelectedIndex = roleIndex;

        LblAccessMatrix.Text = L.Get("AccessMatrixTitle");
        UpdateAccessMatrix();

        if (_userCount > 0)
            UserCount.Text = L.Format("UsersCount", _userCount);

        if (_users.Count > 0)
            LoadUsers();
    }

    private void LoadUsers()
    {
        _users.Clear();
        try
        {
            foreach (var u in UserService.GetAll())
            {
                u.RoleName = L.TranslateRole(u.RoleName);
                _users.Add(u);
            }
            _userCount = _users.Count;
            UserCount.Text = L.Format("UsersCount", _userCount);
        }
        catch
        {
            UserCount.Text = L.Get("DbNotConnectedShort");
        }
        UsersGrid.ItemsSource = null;
        UsersGrid.ItemsSource = _users;
    }

    private void AddUser_Click(object? s, RoutedEventArgs e)
    {
        _editingId = -1;
        FormTitle.Text = L.Get("FormAddUser");
        TxtUsername.IsEnabled = true;
        ChkActive.IsVisible = false;
        LblPasswordHint.Text = "";
        ClearForm();
        FormPanel.IsVisible = true;
        UpdateAccessMatrix();
    }

    private void CmbRole_Changed(object? s, SelectionChangedEventArgs e)
        => UpdateAccessMatrix();

    private void UpdateAccessMatrix()
    {
        var roleName = GetSelectedRoleName();
        TxtAccessMatrix.Text = AccessMatrixFormatter.ForRole(roleName);
    }

    private string? GetSelectedRoleName()
    {
        if (CmbRole.SelectedItem is ComboBoxItem item && item.Tag is int roleId)
            return _roles.FirstOrDefault(r => r.RoleID == roleId)?.RoleName;
        return _roles.Count > 0 ? _roles[0].RoleName : "Admin";
    }

    private void CancelForm_Click(object? s, RoutedEventArgs e)
    {
        FormPanel.IsVisible = false;
        ClearForm();
    }

    private void SaveUser_Click(object? s, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtUsername.Text) && _editingId == -1) return;
        if (string.IsNullOrWhiteSpace(TxtFullName.Text)) return;
        if (_editingId == -1 && string.IsNullOrWhiteSpace(TxtPassword.Text)) return;

        var roleId = CmbRole.SelectedItem is ComboBoxItem item && item.Tag is int id ? id : 1;

        try
        {
            if (_editingId == -1)
                UserService.Create(TxtUsername.Text!, TxtPassword.Text!, TxtFullName.Text!, roleId);
            else
                UserService.Update(_editingId, TxtFullName.Text!, roleId, ChkActive.IsChecked == true, TxtPassword.Text);
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
            return;
        }

        FormPanel.IsVisible = false;
        ClearForm();
        LoadUsers();
    }

    private void EditUser_Click(object? s, RoutedEventArgs e)
    {
        if (UsersGrid.SelectedItem is not UserAccount u) return;
        _editingId = u.UserID;
        FormTitle.Text = L.Get("FormEditUser");
        TxtUsername.Text = u.Username;
        TxtUsername.IsEnabled = false;
        TxtFullName.Text = u.FullName;
        TxtPassword.Text = "";
        ChkActive.IsVisible = true;
        ChkActive.IsChecked = u.IsActive;
        LblPasswordHint.Text = L.Get("LeaveBlankToKeep");

        for (var i = 0; i < CmbRole.Items.Count; i++)
        {
            if ((CmbRole.Items[i] as ComboBoxItem)?.Tag is int id && id == u.RoleID)
            {
                CmbRole.SelectedIndex = i;
                break;
            }
        }
        FormPanel.IsVisible = true;
        UpdateAccessMatrix();
    }

    private void DeleteUser_Click(object? s, RoutedEventArgs e)
    {
        if (UsersGrid.SelectedItem is not UserAccount u) return;
        if (AuthSession.CurrentUser?.UserId == u.UserID)
        {
            ShowError(L.Get("CannotDeleteSelf"));
            return;
        }
        try
        {
            UserService.Delete(u.UserID);
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
            return;
        }
        LoadUsers();
    }

    private void ClearForm()
    {
        TxtUsername.Text = TxtPassword.Text = TxtFullName.Text = "";
        CmbRole.SelectedIndex = 0;
        ChkActive.IsChecked = true;
        _editingId = -1;
    }

    private void ShowError(string message)
    {
        var box = new Window
        {
            Title = L.Get("Error"),
            Width = 450,
            Height = 180,
            Content = new TextBlock
            {
                Text = message,
                Margin = new Avalonia.Thickness(20),
                TextWrapping = Avalonia.Media.TextWrapping.Wrap
            }
        };
        box.Show();
    }
}
