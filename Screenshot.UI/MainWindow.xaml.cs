using System.Windows.Forms;
using SelectArea.Utilities;
using WK.Libraries.HotkeyListenerNS;
using Shortcut = Screenshot.KeyboardManager.Shortcut;

namespace SelectArea
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly HotkeyListener _hotkeyListener;
        public Hotkey HotkeySelectArea = new(Keys.Control, Keys.PrintScreen);
        public MainWindow()
        {
            InitializeComponent();
            _hotkeyListener = Shortcut.Listen(HotkeySelectArea,HotKeyPressed);
        }

        private void HotKeyPressed(object sender, HotkeyEventArgs e)
        {
            WindowUtilities.LaunchOCROverlayOnEveryScreen();
        }

        public void SetHotkeySelectArea(Hotkey hotkey)
        {
            Shortcut.Replace(_hotkeyListener, HotkeySelectArea, hotkey);
            HotkeySelectArea = hotkey;
        }
    }
}