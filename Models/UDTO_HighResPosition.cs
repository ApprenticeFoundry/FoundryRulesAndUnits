using System;
using FoundryRulesAndUnits.Units;


namespace FoundryRulesAndUnits.Models
{


	[System.Serializable]
	public class UDTO_HighResPosition
	{
		public double xLoc;
		public double yLoc;
		public double zLoc;

		public double xAng;
		public double yAng;
		public double zAng;

		public UDTO_HighResPosition()
		{
			xLoc =0;
			yLoc = 0;
			zLoc = 0;

			xAng = 0;
			yAng = 0;
			zAng = 0;
		}
		public UDTO_HighResPosition(HighResPosition source): this()
		{
			copyFrom(source);
		}
		
		public UDTO_HighResPosition(double xLoc, double yLoc, double zLoc) : this()
		{
			this.Loc(xLoc, yLoc, zLoc);
		}



		public double distanceXZ()
		{
			return Math.Sqrt(this.xLoc * this.xLoc + this.zLoc * this.zLoc);
		}

		public double bearingXZ()
		{
			return Math.Atan2(this.xLoc, this.zLoc);
		}



		public UDTO_HighResPosition copyOther(UDTO_HighResPosition pos)
		{
			this.xLoc = pos.xLoc;
			this.yLoc = pos.yLoc;
			this.zLoc = pos.zLoc;
			this.xAng = pos.xAng;
			this.yAng = pos.yAng;
			this.zAng = pos.zAng;
			return this;
		}

		public UDTO_HighResPosition copyFrom(HighResPosition pos)
		{
			this.xLoc = pos.xLoc.Value();
			this.yLoc = pos.yLoc.Value();
			this.zLoc = pos.zLoc.Value();
			this.xAng = pos.xAng.Value();
			this.yAng = pos.yAng.Value();
			this.zAng = pos.zAng.Value();
			return this;
		}
		public UDTO_HighResPosition Loc(double xLoc, double yLoc, double zLoc)
		{
			this.xLoc = xLoc;
			this.yLoc = yLoc;
			this.zLoc = zLoc;
			return this;
		}
		public UDTO_HighResPosition Ang(double xAng, double yAng, double zAng)
		{
			this.xAng = xAng;
			this.yAng = yAng;
			this.zAng = zAng;
			return this;
		}

	}
}

