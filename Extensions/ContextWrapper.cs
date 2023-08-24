using System;
using System.Collections.Generic;
using System.Linq;

namespace FoundryRulesAndUnits.Models
{

	public interface ISuccessOrFailure
	{
		bool Status { get; set; }
		string Message { get; set; }
	}

	public class Success : ISuccessOrFailure
	{
		public bool Status { get; set; }
		public string Message { get; set; }

		public Success()
		{
			Status = true;
			Message = string.Empty;
		}
	}

	public class Failure : ISuccessOrFailure
	{
		public bool Status { get; set; }
		public string Message { get; set; }

		public Failure()
		{
			Status = false;
			Message = string.Empty;
		}
	}


	[System.Serializable]
	public class ContextWrapper<T>
	{

		public DateTime dateTime;
		public int length;
		public String payloadType;
		public ICollection<T> payload;
		public bool hasError;
		public string message;

		public ContextWrapper()
		{
			this.dateTime = DateTime.UtcNow;
			this.length = 0;
			this.payloadType = typeof(T).Name;
			this.payload = new List<T>() { };
			this.hasError = false;
			this.message = string.Empty;
		}

		public ContextWrapper(T? obj, string error = "")
		{
			this.dateTime = DateTime.UtcNow;

			this.payloadType = obj == null ? "NONE" : obj.GetType().Name;
			this.payload = new List<T>() { };
			if ( obj != null )
				this.payload.Add(obj);

			this.length = this.payload.Count;

			this.hasError = error != string.Empty;
			this.message = error != string.Empty ? error : string.Empty;
		}

		public ContextWrapper(ICollection<T> list, string error = "") : this(list.FirstOrDefault(), error)
		{
			this.payload = list;
			this.length = payload.Count;
		}

		public ContextWrapper(IEnumerable<T> list, string error = "") : this(list.FirstOrDefault(), error)
		{
			this.payload = list.ToArray();
			this.length = payload.Count;
		}

		public ContextWrapper(string error)
		{
			this.length = 0;
			this.dateTime = DateTime.UtcNow;
			this.payloadType = typeof(T).Name;
			this.payload = new List<T>() { };
			this.hasError = true;
			this.message = error;
		}
		public List<T> PayloadAsList()
		{
			return this.payload.ToList();
		}

		public static ContextWrapper<Success> success(string message = "")
		{
			var wrap = new ContextWrapper<Success>(new Success()
			{
				Message = message,
				Status = true
			})
			{
				hasError = false
			};
			return wrap;
		}
		public static ContextWrapper<Failure> exception(string message = "")
		{
			var wrap = new ContextWrapper<Failure>(new Failure()
			{
				Message = message,
				Status = false
			})
			{
				hasError = true
			};
			return wrap;
		}

	}

}