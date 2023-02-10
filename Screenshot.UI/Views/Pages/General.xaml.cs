using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Autofac;
using Microsoft.FSharp.Collections;
using Screenshot.Settings.API;
using SelectArea.Common;
using SelectArea.Helpers;
using WK.Libraries.HotkeyListenerNS;
using System.Windows.Input;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace SelectArea.Views.Pages;

public partial class General : Page
{
    private HashSet<Key> _hotkeyCaptureRegion = new();
    private SettingsAPI _settings;


    public General()
    {
        InitializeComponent();
        _settings = ContainerManager.Container.Resolve<SettingsAPI>();
        var hks = new HotkeySelector();
        hks.Enable(TB_RegionHotkey);

        TB_RegionHotkey.Text = HotKeyToText(StringToHotkey(_settings.Settings.hotkeySelectArea));
    }
    private void TB_RegionHotkey_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (_hotkeyCaptureRegion.Contains(e.Key)) return;
        
        _hotkeyCaptureRegion.Add(e.Key);
        TB_RegionHotkey.Text += e.Key;
    }

    private void TB_RegionHotkey_OnGotFocus(object sender, RoutedEventArgs e)
    {
        TB_RegionHotkey.Text = "";
    }

    private void TB_RegionHotkey_OnKeyUp(object sender, KeyEventArgs e)
    {
        var newSettings = new SettingsHelper.Settings
        {
            hotkeySelectArea = HotKeyToText(KeyListToHotKey(_hotkeyCaptureRegion))
        };
        
        _settings.UpdateSettings(newSettings);
    }

    private string HotKeyToText(Hotkey hotkey)
    {
        return hotkey.ToString();
    }

    private Hotkey KeyListToHotKey(ICollection<Key> keys)
    {
        return new Hotkey(string.Join("", keys));
    }

    private Hotkey StringToHotkey(string hotkey)
    {
        return new Hotkey(hotkey);
    }
}