using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Models
{
	[System.Serializable]
	public class BoundingBox
	{
		public Length width;
		public Length height;
		public Length depth;

		public Length pinX;
		public Length pinY;
		public Length pinZ;

		public double scaleX = 1;
		public double scaleY = 1;
		public double scaleZ = 1;

		public BoundingBox()
		{
			var UnitFactory = new UnitSystem();
			width = UnitFactory.CreateUnit<Length>(10, "m");
			height = UnitFactory.CreateUnit<Length>(20, "m");
			depth = UnitFactory.CreateUnit<Length>(30, "m");

			pinX = UnitFactory.CreateUnit<Length>(0, "m");
			pinY = UnitFactory.CreateUnit<Length>(0, "m");
			pinZ = UnitFactory.CreateUnit<Length>(0, "m");
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
			var factory = new UnitSystem();
			this.width = this.width == null ? factory.CreateUnit<Length>(w, units) : this.width.Assign(w, units);
			this.height = this.height == null ? factory.CreateUnit<Length>(h, units) : this.height.Assign(h, units);
			this.depth = this.depth == null ? factory.CreateUnit<Length>(d, units) : this.depth.Assign(d, units);
			return this;
		}
		public BoundingBox Pin(double x, double y, double z, string units = "m")
		{
			var factory = new UnitSystem();
			this.pinX = this.pinX == null ? factory.CreateUnit<Length>(x, units) : this.pinX.Assign(x, units);
			this.pinY = this.pinY == null ? factory.CreateUnit<Length>(y, units) : this.pinY.Assign(y, units);
			this.pinZ = this.pinZ == null ? factory.CreateUnit<Length>(z, units) : this.pinZ.Assign(z, units);
			return this;
		}
	}
}
