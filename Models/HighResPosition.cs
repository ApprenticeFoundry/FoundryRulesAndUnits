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
		public string order = "XYZ";

		public HighResPosition()
		{
			var factory = UnitFactory.SI();
			xLoc = factory.CreateLength(0, "m");
			yLoc = factory.CreateLength(0, "m");
			zLoc = factory.CreateLength(0, "m");

			xAng = factory.CreateAngle(0, "deg");
			yAng = factory.CreateAngle(0, "deg");
			zAng = factory.CreateAngle(0, "deg");
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
			this.order = pos.order;
			return this;
		}
		public HighResPosition Loc(double xLoc, double yLoc, double zLoc, string units = "m")
		{
			var factory = UnitFactory.SI();
			this.xLoc = this.xLoc == null ? factory.CreateLength(xLoc, units) : this.xLoc.Assign(xLoc, units);
			this.yLoc = this.yLoc == null ? factory.CreateLength(yLoc, units) : this.yLoc.Assign(yLoc, units);
			this.zLoc = this.zLoc == null ? factory.CreateLength(zLoc, units) : this.zLoc.Assign(zLoc, units);
			return this;
		}
		public HighResPosition Ang(double xAng, double yAng, double zAng, string units = "rad")
		{
			var factory = UnitFactory.SI();
			this.xAng = this.xAng == null ? factory.CreateAngle(xAng, units) : this.xAng.Assign(xAng, units);
			this.yAng = this.yAng == null ? factory.CreateAngle(yAng, units) : this.yAng.Assign(yAng, units);
			this.zAng = this.zAng == null ? factory.CreateAngle(zAng, units) : this.zAng.Assign(zAng, units);
			return this;
		}

	}
}

