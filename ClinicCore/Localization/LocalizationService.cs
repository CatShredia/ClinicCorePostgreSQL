using System;

namespace ClinicCore.Localization;

public enum AppLanguage
{
    English,
    Russian
}

public static class LocalizationService
{
    public static AppLanguage Current { get; private set; } = AppLanguage.Russian;

    public static event Action? LanguageChanged;

    public static void SetLanguage(AppLanguage language)
    {
        if (Current == language) return;
        Current = language;
        LanguageChanged?.Invoke();
    }
}
