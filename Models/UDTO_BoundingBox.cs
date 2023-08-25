using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Models
{
	[System.Serializable]
	public class UDTO_BoundingBox
	{
		public double width;
		public double height;
		public double depth;

		public double pinX = 0;
		public double pinY = 0;
		public double pinZ = 0;

		public double scaleX = 1;
		public double scaleY = 1;
		public double scaleZ = 1;

		public UDTO_BoundingBox()
		{
		}

		public UDTO_BoundingBox(BoundingBox source) : this()
		{
			copyFrom(source);
		}

		public UDTO_BoundingBox(double width, double height, double depth) : this()
		{
			this.Box(width, height, depth);
		}


		public UDTO_BoundingBox copyFrom(BoundingBox pos)
		{
			this.width = pos.width.Value();
			this.height = pos.height.Value();
			this.depth = pos.depth.Value();
			this.pinX = pos.pinX.Value();
			this.pinY = pos.pinY.Value();
			this.pinZ = pos.pinZ.Value();
			this.scaleX = pos.scaleX;
			this.scaleY = pos.scaleY;
			this.scaleZ = pos.scaleZ;

			return this;
		}

		public UDTO_BoundingBox Scale(double scaleX, double scaleY, double scaleZ)
		{
			this.scaleX = scaleX;
			this.scaleY = scaleY;
			this.scaleZ = scaleZ;
			return this;
		}
		public UDTO_BoundingBox Scale(double scale)
		{
			this.scaleX = scale;
			this.scaleY = scale;
			this.scaleZ = scale;
			return this;
		}

		public UDTO_BoundingBox Box(double w, double h, double d)
		{
			this.width = w;
			this.height = h;
			this.depth = d;
			return this;
		}
		public UDTO_BoundingBox Pin(double x, double y, double z)
		{
			this.pinX = x;
			this.pinY = y;
			this.pinZ = z;
			return this;
		}
	}
}
