using System;

namespace FoundryRulesAndUnits.Models
{
	public class DT_Base
	{
		public string? guid;
		public string? parentGuid;
		public string? name;
		public string? type;
		public string? url;

		public string? timeStamp;

		public ControlParameters? metadata;


		public DT_Base()
		{
			this.initialize();
		}
		public DT_Base(string name)
		{
			this.name = name;
			this.initialize();
		}
		public virtual T merge<T>(T obj) where T : DT_Base
		{
			if (this.timeStamp?.CompareTo(obj.timeStamp) < 0)
			{
				this.timeStamp = obj.timeStamp;
			}
			return (this as T)!;
		}

		public string resetTimeStamp()
		{
			this.timeStamp = DateTime.UtcNow.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
			return this.timeStamp;
		}

		public static string asTopic(string name)
		{
			return name.Replace("DT_", "");
		}

		public static string asTopic<T>() where T : DT_Base
		{
			return DT_Base.asTopic(typeof(T).Name);
		}

		public static string asTopicLower<T>() where T : DT_Base
		{
			return DT_Base.asTopic(typeof(T).Name).ToLower();
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
			if ( value != null && string.IsNullOrEmpty(value) == false)
			{
				MetaData().Establish(key, value);
			}
			return metadata!;
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
			if (String.IsNullOrEmpty(type))
			{
				type = DT_Base.asTopic(this.GetType().Name);
			}
			if (String.IsNullOrEmpty(timeStamp))
			{
				resetTimeStamp();
			}
			if (String.IsNullOrEmpty(guid))
			{
				guid = Guid.NewGuid().ToString();
			}
			return this;
		}

	}
}
