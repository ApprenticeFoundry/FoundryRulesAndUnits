using System;

namespace FoundryRulesAndUnits.Models;


public interface ITags
{
	List<string> GetTags();
	DT_Base AddTag(string tag);
	void Fill_DT_Component(DT_Component component);
}

public class DT_Base
{
	public string? Guid { get; set; }
	public string? ParentGuid { get; set; }
	public string? Name { get; set; }
	public string? Type { get; set; }
	public string? Url { get; set; }
    public List<string> Tags { get; set; }

	public string? TimeStamp;

	protected ControlParameters? metadata;


	public DT_Base()
	{
		this.initialize();
	}
	public DT_Base(string name)
	{
		this.Name = name;
		this.initialize();
	}
	public virtual T Merge<T>(T obj) where T : DT_Base
	{
		if (this.TimeStamp?.CompareTo(obj.TimeStamp) < 0)
		{
			this.TimeStamp = obj.TimeStamp;
		}
		return (this as T)!;
	}

	public List<string> GetTags()
    {
		if (Tags == null)
		{
			Tags = new List<string>();
        }
        return Tags;
    }
	
    public DT_Base AddTag(string tag)
	{
		if (string.IsNullOrEmpty(tag))
			return this;

		var tags = GetTags();

        var tagUpper = tag.ToUpper();
		if (tags.Contains(tagUpper))
			return this;

        tags.Add(tag);
		return this;
	}

	public virtual void Fill_DT_Component(DT_Component component)
	{
		foreach (var item in GetTags())
			component.AddTag(item);
		
		if (HasMetaData())
			component.MergeMetaData(metadata);
	}

	public string ResetTimeStamp()
	{
		this.TimeStamp = DateTime.UtcNow.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
		return this.TimeStamp;
	}

	public static string AsTopic(string name)
	{
		return name.Replace("DT_", "");
	}

	public static string AsTopic<T>() where T : DT_Base
	{
		return DT_Base.AsTopic(typeof(T).Name);
	}

	public static string AsTopicLower<T>() where T : DT_Base
	{
		return DT_Base.AsTopic(typeof(T).Name).ToLower();
	}


	public ControlParameters MetaData()
	{
		metadata ??= new ControlParameters();
		return metadata;
	}


	public bool HasMetaData()
	{
		return metadata != null;
	}

	public bool HasMetaDataKey(string key)
	{
		if (metadata != null)
		{
			return metadata.Find(key) != null;
		}
		return false;
	}

	public ControlParameters AddMetaDataIfNotNull(string key, string value)
	{
		if (value != null && string.IsNullOrEmpty(value) == false)
		{
			MetaData().Establish(key, value);
		}
		return metadata!;
	}

	public void MergeMetaData(ControlParameters? others)
	{
		if (others != null && others.Lookup != null)
		{
			var data = MetaData();
			foreach (var item in others.Lookup)
			{
				data.Establish(item.Key, item.Value);
			}
		}
	}

	public ControlParameters AddMetaData(string key, string value)
	{
		MetaData().Establish(key, value);
		return metadata!;
	}

	public object GetMetaData(string key)
	{
		return metadata?.Find(key) ?? null!;
	}

	public DT_Base initialize()
	{
		if (String.IsNullOrEmpty(Type))
		{
			Type = DT_Base.AsTopic(this.GetType().Name);
		}
		if (String.IsNullOrEmpty(TimeStamp))
		{
			ResetTimeStamp();
		}
		if (String.IsNullOrEmpty(Guid))
		{
			Guid = System.Guid.NewGuid().ToString();
		}
		return this;
	}

}

