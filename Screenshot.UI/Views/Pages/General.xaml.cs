using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.FSharp.Collections;
using WK.Libraries.HotkeyListenerNS;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace SelectArea.Views.Pages;

public partial class General : Page
{
    private HashSet<Key> _hotkeyCaptureRegion = new();
    private MainWindow _mainWindow;


    public General()
    {
        InitializeComponent();
    }
    public General(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;

        TB_RegionHotkey.Text = HotKeyToText(_mainWindow.HotkeySelectArea);
    }

    private void TB_RegionHotkey_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (!_hotkeyCaptureRegion.Contains(e.Key))
        {
            _hotkeyCaptureRegion.Add(e.Key);
        }
    }

    private void TB_RegionHotkey_OnGotFocus(object sender, RoutedEventArgs e)
    {
        _hotkeyCaptureRegion.Clear();
    }

    private void TB_RegionHotkey_OnKeyUp(object sender, KeyEventArgs e)
    {
        
        _mainWindow.SetHotkeySelectArea(KeyListToHotKey(_hotkeyCaptureRegion));
    }

    private string HotKeyToText(Hotkey hotkey)
    {
        return hotkey.ToString();
    }

    private Hotkey KeyListToHotKey(ICollection<Key> keys)
    {
        return new Hotkey(string.Join("", keys));
    }
}