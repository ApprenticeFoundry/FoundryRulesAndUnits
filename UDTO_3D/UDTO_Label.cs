using FoundryRulesAndUnits.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FoundryRulesAndUnits.Models;

[System.Serializable]
public class UDTO_Label : UDTO_Body
{
    public string? Size { get; set; }
    public List<string>? Details { get; set; }
    public string? TargetGuid { get; set; }


    public UDTO_Label() : base()
    {
    }
    public override UDTO_3D CopyFrom(UDTO_3D obj)
    {
        base.CopyFrom(obj);

        var label = obj as UDTO_Label;

        this.Size = label?.Size;
        this.Details = label?.Details;
        this.TargetGuid = label?.TargetGuid;

        if (this.Position == null)
        {
            this.Position = label?.Position;
        }
        else if (label?.Position != null)
        {
            this.Position.copyOther(label.Position);
        }

        return this;
    }

    public UDTO_Label CreateTextAt(string text, double xLoc = 0.0, double yLoc = 0.0, double zLoc = 0.0)
    {
        this.Text = text.Trim();
        this.Type = "Label";
        Position = new UDTO_HighResPosition(xLoc, yLoc, zLoc);

        return this;
    }

    public UDTO_Label CreateLabelAt(string text, List<string>? details = null, double xLoc = 0.0, double yLoc = 0.0, double zLoc = 0.0)
    {
        this.Text = text.Trim();
        this.Details = details;
        this.Type = "Label";

        Position = new UDTO_HighResPosition(xLoc, yLoc, zLoc);
        return this;
    }
}