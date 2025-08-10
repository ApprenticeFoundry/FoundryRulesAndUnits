using System.Collections.Generic;

namespace FoundryRulesAndUnits.Models;


[System.Serializable]
public class DT_Title : DT_Searchable
{
    public bool IsExpanded { get; set; } = true;
    public List<DT_Comment>? Comments { get; set; } = null;
    public List<DT_QualityAssurance>? QualityChecks { get; set; } = null;



    public DT_Title() : base()
    {
    }
    public string GetTitle()
    {
        return Title ?? "No Title";
    }
    public string SetTitle(string value)
    {
        Title = value;
        return GetTitle();
    }
    public DT_Comment AddComment(DT_Comment item)
    {
        if (Comments == null)
        {
            Comments = new List<DT_Comment>();
        }

        Comments.Add(item);
        item.ParentGuid = this.Guid;
        return item;
    }

    public virtual List<DT_Comment> CollectComments(List<DT_Comment> list)
    {
        if (Comments != null)
        {
            list.AddRange(Comments);
        }
        return list;
    }

    public virtual List<DT_QualityAssurance> CollectQualityAssurance(List<DT_QualityAssurance> list)
    {

        if (QualityChecks != null)
        {
            list.AddRange(QualityChecks);
        }
        return list;
    }

    public DT_QualityAssurance AddQualityCheck(DT_QualityAssurance item)
    {
        if (QualityChecks == null)
        {
            QualityChecks = new List<DT_QualityAssurance>();
        }

        QualityChecks.Add(item);
        item.ParentGuid = this.Guid;
        return item;
    }

}
