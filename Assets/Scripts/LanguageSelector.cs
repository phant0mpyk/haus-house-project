using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageSelector : MonoBehaviour
{
    private bool _active = false;

    public void ChangeLanguage(int localeID)
    {
        if (_active) return;
        StartCoroutine(SetLocale(localeID));
    }
    public void ToggleNextLanguage()
    {
        int currentID = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
        int nextID = (currentID + 1) % LocalizationSettings.AvailableLocales.Locales.Count;
        ChangeLanguage(nextID);
    }
    private IEnumerator SetLocale(int _localeID)
    {
        _active = true;
        yield return LocalizationSettings.InitializationOperation;

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];

        _active = false;
    }
}