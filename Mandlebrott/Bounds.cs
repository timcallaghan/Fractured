using System;

namespace Mandlebrott
{
	/// <summary>
	/// Storage for min and max Real and Imaginary parts of a complex number.
	/// </summary>
	public class Bounds
	{
		public Bounds(double flXMin, double flXMax, double flYMin, double flYMax)
		{
			m_flXMin = flXMin;
			m_flXMax = flXMax;
			m_flYMin = flYMin;
			m_flYMax = flYMax;
		}

		public Bounds(Bounds bounds)
		{
			m_flXMin = bounds.m_flXMin;
			m_flXMax = bounds.m_flXMax;
			m_flYMin = bounds.m_flYMin;
			m_flYMax = bounds.m_flYMax;
		}

		public void Normalise()
		{
			if (m_flXMax < m_flXMin)
			{
				double temp = m_flXMin;
				m_flXMin = m_flXMax;
				m_flXMax = temp;
			}

			if (m_flYMax < m_flYMin)
			{
				double temp = m_flYMin;
				m_flYMin = m_flYMax;
				m_flYMax = temp;
			}
		}

		public double m_flXMin;
		public double m_flXMax;
		public double m_flYMin;
		public double m_flYMax;
	}
}
