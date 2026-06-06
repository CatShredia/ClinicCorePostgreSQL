using Avalonia.Controls;
using ClinicCore.Localization;

namespace ClinicCore.Views;

public abstract class LocalizableUserControl : UserControl
{
    protected LocalizableUserControl()
    {
        LocalizationService.LanguageChanged += OnLanguageChanged;
    }

    protected abstract void ApplyLocalization();

    private void OnLanguageChanged() => ApplyLocalization();
}
