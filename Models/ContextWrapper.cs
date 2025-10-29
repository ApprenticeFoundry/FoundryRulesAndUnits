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

    public interface IContextWrapper
    {
        bool hasError { get; set; }
        string message { get; set; }
        int length { get; }
        DateTime timestamp { get; }

		bool IsEmpty();
		bool IsError();


    }

	[System.Serializable]
	public class ContextWrapper<T> :IContextWrapper
	{

		public DateTime dateTime;
		public int length;
		public String payloadType;
		public ICollection<T> payload;
		public bool hasError;
		public string message;

		// Explicit interface property implementations - expose fields as properties for interface
		DateTime IContextWrapper.timestamp => dateTime;
		int IContextWrapper.length => length;
		bool IContextWrapper.hasError { get => hasError; set => hasError = value; }
		string IContextWrapper.message { get => message; set => message = value; }

		public ContextWrapper()
		{
			this.dateTime = DateTime.UtcNow;
			this.length = 0;
			this.payloadType = typeof(T).Name;
			this.payload = new List<T>() { };
			this.hasError = false;
			this.message = string.Empty;
		}

		public bool IsEmpty()
		{
			return this.length == 0;
		}

		public bool IsError()
		{
			return this.hasError;
		}

		public ContextWrapper<T> SetError(string error)
		{
			this.hasError = true;
			this.message = $"{this.message} {error}".Trim();
			return this;
		}

		/// <summary>
		/// Converts this error wrapper to a different type while preserving the error state
		/// Usage: var docError = agentError.AsErrorFor<DocumentDTO>();
		/// </summary>
		public ContextWrapper<TResult> AsErrorFor<TResult>()
		{
			return new ContextWrapper<TResult>
			{
				hasError = this.hasError,
				message = this.message
			};
		}

		// ============================================================================
		// PUBLIC CONSTRUCTORS - Good, unambiguous constructors
		// ============================================================================

		/// <summary>
		/// Creates a wrapper with a single payload item
		/// </summary>
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

		/// <summary>
		/// Creates a wrapper with a collection of items
		/// </summary>
		public ContextWrapper(ICollection<T> list, string error = "") : this(list.FirstOrDefault(), error)
		{
			this.payload = list;
			this.length = payload.Count;
		}

		/// <summary>
		/// Creates a wrapper with an enumerable of items
		/// </summary>
		public ContextWrapper(IEnumerable<T> list, string error = "") : this(list.FirstOrDefault(), error)
		{
			this.payload = list.ToArray();
			this.length = payload.Count;
		}

		// ============================================================================
		// REMOVED: Dangerous ambiguous constructor
		// ============================================================================
		// public ContextWrapper(string error) - REMOVED!
		// This constructor was dangerous because when T=string, it created ambiguity:
		//   new ContextWrapper<string>("text") - Error or payload? Impossible to tell!
		// 
		// Use factory methods instead:
		//   ContextWrapper<string>.Error("error message")  // Explicit error
		//   ContextWrapper<string>.Ok("payload data")      // Explicit payload
		// ============================================================================

		public List<T> PayloadAsList()
		{
			return this.payload.ToList();
		}

	// ============================================================================
	// STATIC FACTORY METHODS - Explicit, unambiguous API
	// ============================================================================
	// Factory methods provide clear intent, especially for ContextWrapper<string>
	// where the dangerous single-string constructor has been removed.
	// ============================================================================

	/// <summary>
	/// Creates an error wrapper with a message (no payload)
	/// Usage: return ContextWrapper<DocumentDTO>.Error("Document not found");
	/// 
	/// This is the REQUIRED way to create error wrappers without payload.
	/// The old "new ContextWrapper(string)" constructor was removed due to
	/// ambiguity when T=string.
	/// </summary>
	public static ContextWrapper<T> Error(string message)
	{
		return new ContextWrapper<T>
		{
			dateTime = DateTime.UtcNow,
			length = 0,
			payloadType = typeof(T).Name,
			payload = new List<T>(),
			hasError = true,
			message = message
		};
	}

	/// <summary>
	/// Creates a successful wrapper with a single payload item
	/// Usage: return ContextWrapper<DocumentDTO>.Ok(document);
	/// 
	/// Equivalent to: new ContextWrapper<T>(item)
	/// </summary>
	public static ContextWrapper<T> Ok(T item)
	{
		return new ContextWrapper<T>(item);
	}

	/// <summary>
	/// Creates a successful wrapper with multiple payload items
	/// Usage: return ContextWrapper<DocumentDTO>.Ok(documentList);
	/// </summary>
	public static ContextWrapper<T> Ok(IEnumerable<T> items)
	{
		return new ContextWrapper<T>(items);
	}

	/// <summary>
	/// Creates an empty successful wrapper (no payload, no error)
	/// Usage: return ContextWrapper<DocumentDTO>.Empty();
	/// </summary>
	public static ContextWrapper<T> Empty()
	{
		return new ContextWrapper<T>();
	}

	// ============================================================================
	// LEGACY FACTORY METHODS - Preserved for backward compatibility
	// ============================================================================

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

}}