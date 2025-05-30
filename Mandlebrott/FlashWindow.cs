using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Mandlebrott
{
	/// <summary>
	/// Summary description for FlashWindow.
	/// </summary>
	public class FlashWindow
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct FLASHWINFO
		{
			public UInt32 cbSize;
			public IntPtr hwnd;
			public UInt32 dwFlags;
			public UInt32 uCount;
			public UInt32 dwTimeout;
		}

		[DllImport("user32.dll")]
		static extern Int32 FlashWindowEx(ref FLASHWINFO pwfi);

		//Stop flashing. The system restores the window to its original state.
		public const UInt32 FLASHW_STOP = 0;
		//Flash the window caption.
		public const UInt32 FLASHW_CAPTION = 1;
		//Flash the taskbar button.
		public const UInt32 FLASHW_TRAY = 2;
		//Flash both the window caption and taskbar button.
		//This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags.
		public const UInt32 FLASHW_ALL = 3;
		//Flash continuously, until the FLASHW_STOP flag is set.
		public const UInt32 FLASHW_TIMER = 4;
		//Flash continuously until the window comes to the foreground.
		public const UInt32 FLASHW_TIMERNOFG = 12;

		/// <summary>
		/// Flashes a window until the window comes to the foreground
		/// Receives the form that will flash
		/// </summary>
		/// <param name="frm">The form to flash</param>
		/// <returns>whether or not the window needed flashing</returns>
		public static bool FlashWindowEx(Form frm)
		{
			IntPtr hWnd = frm.Handle;
			FLASHWINFO fInfo = new FLASHWINFO();

			fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
			fInfo.hwnd = hWnd;
			fInfo.dwFlags = FLASHW_ALL | FLASHW_TIMERNOFG;
			fInfo.uCount = UInt32.MaxValue;
			fInfo.dwTimeout = 0;

			return (FlashWindowEx(ref fInfo) == 0);
		}
	}
}
