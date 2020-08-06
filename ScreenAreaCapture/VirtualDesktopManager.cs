using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ScreenAreaCapture
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("a5cd92ff-29be-454c-8d04-d82879fb3f1b")]
    [System.Security.SuppressUnmanagedCodeSecurity]
    public interface IVirtualDesktopManager
    {
        [PreserveSig]
        int IsWindowOnCurrentVirtualDesktop(
            [In] IntPtr topLevelWindow,
            [Out] out int onCurrentDesktop
            );
        [PreserveSig]
        int GetWindowDesktopId(
            [In] IntPtr topLevelWindow,
            [Out] out Guid currentDesktop
            );

        [PreserveSig]
        int MoveWindowToDesktop(
            [In] IntPtr topLevelWindow,
            [MarshalAs(UnmanagedType.LPStruct)]
            [In]Guid currentDesktop
            );
    }

    public class NewWindow : Form
    {
    }
    [ComImport, Guid("aa509086-5ca9-4c25-8f95-589d3c07b48a")]
    public class CVirtualDesktopManager
    {

    }
    public class VirtualDesktopManager
    {
        public VirtualDesktopManager()
        {
            _cmanager = new CVirtualDesktopManager();
            _manager = (IVirtualDesktopManager) _cmanager;
        }
        ~VirtualDesktopManager()
        {
            _manager = null;
            _cmanager = null;
        }
        private CVirtualDesktopManager _cmanager;
        private IVirtualDesktopManager _manager;

        public bool IsWindowOnCurrentVirtualDesktop(IntPtr topLevelWindow)
        {
            int result;
            int hr;
            if ((hr = _manager.IsWindowOnCurrentVirtualDesktop(topLevelWindow, out result)) != 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }
            return result != 0;
        }

        public Guid GetWindowDesktopId(IntPtr topLevelWindow)
        {
            int hr;
            if ((hr = _manager.GetWindowDesktopId(topLevelWindow, out var result)) != 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }
            return result;
        }

        public void MoveWindowToDesktop(IntPtr topLevelWindow, Guid currentDesktop)
        {
            int hr;
            if ((hr = _manager.MoveWindowToDesktop(topLevelWindow, currentDesktop)) != 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }
        }
    }
}
