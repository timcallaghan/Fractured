using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace Mandlebrott
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Panel panelControls;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxIterations;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.Splitter splitterStatusBar;
		private System.Windows.Forms.Panel panelZoomHistory;
		private System.Windows.Forms.Splitter splitterZoomHistory;
		private BufferedPanel panelPlot;

		private Bitmap m_PlotPanelImage = null;
		private Bitmap m_CaptureImage = null;
		private int m_nMaxIterations = 100;

		private Bounds m_bounds = new Bounds(-2.0f, 1.0f, -1.5f, 1.5f);
		private Bounds m_zoomBounds = null;
		private bool m_bIsZooming = false;

		private Point mouseDownPos = Point.Empty;
		private Color selectionColor; 
		private Brush selectionBrush; 
		private Color frameColor; 
		private Pen framePen;
		private Rectangle maxInvalidateRect = Rectangle.Empty;
		private System.Collections.ArrayList m_arrayBounds = new ArrayList();

		private int m_nAVIWidth;
		private int m_nAVIHeight;
		private Bitmap m_AVIImage = null;
		private int m_nMovieLength;
		private double m_dblShrinkRate;

		#region Resizing Code
		// Variables having to do with the resizing code follow
		protected bool bSizeMode = false;

		/// <summary>
		/// Windows Constants for WndProc call in BeginResize/EndResize events
		/// </summary>
		protected const int WM_SIZING = 0x214,
			WM_ENTERSIZEMOVE = 0x231,
			WM_EXITSIZEMOVE = 0x232,
			WM_SYSCOMMAND = 0x112,
			SC_SIZE = 0xF000;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxCaptureWidth;
		private System.Windows.Forms.Button buttonCapture;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.Label labelZoomHistory;
		private System.Windows.Forms.Button buttonReset;
		private System.Windows.Forms.Button buttonMakeMovie;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Label labelProgress;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBoxMovieLength;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBoxShrinkRate;
		private System.Windows.Forms.TextBox textBoxCaptureHeight;

		/// <summary>
		/// Process Windows messages (in this case to provide Begin/EndResize events
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref Message m)
		{
			switch ( m.Msg )
			{
				case WM_SIZING:
					// Resize event already provided
					break;
				case WM_SYSCOMMAND:
					bSizeMode = ((m.WParam.ToInt32() & 0xFFF0) == SC_SIZE);
					break;
				case WM_ENTERSIZEMOVE:
					if ( bSizeMode )
					{
						OnBeginResize(EventArgs.Empty);
					}
					break;
				case WM_EXITSIZEMOVE:
				{
					if ( bSizeMode )
					{
						OnEndResize(EventArgs.Empty);
					}
					break;
				}

			} // end switch
			base.WndProc (ref m);
		}

		/// <summary>
		/// Raise BeginResize event
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnBeginResize(System.EventArgs e)
		{
		}

		/// <summary>
		/// Raise EndResize event
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnEndResize(System.EventArgs e)
		{
			RedrawPlotPanelImage(true, true);
		}
		#endregion

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.MinimumSize = new Size(groupBox2.Right + 20 + groupBox1.Width, panelControls.Height + System.Windows.Forms.SystemInformation.CaptionHeight);
			
			selectionColor = Color.FromArgb(200, 0xE8, 0xED, 0xF5); 
			selectionBrush = new SolidBrush(selectionColor); 
			frameColor =  Color.FromArgb(0x33, 0x5E, 0xA8); 
			framePen = new Pen(frameColor);

			RedrawPlotPanelImage(false, false);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}

			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
			this.panelControls = new System.Windows.Forms.Panel();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxIterations = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.buttonReset = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.textBoxShrinkRate = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.textBoxMovieLength = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.buttonCapture = new System.Windows.Forms.Button();
			this.textBoxCaptureHeight = new System.Windows.Forms.TextBox();
			this.textBoxCaptureWidth = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.buttonMakeMovie = new System.Windows.Forms.Button();
			this.labelProgress = new System.Windows.Forms.Label();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.splitterStatusBar = new System.Windows.Forms.Splitter();
			this.panelZoomHistory = new System.Windows.Forms.Panel();
			this.labelZoomHistory = new System.Windows.Forms.Label();
			this.splitterZoomHistory = new System.Windows.Forms.Splitter();
			this.panelPlot = new Mandlebrott.BufferedPanel();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.panelControls.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.panelZoomHistory.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelControls
			// 
			this.panelControls.BackColor = System.Drawing.SystemColors.Control;
			this.panelControls.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelControls.Controls.Add(this.groupBox2);
			this.panelControls.Controls.Add(this.groupBox1);
			this.panelControls.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelControls.Location = new System.Drawing.Point(0, 0);
			this.panelControls.Name = "panelControls";
			this.panelControls.Size = new System.Drawing.Size(792, 134);
			this.panelControls.TabIndex = 0;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.textBoxIterations);
			this.groupBox2.Controls.Add(this.button1);
			this.groupBox2.Controls.Add(this.buttonReset);
			this.groupBox2.Location = new System.Drawing.Point(8, 8);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(217, 117);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Mandlebrot Parameters";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Number of Iterations:";
			// 
			// textBoxIterations
			// 
			this.textBoxIterations.Location = new System.Drawing.Point(128, 24);
			this.textBoxIterations.Name = "textBoxIterations";
			this.textBoxIterations.Size = new System.Drawing.Size(72, 20);
			this.textBoxIterations.TabIndex = 1;
			this.textBoxIterations.Text = "100";
			this.textBoxIterations.WordWrap = false;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(128, 69);
			this.button1.Name = "button1";
			this.button1.TabIndex = 3;
			this.button1.Text = "Calculate";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			this.button1.MouseEnter += new System.EventHandler(this.button1_MouseEnter);
			this.button1.MouseLeave += new System.EventHandler(this.button1_MouseLeave);
			// 
			// buttonReset
			// 
			this.buttonReset.Location = new System.Drawing.Point(8, 69);
			this.buttonReset.Name = "buttonReset";
			this.buttonReset.Size = new System.Drawing.Size(88, 23);
			this.buttonReset.TabIndex = 2;
			this.buttonReset.Text = "Reset";
			this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
			this.buttonReset.MouseEnter += new System.EventHandler(this.buttonReset_MouseEnter);
			this.buttonReset.MouseLeave += new System.EventHandler(this.buttonReset_MouseLeave);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.textBoxShrinkRate);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.textBoxMovieLength);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.buttonCapture);
			this.groupBox1.Controls.Add(this.textBoxCaptureHeight);
			this.groupBox1.Controls.Add(this.textBoxCaptureWidth);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.buttonMakeMovie);
			this.groupBox1.Controls.Add(this.labelProgress);
			this.groupBox1.Controls.Add(this.progressBar);
			this.groupBox1.Location = new System.Drawing.Point(338, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(445, 117);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Capture Image(s)";
			// 
			// textBoxShrinkRate
			// 
			this.textBoxShrinkRate.Location = new System.Drawing.Point(272, 56);
			this.textBoxShrinkRate.Name = "textBoxShrinkRate";
			this.textBoxShrinkRate.Size = new System.Drawing.Size(59, 20);
			this.textBoxShrinkRate.TabIndex = 109;
			this.textBoxShrinkRate.Text = "0.8";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(154, 56);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(109, 20);
			this.label5.TabIndex = 108;
			this.label5.Text = "Shrink Rate:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textBoxMovieLength
			// 
			this.textBoxMovieLength.Location = new System.Drawing.Point(272, 24);
			this.textBoxMovieLength.Name = "textBoxMovieLength";
			this.textBoxMovieLength.Size = new System.Drawing.Size(59, 20);
			this.textBoxMovieLength.TabIndex = 6;
			this.textBoxMovieLength.Text = "5";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(154, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(109, 20);
			this.label4.TabIndex = 107;
			this.label4.Text = "Movie Length (secs):";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// buttonCapture
			// 
			this.buttonCapture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCapture.Location = new System.Drawing.Point(350, 24);
			this.buttonCapture.Name = "buttonCapture";
			this.buttonCapture.Size = new System.Drawing.Size(88, 23);
			this.buttonCapture.TabIndex = 6;
			this.buttonCapture.Text = "Take Photo";
			this.buttonCapture.Click += new System.EventHandler(this.buttonCapture_Click);
			this.buttonCapture.MouseEnter += new System.EventHandler(this.buttonCapture_MouseEnter);
			this.buttonCapture.MouseLeave += new System.EventHandler(this.buttonCapture_MouseLeave);
			// 
			// textBoxCaptureHeight
			// 
			this.textBoxCaptureHeight.Location = new System.Drawing.Point(82, 56);
			this.textBoxCaptureHeight.Name = "textBoxCaptureHeight";
			this.textBoxCaptureHeight.Size = new System.Drawing.Size(59, 20);
			this.textBoxCaptureHeight.TabIndex = 5;
			this.textBoxCaptureHeight.Text = "400";
			// 
			// textBoxCaptureWidth
			// 
			this.textBoxCaptureWidth.Location = new System.Drawing.Point(82, 24);
			this.textBoxCaptureWidth.Name = "textBoxCaptureWidth";
			this.textBoxCaptureWidth.Size = new System.Drawing.Size(59, 20);
			this.textBoxCaptureWidth.TabIndex = 4;
			this.textBoxCaptureWidth.Text = "400";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(10, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 16);
			this.label3.TabIndex = 1;
			this.label3.Text = "Height:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(10, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 20);
			this.label2.TabIndex = 0;
			this.label2.Text = "Width:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// buttonMakeMovie
			// 
			this.buttonMakeMovie.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonMakeMovie.Location = new System.Drawing.Point(350, 56);
			this.buttonMakeMovie.Name = "buttonMakeMovie";
			this.buttonMakeMovie.Size = new System.Drawing.Size(88, 23);
			this.buttonMakeMovie.TabIndex = 7;
			this.buttonMakeMovie.Text = "Make Movie";
			this.buttonMakeMovie.Click += new System.EventHandler(this.buttonMakeMovie_Click);
			this.buttonMakeMovie.MouseEnter += new System.EventHandler(this.buttonMakeMovie_MouseEnter);
			this.buttonMakeMovie.MouseLeave += new System.EventHandler(this.buttonMakeMovie_MouseLeave);
			// 
			// labelProgress
			// 
			this.labelProgress.BackColor = System.Drawing.SystemColors.Control;
			this.labelProgress.Location = new System.Drawing.Point(10, 89);
			this.labelProgress.Name = "labelProgress";
			this.labelProgress.Size = new System.Drawing.Size(64, 16);
			this.labelProgress.TabIndex = 106;
			this.labelProgress.Text = "Progress:";
			this.labelProgress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(82, 89);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(356, 16);
			this.progressBar.TabIndex = 105;
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 544);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(792, 22);
			this.statusBar1.TabIndex = 4;
			this.statusBar1.Text = "Ready";
			// 
			// splitterStatusBar
			// 
			this.splitterStatusBar.BackColor = System.Drawing.SystemColors.WindowFrame;
			this.splitterStatusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitterStatusBar.Enabled = false;
			this.splitterStatusBar.Location = new System.Drawing.Point(0, 543);
			this.splitterStatusBar.Name = "splitterStatusBar";
			this.splitterStatusBar.Size = new System.Drawing.Size(792, 1);
			this.splitterStatusBar.TabIndex = 101;
			this.splitterStatusBar.TabStop = false;
			// 
			// panelZoomHistory
			// 
			this.panelZoomHistory.AutoScroll = true;
			this.panelZoomHistory.AutoScrollMargin = new System.Drawing.Size(4, 5);
			this.panelZoomHistory.BackColor = System.Drawing.SystemColors.Control;
			this.panelZoomHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelZoomHistory.Controls.Add(this.labelZoomHistory);
			this.panelZoomHistory.Dock = System.Windows.Forms.DockStyle.Left;
			this.panelZoomHistory.Location = new System.Drawing.Point(0, 134);
			this.panelZoomHistory.Name = "panelZoomHistory";
			this.panelZoomHistory.Size = new System.Drawing.Size(152, 409);
			this.panelZoomHistory.TabIndex = 102;
			// 
			// labelZoomHistory
			// 
			this.labelZoomHistory.Location = new System.Drawing.Point(6, 0);
			this.labelZoomHistory.Name = "labelZoomHistory";
			this.labelZoomHistory.Size = new System.Drawing.Size(123, 23);
			this.labelZoomHistory.TabIndex = 0;
			this.labelZoomHistory.Text = "Zoom History";
			this.labelZoomHistory.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// splitterZoomHistory
			// 
			this.splitterZoomHistory.BackColor = System.Drawing.SystemColors.ScrollBar;
			this.splitterZoomHistory.Location = new System.Drawing.Point(152, 134);
			this.splitterZoomHistory.MinExtra = 100;
			this.splitterZoomHistory.MinSize = 152;
			this.splitterZoomHistory.Name = "splitterZoomHistory";
			this.splitterZoomHistory.Size = new System.Drawing.Size(3, 409);
			this.splitterZoomHistory.TabIndex = 103;
			this.splitterZoomHistory.TabStop = false;
			this.splitterZoomHistory.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitterZoomHistory_SplitterMoved);
			this.splitterZoomHistory.MouseEnter += new System.EventHandler(this.splitterZoomHistory_MouseEnter);
			this.splitterZoomHistory.MouseLeave += new System.EventHandler(this.splitterZoomHistory_MouseLeave);
			// 
			// panelPlot
			// 
			this.panelPlot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelPlot.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelPlot.Location = new System.Drawing.Point(155, 134);
			this.panelPlot.Name = "panelPlot";
			this.panelPlot.Size = new System.Drawing.Size(637, 409);
			this.panelPlot.TabIndex = 104;
			this.panelPlot.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelPlot_MouseUp);
			this.panelPlot.Paint += new System.Windows.Forms.PaintEventHandler(this.panelPlot_Paint);
			this.panelPlot.MouseEnter += new System.EventHandler(this.panelPlot_MouseEnter);
			this.panelPlot.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelPlot_MouseMove);
			this.panelPlot.MouseLeave += new System.EventHandler(this.panelPlot_MouseLeave);
			this.panelPlot.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelPlot_MouseDown);
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.Filter = "JPeg Files|*.jpg|PNG Files|*.png|24-bit Bitmap|*.bmp";
			this.saveFileDialog.Title = "Save the Captured Image";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(792, 566);
			this.Controls.Add(this.panelPlot);
			this.Controls.Add(this.splitterZoomHistory);
			this.Controls.Add(this.panelZoomHistory);
			this.Controls.Add(this.splitterStatusBar);
			this.Controls.Add(this.statusBar1);
			this.Controls.Add(this.panelControls);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Name = "Form1";
			this.Text = "Fractured";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
			this.Resize += new System.EventHandler(this.Form1_Resize);
			this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseWheel);
			this.panelControls.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.panelZoomHistory.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		
		private void RedrawPlotPanelImage(bool ShowWaitCursor, bool fRefresh)
		{
			if (ShowWaitCursor)
			{
				Cursor.Current = Cursors.WaitCursor;
			}

			if (panelPlot.ClientRectangle.Width <= 0 || panelPlot.ClientRectangle.Height <= 0)
			{
				return;
			}

			if (m_PlotPanelImage != null)
			{
				m_PlotPanelImage.Dispose();
			}

			m_PlotPanelImage = new Bitmap(panelPlot.ClientRectangle.Width, panelPlot.ClientRectangle.Height);
			Mandlebrot.ComputePixelColours
				(
					m_PlotPanelImage,
					m_bounds,
					m_nMaxIterations
				);

			if (fRefresh)
			{
				this.Refresh();
			}
		}
		
		private void panelPlot_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			//get the Graphics object from the PaintEventArgs
			Graphics graphics = e.Graphics;
			graphics.DrawImageUnscaled(m_PlotPanelImage, 0, 0);
		
			// Now, if needed, draw the selection rectangle 
			if (mouseDownPos != Point.Empty) { 
				Point mousePos = panelPlot.PointToClient(MousePosition); 
				Rectangle selectedRect = new Rectangle( 
					Math.Min(mouseDownPos.X, mousePos.X), 
					Math.Min(mouseDownPos.Y, mousePos.Y), 
					Math.Abs(mousePos.X - mouseDownPos.X), 
					Math.Abs(mousePos.Y - mouseDownPos.Y)); 
				e.Graphics.FillRectangle(selectionBrush, selectedRect); 
				e.Graphics.DrawRectangle(framePen, selectedRect); 
			} 
		}

		private void Form1_Resize(object sender, System.EventArgs e)
		{
			// Only process if the user is not dragging the size bars
			if (!bSizeMode && this.WindowState != System.Windows.Forms.FormWindowState.Minimized)
			{
				if 
				(
					panelPlot.ClientRectangle.Width != m_PlotPanelImage.Width
					||
					panelPlot.ClientRectangle.Height != m_PlotPanelImage.Height
				)
				{
					RedrawPlotPanelImage(true, false);
				}

				this.Refresh();
			}
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			m_nMaxIterations = Int32.Parse(textBoxIterations.Text);

			RedrawPlotPanelImage(true, true);
		}

		private void panelPlot_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				int nWidth = panelPlot.ClientRectangle.Width;
				int nHeight = panelPlot.ClientRectangle.Height;

				double fDeltaX = (m_bounds.m_flXMax - m_bounds.m_flXMin)/nWidth;
				double fDeltaY = (m_bounds.m_flYMax - m_bounds.m_flYMin)/nHeight;
			
				double fReZ = m_bounds.m_flXMin + fDeltaX*e.X;
				double fImZ = m_bounds.m_flYMax - fDeltaY*e.Y;
				m_zoomBounds = new Bounds(fReZ, 0.0f, 0.0f, fImZ);

				mouseDownPos = new Point(e.X, e.Y);
			}
		}

		private void panelPlot_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && mouseDownPos != Point.Empty) 
			{
				m_bIsZooming = true;

				Point mousePos = panelPlot.PointToClient(MousePosition); 
				Rectangle selectedRect = new Rectangle( 
					Math.Min(mouseDownPos.X, mousePos.X), 
					Math.Min(mouseDownPos.Y, mousePos.Y), 
					Math.Abs(mousePos.X - mouseDownPos.X), 
					Math.Abs(mousePos.Y - mouseDownPos.Y));

				selectedRect.Inflate(1, 1);

				if (maxInvalidateRect == Rectangle.Empty)
				{
					maxInvalidateRect = selectedRect;
				}
				else
				{
					maxInvalidateRect = Rectangle.Union(maxInvalidateRect, selectedRect);
				}

				panelPlot.Invalidate(maxInvalidateRect);
			}
		}

		private void panelPlot_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (mouseDownPos != Point.Empty && m_bIsZooming)
			{
				Cursor.Current = Cursors.WaitCursor;
				m_bIsZooming = false;

				// Save the current contents of panelPlot in a bitmap
				Bitmap bitmapTemp = new Bitmap(m_PlotPanelImage);

				// Include the current selection rectangle
				Point mousePos = panelPlot.PointToClient(MousePosition); 
				Rectangle selectedRect = new Rectangle( 
					Math.Min(mouseDownPos.X, mousePos.X), 
					Math.Min(mouseDownPos.Y, mousePos.Y), 
					Math.Abs(mousePos.X - mouseDownPos.X), 
					Math.Abs(mousePos.Y - mouseDownPos.Y));
				
				Graphics graphics = Graphics.FromImage(bitmapTemp);
				graphics.FillRectangle(selectionBrush, selectedRect); 
				graphics.DrawRectangle(framePen, selectedRect);
				graphics.Dispose();

				AddImageToZoomHistory(bitmapTemp);

				mouseDownPos = Point.Empty;
				maxInvalidateRect = Rectangle.Empty;

				int nWidth = panelPlot.ClientRectangle.Width;
				int nHeight = panelPlot.ClientRectangle.Height;

				double fDeltaX = (m_bounds.m_flXMax - m_bounds.m_flXMin)/nWidth;
				double fDeltaY = (m_bounds.m_flYMax - m_bounds.m_flYMin)/nHeight;

				double fReZ = m_bounds.m_flXMin + fDeltaX*e.X;
				double fImZ = m_bounds.m_flYMax - fDeltaY*e.Y;

				m_zoomBounds.m_flXMax = fReZ;
				m_zoomBounds.m_flYMin = fImZ;

				m_zoomBounds.Normalise();

				m_bounds = m_zoomBounds;

				RedrawPlotPanelImage(true, true);
			}
		}

		private void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if 
			(
				e.KeyCode == Keys.Escape
				&&
				mouseDownPos != Point.Empty
			)
			{
				mouseDownPos = Point.Empty;
				panelPlot.Invalidate(maxInvalidateRect);
				maxInvalidateRect = Rectangle.Empty;
			}
		}

		private void AddImageToZoomHistory(Image image)
		{
			panelZoomHistory.SuspendLayout();

			int nPanelWidth = panelZoomHistory.Width - 12 - System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
			float flScale = (float)(nPanelWidth)/image.Width;
			int nPicBoxLocation = labelZoomHistory.Bottom + 6;
			int nPictureBoxCount = 1;
			for (int nIndex = 0; nIndex < panelZoomHistory.Controls.Count; ++nIndex)
			{
				if (panelZoomHistory.Controls[nIndex] is System.Windows.Forms.PictureBox)
				{
					PictureBox tempPicBox = (System.Windows.Forms.PictureBox)panelZoomHistory.Controls[nIndex];
					nPicBoxLocation = tempPicBox.Bottom + 6;
					++nPictureBoxCount;
				}
			}

			// Calculate possible new panel height and resize if necessary
			int nPanelHeight = nPicBoxLocation + (int)(image.Height*flScale) + 6;
			if (nPanelHeight > panelZoomHistory.Height)
			{
				panelZoomHistory.Height = nPanelHeight;
			}

			System.Windows.Forms.PictureBox pictureBox = new System.Windows.Forms.PictureBox();
			pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureBox.Width = nPanelWidth;
			pictureBox.Height = (int)(image.Height*flScale);
			pictureBox.Image = image;
			panelZoomHistory.Controls.Add(pictureBox);
			pictureBox.Location = new System.Drawing.Point(6, nPicBoxLocation);
			pictureBox.Name = "pictureBox" + (nPictureBoxCount - 1).ToString();
			pictureBox.TabIndex = nPictureBoxCount - 1; // Hijacked to store the bounds array index
			pictureBox.TabStop = false;
			pictureBox.DoubleClick += new System.EventHandler(this.pictureBox_DoubleClick);
			pictureBox.MouseEnter += new EventHandler(pictureBox_MouseEnter);
			pictureBox.MouseLeave += new EventHandler(pictureBox_MouseLeave);

			m_arrayBounds.Add(m_bounds);

			panelZoomHistory.ResumeLayout();
			panelZoomHistory.Refresh();
		}

		private void splitterZoomHistory_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			ResizeZoomHistoryPics();
			RedrawPlotPanelImage(false, false);
			panelPlot.Refresh();	
		}

		private void ResizeZoomHistoryPics()
		{
			panelZoomHistory.SuspendLayout();
			int nWidth = panelZoomHistory.Width - 12 - System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;

			Point currentScrollPos = new Point(Math.Abs(panelZoomHistory.AutoScrollPosition.X), Math.Abs(panelZoomHistory.AutoScrollPosition.Y));

			int nCurrentVPos = labelZoomHistory.Bottom + 6;
			int nOldHeight = labelZoomHistory.Height + 6;
			int nNewHeight = nOldHeight;

			for (int nIndex = 0; nIndex < panelZoomHistory.Controls.Count; ++nIndex)
			{
				if (panelZoomHistory.Controls[nIndex] is System.Windows.Forms.PictureBox)
				{
					PictureBox tempPicBox = (System.Windows.Forms.PictureBox)panelZoomHistory.Controls[nIndex];
					if (tempPicBox.Width != nWidth)
					{
						float flScale = (float)(nWidth)/tempPicBox.Width;
						tempPicBox.Width = nWidth;
						nOldHeight += tempPicBox.Height + 6;
						tempPicBox.Height = (int)(tempPicBox.Height * flScale);
						nNewHeight += tempPicBox.Height + 6;

						tempPicBox.Location = new Point(6, nCurrentVPos);

						nCurrentVPos = tempPicBox.Bottom + 6;
					}
				}
				else if (panelZoomHistory.Controls[nIndex] is System.Windows.Forms.Label)
				{
					Label tempLabel = (System.Windows.Forms.Label)panelZoomHistory.Controls[nIndex];
					if (tempLabel.Width != nWidth)
					{
						tempLabel.Width = nWidth;
					}
				}
			}

			panelZoomHistory.ResumeLayout();


			float flRatio = (float)(currentScrollPos.Y)/nOldHeight;
			currentScrollPos.Y = (int)(flRatio*nNewHeight);

			panelZoomHistory.AutoScrollPosition = currentScrollPos;
			panelZoomHistory.Refresh();
		}

		private void buttonCapture_Click(object sender, System.EventArgs e)
		{
			int nWidth = Int32.Parse(textBoxCaptureWidth.Text);
			int nHeight = Int32.Parse(textBoxCaptureHeight.Text);

			if (nWidth == 0 || nHeight == 0)
			{
				MessageBox.Show(this, "The width and height must be greater than zero", "Resolution error", MessageBoxButtons.OK);
				return;
			}

			Cursor.Current = Cursors.WaitCursor;

			if (m_CaptureImage != null)
			{
				m_CaptureImage.Dispose();
			}

			m_CaptureImage = new Bitmap(nWidth, nHeight);
			Bounds tempBounds = new Bounds(m_bounds);
			Mandlebrot.ComputePixelColours
				(
					m_CaptureImage,
					tempBounds,
					m_nMaxIterations
				);

			if (m_CaptureImage != null)
			{

				saveFileDialog.Filter = @"JPeg Files|*.jpg|PNG Files|*.png|24-bit Bitmap|*.bmp";
				saveFileDialog.Title = "Save the Captured Image";
				if(saveFileDialog.ShowDialog() == DialogResult.OK )
				{
					string fileName = saveFileDialog.FileName;
					if (fileName.EndsWith(".bmp"))
					{
						m_CaptureImage.Save(fileName, ImageFormat.Bmp);
					}
					else if (fileName.EndsWith(".png"))
					{
						m_CaptureImage.Save(fileName, ImageFormat.Png);
					}
					else if (fileName.EndsWith(".jpg"))
					{
						m_CaptureImage.Save(fileName, ImageFormat.Jpeg);
					}
				}
	
				m_CaptureImage.Dispose();
			}
		}

		private void buttonReset_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			m_bounds = new Bounds(-2.0f, 1.0f, -1.5f, 1.5f);
			textBoxIterations.Text = "100";
			m_nMaxIterations = 100;

			if (panelPlot.ClientRectangle.Width == 0 || panelPlot.ClientRectangle.Height == 0)
			{
				return;
			}

			if (m_PlotPanelImage != null)
			{
				m_PlotPanelImage.Dispose();
			}
			m_PlotPanelImage = new Bitmap(panelPlot.ClientRectangle.Width, panelPlot.ClientRectangle.Height);
			Mandlebrot.ComputePixelColours
				(
					m_PlotPanelImage,
					m_bounds,
					m_nMaxIterations
				);
			Refresh();		
		}

		private void pictureBox_DoubleClick(object sender, System.EventArgs e)
		{
			PictureBox tempPicBox = (PictureBox)sender;
			m_bounds = (Bounds)m_arrayBounds[tempPicBox.TabIndex];
			RedrawPlotPanelImage(true, true);
		}

		private void pictureBox_MouseEnter(object sender, EventArgs e)
		{
			statusBar1.Text = "Double click image to return to this zoom state";
		}

		private void pictureBox_MouseLeave(object sender, EventArgs e)
		{
			statusBar1.Text = "Ready";
		}

		private void panelPlot_MouseEnter(object sender, System.EventArgs e)
		{
			statusBar1.Text = "Left click and drag to zoom. Hit ESC to cancel zoom";
		}

		private void panelPlot_MouseLeave(object sender, System.EventArgs e)
		{
			statusBar1.Text = "Ready";
		}

		private void buttonCapture_MouseEnter(object sender, System.EventArgs e)
		{
			statusBar1.Text = "Click to save the current image to file using the specified resolution";
		}

		private void buttonCapture_MouseLeave(object sender, System.EventArgs e)
		{
			statusBar1.Text = "Ready";
		}

		private void splitterZoomHistory_MouseEnter(object sender, System.EventArgs e)
		{
			statusBar1.Text = "Drag the slider to resize the zoom history window";		
		}

		private void splitterZoomHistory_MouseLeave(object sender, System.EventArgs e)
		{
			statusBar1.Text = "Ready";		
		}

		private void Form1_MouseWheel(object sender, MouseEventArgs e)
		{
			// Find out if we are over the zoom history panel. If we are, scroll it!
			Point mousePos = panelZoomHistory.PointToClient(MousePosition);
			if (panelZoomHistory.ClientRectangle.Contains(mousePos))
			{
				int numberOfTextLinesToScroll = e.Delta * SystemInformation.MouseWheelScrollLines / 120;
				numberOfTextLinesToScroll *= 5;

				Point currentScrollPos = new Point(Math.Abs(panelZoomHistory.AutoScrollPosition.X), Math.Abs(panelZoomHistory.AutoScrollPosition.Y));
				currentScrollPos.Y -= numberOfTextLinesToScroll;
				panelZoomHistory.AutoScrollPosition = currentScrollPos;
			}
		}

		delegate void ShowProgressDelegate(int nTotalFrames, int nFramesSoFar);

		void ShowProgress(int nTotalFrames, int nFramesSoFar) 
		{
			// Make sure we're on the right thread
			if(progressBar.InvokeRequired == false ) 
			{
				progressBar.Maximum = nTotalFrames;
				progressBar.Value = nFramesSoFar;
			}
			else 
			{
				// Show progress asynchronously
				ShowProgressDelegate  showProgress = new ShowProgressDelegate(ShowProgress);
				BeginInvoke(showProgress, new object[] { nTotalFrames, nFramesSoFar});
			}
		}

		delegate void MakeMovieDelegate();

		private void buttonMakeMovie_Click(object sender, System.EventArgs e)
		{
			if (m_arrayBounds.Count < 1)
			{
				MessageBox.Show
					(
						"There must be at least 1 image in the zoom history to make a movie!",
						"Invalid number of zoom states",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error
					);
				return;
			}

			m_nAVIWidth = Int32.Parse(textBoxCaptureWidth.Text);
			m_nAVIHeight = Int32.Parse(textBoxCaptureHeight.Text);

			if (m_nAVIWidth <= 0 || m_nAVIHeight <= 0)
			{
				MessageBox.Show
					(
						"The width and height must be greater than zero",
						"Picture resolution error",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error
					);
				return;
			}

			m_nMovieLength = Int32.Parse(textBoxMovieLength.Text);
			if (m_nMovieLength <= 0)
			{
				MessageBox.Show
					(
						"The movie length must be greater than zero",
						"Movie Length error",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error
					);
				return;
			}

			m_dblShrinkRate = Double.Parse(textBoxShrinkRate.Text);
			if (m_dblShrinkRate <= 0.0 || m_dblShrinkRate >= 1.0)
			{
				MessageBox.Show
					(
						"The shrink rate must be between 0.0 and 1.0 exclusive",
						"Shrink Rate error",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error
					);
				return;
			}

			MakeMovieDelegate  makeMovie = new MakeMovieDelegate(MakeMovie);
			makeMovie.BeginInvoke(null, null);
		}

		delegate void SaveAVIDelegate(FileInfo fileInfo);

		void SaveAVI(FileInfo fileInfo) 
		{
			// Make sure we're on the right thread
			if(progressBar.InvokeRequired == false ) 
			{
				SaveFileDialog saveAVIFileDialog = new SaveFileDialog();
				saveAVIFileDialog.Filter = @"AVI Files|*.avi";
				saveAVIFileDialog.Title = "Save AVI file";
				FlashWindow.FlashWindowEx(this);
				if(saveAVIFileDialog.ShowDialog(this) == DialogResult.OK )
				{
					string fileName = saveAVIFileDialog.FileName;
					fileInfo.CopyTo(fileName, true);
					fileInfo.Delete();
				}
			}
			else 
			{
				// Show progress asynchronously
				SaveAVIDelegate  saveAVI = new SaveAVIDelegate(SaveAVI);
				BeginInvoke(saveAVI, new object[] {fileInfo});
			}
		}

		private void MakeMovie()
		{
			try 
			{
				UInt32 frameRate = 25;
				int nTotalFrames = (int)(frameRate)*m_nMovieLength;
				string strTempAVIFile = Application.StartupPath + "\\temp.avi";
				AviWriter aw = new AviWriter();

				if (m_AVIImage != null)
				{
					m_AVIImage.Dispose();
				}
				m_AVIImage = aw.Open(strTempAVIFile, frameRate, m_nAVIWidth, m_nAVIHeight);

				Bounds startBounds = (Bounds)m_arrayBounds[0];
				Bounds endBounds = new Bounds(m_bounds);

				double denom = 1.0 - Math.Pow(m_dblShrinkRate, (double)nTotalFrames);
				double deltaXMin = (endBounds.m_flXMin - startBounds.m_flXMin)*(1.0 - m_dblShrinkRate)/denom;
				double deltaXMax = (endBounds.m_flXMax - startBounds.m_flXMax)*(1.0 - m_dblShrinkRate)/denom;
				double deltaYMin = (endBounds.m_flYMin - startBounds.m_flYMin)*(1.0 - m_dblShrinkRate)/denom;
				double deltaYMax = (endBounds.m_flYMax - startBounds.m_flYMax)*(1.0 - m_dblShrinkRate)/denom;

				ShowProgress(nTotalFrames, 0);
				Bounds currentBounds = new Bounds(startBounds);
				for (int nIndex = 1; nIndex <= nTotalFrames; ++nIndex)
				{
					Mandlebrot.ComputePixelColours
						(
							m_AVIImage,
							currentBounds,
							m_nMaxIterations
						);

					aw.AddFrame();

					currentBounds.m_flXMin += deltaXMin*Math.Pow(m_dblShrinkRate, (double)(nIndex - 1));
					currentBounds.m_flXMax += deltaXMax*Math.Pow(m_dblShrinkRate, (double)(nIndex - 1));
					currentBounds.m_flYMin += deltaYMin*Math.Pow(m_dblShrinkRate, (double)(nIndex - 1));
					currentBounds.m_flYMax += deltaYMax*Math.Pow(m_dblShrinkRate, (double)(nIndex - 1));

					ShowProgress(nTotalFrames, nIndex);
				}
      
				aw.Close();

				FileInfo fileInfo = new FileInfo(strTempAVIFile);
				SaveAVI(fileInfo);					

				m_AVIImage.Dispose();

				ShowProgress(nTotalFrames, 0);
			}
			catch (AviException AviExcep) 
			{
				MessageBox.Show("AVI Exception in: " + AviExcep.ToString());
				m_AVIImage.Dispose();
			}
		}

		private void buttonMakeMovie_MouseEnter(object sender, System.EventArgs e)
		{
			if (m_arrayBounds.Count < 1)
			{
				statusBar1.Text = "There must be at least 1 image in the zoom history to make a movie";
			}
			else
			{
				statusBar1.Text = "Click to save the current zoom history to a movie";
			}
		}

		private void buttonMakeMovie_MouseLeave(object sender, System.EventArgs e)
		{
			statusBar1.Text = "Ready";
		}

		private void button1_MouseEnter(object sender, System.EventArgs e)
		{
			statusBar1.Text = "Click to recalculate the main image using the number of iterations";
		}

		private void button1_MouseLeave(object sender, System.EventArgs e)
		{
			statusBar1.Text = "Ready";
		}

		private void buttonReset_MouseEnter(object sender, System.EventArgs e)
		{
			statusBar1.Text = "Click to reset all parameters to their starting values";
		}

		private void buttonReset_MouseLeave(object sender, System.EventArgs e)
		{
			statusBar1.Text = "Ready";
		}
	}
}
