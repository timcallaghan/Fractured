using System.Windows.Forms;

namespace Mandlebrott
{
	public class BufferedPanel : System.Windows.Forms.Panel
	{
		public BufferedPanel()
		{
			SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
		}
	}		
}