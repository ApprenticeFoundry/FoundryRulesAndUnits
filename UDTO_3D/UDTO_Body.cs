using System.Collections.Generic;
using FoundryRulesAndUnits.Models;

namespace FoundryRulesAndUnits.Models;


[System.Serializable]
public class UDTO_Body : UDTO_3D
{
    public string? Text { get; set; }
    public UDTO_HighResPosition? Position { get; set; }
    public UDTO_BoundingBox? BoundingBox { get; set; }

    public string? Category { get; set; }
    public List<UDTO_Body>? Members { get; set; }

    public UDTO_Body() : base()
    {
    }
    public bool HasMembers()
    {
        return Members != null && Members.Count > 0;
    }
    public void ClearMembers()
    {
        Members = null;
    }
    public List<UDTO_Body> GetMembers()
    {
        Members ??= new List<UDTO_Body>();
        return Members;
    }

    public UDTO_Body AddMember(UDTO_Body child)
    {
        Members ??= new List<UDTO_Body>();
        child.ParentUniqueGuid = this.UniqueGuid;
        Members.Add(child);
        return child;
    }
    public override UDTO_3D CopyFrom(UDTO_3D obj)
    {
        base.CopyFrom(obj);

        var body = obj as UDTO_Body;
        this.SourceURL = body!.SourceURL;
        this.Text = body.Text;

        if (this.Position == null)
        {
            this.Position = body.Position;
        }
        else if (body.Position != null)
        {
            this.Position.copyOther(body.Position);
        }

        if (this.BoundingBox == null)
        {
            this.BoundingBox = body.BoundingBox;
        }
        else if (body.BoundingBox != null)
        {
            this.BoundingBox.copyOther(body.BoundingBox);
        }
        return this;
    }
    public UDTO_Body EstablishLoc(HighResPosition pos)
    {
        Position ??= new UDTO_HighResPosition();
        Position.copyFrom(pos);
        return this;
    }

    public UDTO_Body EstablishLoc(double x = 0.0, double y = 0.0, double z = 0.0)
    {
        Position ??= new UDTO_HighResPosition();
        Position.Loc(x, y, z);
        return this;
    }
    public UDTO_Body EstablishAng(double x = 0.0, double y = 0.0, double z = 0.0)
    {
        Position ??= new UDTO_HighResPosition();
        Position.Ang(x, y, z);
        return this;
    }
    public UDTO_Body EstablishBox(BoundingBox box)
    {
        BoundingBox ??= new UDTO_BoundingBox();
        BoundingBox.copyFrom(box);
        return this;
    }

    public UDTO_Body EstablishBox(double width = 1.0, double height = 1.0, double depth = 1.0)
    {
        BoundingBox ??= new UDTO_BoundingBox();
        BoundingBox.Box(width, height, depth);
        return this;
    }

    public UDTO_Body EstablishPivot(double px = 0.0, double py = 0.0, double pz = 0.0)
    {
        BoundingBox ??= new UDTO_BoundingBox();

        BoundingBox.Pin(px, py, pz);
        return this;
    }
}