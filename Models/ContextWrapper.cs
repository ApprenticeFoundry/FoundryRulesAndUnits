using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

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
	public class ContextWrapper<T> : IContextWrapper
	{

		public DateTime dateTime;

		[JsonInclude]  // Ensure calculated property is serialized to JSON
		public int length => payload?.Count ?? 0;  // Calculated property - always accurate

		public String payloadType;
		public ICollection<T> payload;
		public bool hasError;
		public string message;
		public bool isDeprecated;

		// Explicit interface property implementations - expose fields as properties for interface
		DateTime IContextWrapper.timestamp => dateTime;
		int IContextWrapper.length => length;
		bool IContextWrapper.hasError { get => hasError; set => hasError = value; }
		string IContextWrapper.message { get => message; set => message = value; }

		public ContextWrapper()
		{
			this.dateTime = DateTime.UtcNow;
			// length is now calculated automatically
			this.payloadType = typeof(T).Name;
			this.payload = new List<T>() { };
			this.hasError = false;
			this.message = string.Empty;
			this.isDeprecated = false;
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
		public ContextWrapper<T> SetMessage(string message)
		{
			this.hasError = false;
			this.message = $"{this.message} {message}".Trim();
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
		public ContextWrapper(T? obj, string note = "")
		{
			this.dateTime = DateTime.UtcNow;

			this.payloadType = obj == null ? "NONE" : obj.GetType().Name;
			this.payload = new List<T>() { };
			if (obj != null)
				this.payload.Add(obj);

			// length is now calculated automatically
			message = string.Empty;
			hasError = false;
			SetMessage(note);
		}


		/// <summary>
		/// Creates a wrapper with a collection of items
		/// </summary>
		public ContextWrapper(ICollection<T> list, string note = "") : this(list.FirstOrDefault(), note)
		{
			this.payload = list;
			// length is now calculated automatically
		}

		/// <summary>
		/// Creates a wrapper with an enumerable of items
		/// </summary>
		public ContextWrapper(IEnumerable<T> list, string note = "") : this(list.FirstOrDefault(), note)
		{
			this.payload = list.ToArray();
			// length is now calculated automatically
		}


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
				// length is now calculated automatically
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
		public static ContextWrapper<T> Ok(T item, string message = "")
		{
			return new ContextWrapper<T>(item)
			{
				hasError = false,
				message = message
			};
		}

		public static ContextWrapper<T> ObjectRequired(T? item, string message = "", string error = "Object not found")
		{
			if (item == null)
			{
				return ContextWrapper<T>.Error(error);
			}
			else
			{
				return ContextWrapper<T>.Ok(item, message);
			}
		}

		/// <summary>
		/// Creates a successful wrapper with multiple payload items
		/// Usage: return ContextWrapper<DocumentDTO>.Ok(documentList);
		/// </summary>
		public static ContextWrapper<T> Ok(IEnumerable<T> items, string message = "")
		{
			return new ContextWrapper<T>(items)
			{
				hasError = false,
				message = message
			};
		}

		public static ContextWrapper<T> ObjectRequired(ICollection<T> items, string message = "", string error = "Object not found")
		{
			if (items == null || items.Count == 0)
			{
				return ContextWrapper<T>.Error(error);
			}
			else
			{
				return ContextWrapper<T>.Ok(items, message);
			}
		}

		/// <summary>
		/// Creates an empty successful wrapper (no payload, no error)
		/// Usage: return ContextWrapper<DocumentDTO>.Empty();
		/// </summary>
		public static ContextWrapper<T> Empty(string message = "")
		{
			return new ContextWrapper<T>
			{
				hasError = false,
				message = message
			};
		}

		/// <summary>
		/// Creates a deprecated wrapper with a single item - used to mark code that needs migration
		/// The wrapper works normally but signals that this code path should be updated
		/// Usage: return ContextWrapper<DocumentDTO>.Deprecated(data, "Migrate to new Result<T> pattern");
		/// </summary>
		public static ContextWrapper<T> Deprecated(T item, string migrationMessage = "")
		{
			return new ContextWrapper<T>(item)
			{
				isDeprecated = true,
				message = string.IsNullOrWhiteSpace(migrationMessage)
					? "[DEPRECATED]"
					: $"[DEPRECATED] {migrationMessage}"
			};
		}

		/// <summary>
		/// Creates a deprecated wrapper with a collection - used to mark code that needs migration
		/// Usage: return ContextWrapper<DocumentDTO>.Deprecated(items, "Migrate to new Result<T> pattern");
		/// </summary>
		public static ContextWrapper<T> Deprecated(IEnumerable<T> items, string migrationMessage = "")
		{
			return new ContextWrapper<T>(items)
			{
				isDeprecated = true,
				message = string.IsNullOrWhiteSpace(migrationMessage)
					? "[DEPRECATED]"
					: $"[DEPRECATED] {migrationMessage}"
			};
		}

	}
}