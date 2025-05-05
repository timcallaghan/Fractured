using System;
using System.Drawing.Imaging;
using System.Drawing;

namespace Mandlebrott
{
	/// <summary>
	/// Summary description for Mandlebrot.
	/// </summary>
	public class Mandlebrot
	{
		public static void ComputePixelColours
		(
			Bitmap currentBitmap,
			Bounds bounds,
			int nMaxIterations
		)
		{
			int nWidth = currentBitmap.Width;
			int nHeight = currentBitmap.Height;

			if (nWidth == 0 || nHeight == 0)
			{
				return;
			}

			double fDeltaX = (bounds.m_flXMax - bounds.m_flXMin)/nWidth;
			double fDeltaY = (bounds.m_flYMax - bounds.m_flYMin)/nHeight;

			for (int nX = 0; nX < nWidth; ++nX)
			{
				for (int nY = 0; nY < nHeight; ++nY)
				{
					double fReZ = bounds.m_flXMin + fDeltaX*nX;
					double fReZ0 = fReZ;
					double fImZ = bounds.m_flYMax - fDeltaY*nY;
					double fImZ0 = fImZ;

					int nIteration = 0;
 
					while 
					(
						(fReZ*fReZ + fImZ*fImZ < (2*2))  
						&&  
						(nIteration < nMaxIterations) 
					) 
					{
					 
						double xtemp = fReZ*fReZ - fImZ*fImZ + fReZ0;
						fImZ = 2.0f*fReZ*fImZ + fImZ0;

						fReZ = xtemp;

						++nIteration;
					}
 
					if ( nIteration == nMaxIterations )
					{
						currentBitmap.SetPixel(nX, nY, Color.Black);
					}
					else
					{
						if (nIteration < nMaxIterations/20)
						{
							currentBitmap.SetPixel(nX, nY, Color.Red);
						}
						else if (nIteration < 2*nMaxIterations/20)
						{
							currentBitmap.SetPixel(nX, nY, Color.Green);
						}
						else if (nIteration < 3*nMaxIterations/20)
						{
							currentBitmap.SetPixel(nX, nY, Color.Blue);
						}
						else if (nIteration < 4*nMaxIterations/20)
						{
							currentBitmap.SetPixel(nX, nY, Color.Magenta);
						}
						else if (nIteration < 5*nMaxIterations/20)
						{
							currentBitmap.SetPixel(nX, nY, Color.Beige);
						}
						else if (nIteration < 6*nMaxIterations/20)
						{
							currentBitmap.SetPixel(nX, nY, Color.Azure);
						}
						else if (nIteration < 7*nMaxIterations/20)
						{
							currentBitmap.SetPixel(nX, nY, Color.Aquamarine);
						}
						else if (nIteration < 8*nMaxIterations/20)
						{
							currentBitmap.SetPixel(nX, nY, Color.Yellow);
						}
						else if (nIteration < 9*nMaxIterations/20)
						{
							currentBitmap.SetPixel(nX, nY, Color.Turquoise);
						}
						else if (nIteration < 10*nMaxIterations/20)
						{
							currentBitmap.SetPixel(nX, nY, Color.BurlyWood);
						}
						else if (nIteration < 11*nMaxIterations/20)
						{
							currentBitmap.SetPixel(nX, nY, Color.Thistle);
						}
						else if (nIteration < 12*nMaxIterations/20)
						{
							currentBitmap.SetPixel(nX, nY, Color.Violet);
						}
						else if (nIteration < 13*nMaxIterations/20)
						{
							currentBitmap.SetPixel(nX, nY, Color.YellowGreen);
						}
						else if (nIteration < 14*nMaxIterations/20)
						{
							currentBitmap.SetPixel(nX, nY, Color.Silver);
						}
						else if (nIteration < 15*nMaxIterations/20)
						{
							currentBitmap.SetPixel(nX, nY, Color.DarkOliveGreen);
						}
						else if (nIteration < 16*nMaxIterations/20)
						{
							currentBitmap.SetPixel(nX, nY, Color.HotPink);
						}
						else if (nIteration < 17*nMaxIterations/20)
						{
							currentBitmap.SetPixel(nX, nY, Color.Honeydew);
						}
						else if (nIteration < 18*nMaxIterations/20)
						{
							currentBitmap.SetPixel(nX, nY, Color.PeachPuff);
						}
						else if (nIteration < 19*nMaxIterations/20)
						{
							currentBitmap.SetPixel(nX, nY, Color.DarkSalmon);
						}
						else if (nIteration < nMaxIterations)
						{
							currentBitmap.SetPixel(nX, nY, Color.SlateBlue);
						}
					}
				}
			}
		}
	}
}
