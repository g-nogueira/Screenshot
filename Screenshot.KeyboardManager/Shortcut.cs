using System.Runtime.CompilerServices;
using WK.Libraries.HotkeyListenerNS;

namespace Screenshot.KeyboardManager
{
    public static class Shortcut
    {
        /// <summary>
        /// Listens to keyboard shortcuts.
        /// </summary>
        /// <param name="keys">The hotkey to listen for.</param>
        /// <param name="callback">The function to be called when the hotkey is triggered.</param>
        public static HotkeyListener Listen(Hotkey keys, HotkeyListener.HotkeyEventHandler callback)
        {
            var hkl = new HotkeyListener();
            hkl.Add(keys);
            hkl.HotkeyPressed += callback;

            return hkl;
        }

        public static void UnListen(HotkeyListener listener, Hotkey keys)
        {
            listener.Remove(keys);
        }
        
        public static void Replace(HotkeyListener listener, Hotkey old, Hotkey _new) {
            listener.Update(old, _new);
        }
    }
}