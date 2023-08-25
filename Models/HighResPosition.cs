using System;
using FoundryRulesAndUnits.Units;


namespace FoundryRulesAndUnits.Models
{


	[System.Serializable]
	public class HighResPosition
	{
		public Length xLoc;
		public Length yLoc;
		public Length zLoc;

		public Angle xAng;
		public Angle yAng;
		public Angle zAng;

		public HighResPosition()
		{
			xLoc = new(0);
			yLoc = new(0);
			zLoc = new(0);

			xAng = new(0);
			yAng = new(0);
			zAng = new(0);
		}
		public HighResPosition(HighResPosition source): this()
		{
			copyFrom(source);
		}
		public HighResPosition(double xLoc, double yLoc, double zLoc, string units = "m") : this()
		{
			this.Loc(xLoc, yLoc, zLoc, units);
		}

		public UDTO_HighResPosition AsUDTO()
		{
			return new UDTO_HighResPosition(this);
		}

		// public double distanceXZ()
		// {
		// 	return Math.Sqrt(this.xLoc.V * this.xLoc.V + this.zLoc.V * this.zLoc.V);
		// }

		// public double bearingXZ()
		// {
		// 	return Math.Atan2(this.xLoc.V, this.zLoc.V);
		// }



		public HighResPosition copyFrom(HighResPosition pos)
		{
			this.xLoc.Assign(pos.xLoc);
			this.yLoc.Assign(pos.yLoc);
			this.zLoc.Assign(pos.zLoc);
			this.xAng.Assign(pos.xAng);
			this.yAng.Assign(pos.yAng);
			this.zAng.Assign(pos.zAng);
			return this;
		}
		public HighResPosition Loc(double xLoc, double yLoc, double zLoc, string units = "m")
		{
			this.xLoc = this.xLoc == null ? new(xLoc, units) : this.xLoc.Assign(xLoc, units);
			this.yLoc = this.yLoc == null ? new(yLoc, units) : this.yLoc.Assign(yLoc, units);
			this.zLoc = this.zLoc == null ? new(zLoc, units) : this.zLoc.Assign(zLoc, units);
			return this;
		}
		public HighResPosition Ang(double xAng, double yAng, double zAng, string units = "rad")
		{
			this.xAng = this.xAng == null ? new(xAng, units) : this.xAng.Assign(xAng, units);
			this.yAng = this.yAng == null ? new(yAng, units) : this.yAng.Assign(yAng, units);
			this.zAng = this.zAng == null ? new(zAng, units) : this.zAng.Assign(zAng, units);
			return this;
		}

	}
}

