using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Models
{
	[System.Serializable]
	public class BoundingBox
	{
		public Length width = UnitFactory.SI().CreateLength(10, "m");
		public Length height = UnitFactory.SI().CreateLength(20, "m");
		public Length depth = UnitFactory.SI().CreateLength(30, "m");

		public Length pinX = UnitFactory.SI().CreateLength(0, "m");
		public Length pinY = UnitFactory.SI().CreateLength(0, "m");
		public Length pinZ = UnitFactory.SI().CreateLength(0, "m");

		public double scaleX = 1;
		public double scaleY = 1;
		public double scaleZ = 1;

		public BoundingBox()
		{
		}


		public BoundingBox(BoundingBox source) : this()
		{
			copyFrom(source);
		}

		public BoundingBox(double width, double height, double depth, string units = "m") : this()
		{
			this.Box(width, height, depth, units);
		}

		public UDTO_BoundingBox AsUDTO()
		{
			return new UDTO_BoundingBox(this);
		}

		public BoundingBox copyFrom(BoundingBox pos)
		{
			this.width.Assign(pos.width);
			this.height.Assign(pos.height);
			this.depth.Assign(pos.depth);
			this.pinX.Assign(pos.pinX);
			this.pinY.Assign(pos.pinY);
			this.pinZ.Assign(pos.pinZ);
			this.scaleX = pos.scaleX;
			this.scaleY = pos.scaleY;
			this.scaleZ = pos.scaleZ;

			return this;
		}

		public BoundingBox Scale(double scaleX, double scaleY, double scaleZ)
		{
			this.scaleX = scaleX;
			this.scaleY = scaleY;
			this.scaleZ = scaleZ;
			return this;
		}
		public BoundingBox Scale(double scale)
		{
			this.scaleX = scale;
			this.scaleY = scale;
			this.scaleZ = scale;
			return this;
		}

		public BoundingBox Box(double w, double h, double d, string units = "m")
		{
			var factory = UnitFactory.SI();
			this.width = this.width == null ? factory.CreateLength(w, units) : this.width.Assign(w, units);
			this.height = this.height == null ? factory.CreateLength(h, units) : this.height.Assign(h, units);
			this.depth = this.depth == null ? factory.CreateLength(d, units) : this.depth.Assign(d, units);
			return this;
		}
		public BoundingBox Pin(double x, double y, double z, string units = "m")
		{
			var factory = UnitFactory.SI();
			this.pinX = this.pinX == null ? factory.CreateLength(x, units) : this.pinX.Assign(x, units);
			this.pinY = this.pinY == null ? factory.CreateLength(y, units) : this.pinY.Assign(y, units);
			this.pinZ = this.pinZ == null ? factory.CreateLength(z, units) : this.pinZ.Assign(z, units);
			return this;
		}
	}
}
