using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Models
{
	[System.Serializable]
	public class BoundingBox
	{
		public Length width = new(10);
		public Length height = new(20);
		public Length depth = new(30);

		public Length pinX = new(0);
		public Length pinY = new(0);
		public Length pinZ = new(0);

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
			this.width = this.width == null ? new(w, units) : this.width.Assign(w, units);
			this.height = this.height == null ? new(h, units) : this.height.Assign(h, units);
			this.depth = this.depth == null ? new(d, units) : this.depth.Assign(d, units);
			return this;
		}
		public BoundingBox Pin(double x, double y, double z, string units = "m")
		{
			this.pinX = this.pinX == null ? new(x, units) : this.pinX.Assign(x, units);
			this.pinY = this.pinY == null ? new(y, units) : this.pinY.Assign(y, units);
			this.pinZ = this.pinZ == null ? new(z, units) : this.pinZ.Assign(z, units);
			return this;
		}
	}
}
