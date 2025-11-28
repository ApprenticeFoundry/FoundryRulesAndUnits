
using System.Text;
using System.Collections;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Models
{

	public class StatusBitArray
	{

		private BitArray m_Status;
		
		/// <summary>
		/// Serialize stale bits as integer for efficient JavaScript routing.
		/// JavaScript receives this value and uses bitwise operations to determine operation type.
		/// </summary>
		[JsonInclude]
		[JsonPropertyName("staleBits")]
		public int StaleBits => GetStaleBits();
		
		/// <summary>
		/// Serialize ShouldDelete flag for JavaScript.
		/// </summary>
		[JsonInclude]
		[JsonPropertyName("shouldDelete")]
		public bool ShouldDeleteFlag => ShouldDelete;

		private enum StatusBit 
		{
			// === OBJECT LIFECYCLE (0-2) ===
			ShouldDelete = 0,
			NotNew = 1,              // Inverted: false = new object
			Dirty = 2,               // General-purpose dirty flag (backward compatible)
			
			// === GRANULAR STALE TRACKING - 3D GPU CACHE SYNC (3-7) ===
			NotTransformStale = 3,   // Inverted: false = GPU transform cache is stale
			NotMaterialStale = 4,    // Inverted: false = GPU material cache is stale
			NotGeometryStale = 5,    // Inverted: false = GPU geometry cache is stale
			NotStructureStale = 6,   // Inverted: false = GPU structure cache is stale
			NotDataStale = 7,        // Inverted: false = GPU data cache is stale
			// Reserved = 8,
			
			// === ACCESS CONTROL & PROTECTION (9-12) ===
			IsReadOnly = 9,
			Private = 10,
			ProtectFormula = 11,
			ProtectValue = 12,
			
			// === UI & SELECTION (13-16) ===
			Invisible = 13,
			Unselectable = 14,
			Selected = 15,
			Expanded = 16,
			
			// === RENDERING CONTROL (17-19) ===
			ShouldNotRender = 17,
			ShowChildren = 18,
			// Reserved = 19,
			
			// === EXPRESSION EVALUATOR (20-23) ===
			Calculating = 20,
			Calculated = 21,
			ForceEvaluation = 22,
			ValueIncorrect = 23,
			
			// === DATA PROVENANCE & VALIDATION (24-25) ===
			UserSpecified = 24,
			MetaKnowledge = 25,
			
			// === SHAPE/DIAGRAM DOMAIN (26-28) ===
			AllowSubshapes = 26,
			AllowConnections = 27,
			AllowAsParentShape = 28
			
			// === FUTURE EXPANSION (29-31) ===
			// Reserved = 29-31
		}

		public StatusBitArray()
		{
			m_Status = new BitArray( 32 );
			m_Status.SetAll(false);
		}

		public bool IsVisible
		{
			get
			{
				return !m_Status[(int)StatusBit.Invisible];
			}
			set
			{
				m_Status[(int)StatusBit.Invisible] = !value;
			}
		}


		public bool IsExpanded
		{
			get
			{
				return m_Status[(int)StatusBit.Expanded];
			}
			set
			{
				m_Status[(int)StatusBit.Expanded] = value;
			}
		}
		public bool IsMetaKnowledge
		{
			get
			{
				return m_Status[(int)StatusBit.MetaKnowledge];
			}
			set
			{
				m_Status[(int)StatusBit.MetaKnowledge] = value;
			}
		}
		public bool IsFormulaProtected
		{
			get
			{
				return m_Status[(int)StatusBit.ProtectFormula];
			}
			set
			{
				m_Status[(int)StatusBit.ProtectFormula] = value;
			}
		}

		public bool UserSpecified
		{
			get
			{
				return m_Status[(int)StatusBit.UserSpecified];
			}
			set
			{
				m_Status[(int)StatusBit.UserSpecified] = value;
			}
		}
		public bool IsReadOnly
		{
			get
			{
				return m_Status[(int)StatusBit.IsReadOnly];
			}
			set
			{
				m_Status[(int)StatusBit.IsReadOnly] = value;
			}
		}


		public bool IsValueProtected
		{
			get
			{
				return m_Status[(int)StatusBit.ProtectValue];
			}
			set
			{
				m_Status[(int)StatusBit.ProtectValue] = value;
			}
		}

		public bool IsCalculating
		{
			get
			{
				return m_Status[(int)StatusBit.Calculating];
			}
			set
			{
				m_Status[(int)StatusBit.Calculating] = value;
			}
		}
		public bool IsCalculated
		{
			get
			{
				return m_Status[(int)StatusBit.Calculated];
			}
			set
			{
				m_Status[(int)StatusBit.Calculated] = value;
			}
		}
		public bool AllowSubshapes
		{
			get
			{
				return m_Status[(int)StatusBit.AllowSubshapes];
			}
			set
			{
				m_Status[(int)StatusBit.AllowSubshapes] = value;
			}
		}
		public bool AllowConnections
		{
			get
			{
				return m_Status[(int)StatusBit.AllowConnections];
			}
			set
			{
				m_Status[(int)StatusBit.AllowConnections] = value;
			}
		}

		public bool IsValueIncorrect
		{
			get
			{
				return m_Status[(int)StatusBit.ValueIncorrect];
			}
			set
			{
				m_Status[(int)StatusBit.ValueIncorrect] = value;
			}
		}
		public bool IsEvaluationForce
		{
			get
			{
				return m_Status[(int)StatusBit.ForceEvaluation];
			}
			set
			{
				m_Status[(int)StatusBit.ForceEvaluation] = value;
			}
		}
		public bool AllowAsParentShape
		{
			get
			{
				return m_Status[(int)StatusBit.AllowAsParentShape];
			}
			set
			{
				m_Status[(int)StatusBit.AllowAsParentShape] = value;
			}
		}
		public bool IsPrivate
		{
			get
			{
				return m_Status[(int)StatusBit.Private];
			}
			set
			{
				m_Status[(int)StatusBit.Private] = value;
			}
		}

		public bool IsSelected
		{
			get
			{
				return m_Status[(int)StatusBit.Selected];
			}
			set
			{
				m_Status[(int)StatusBit.Selected] = value;
			}
		}

		public bool ShouldRender
		{
			get
			{
				return !m_Status[(int)StatusBit.ShouldNotRender];
			}
			set
			{
				m_Status[(int)StatusBit.ShouldNotRender] = !value;
			}
		}
		
		public bool ShowChildren
		{
			get
			{
				return !m_Status[(int)StatusBit.ShowChildren];
			}
			set
			{
				m_Status[(int)StatusBit.ShowChildren] = !value;
			}
		}
		public bool IsSelectable
		{
			get
			{
				return !m_Status[(int)StatusBit.Unselectable];
			}
			set
			{
				m_Status[(int)StatusBit.Unselectable] = !value;
			}
		}

		
		public bool IsDirty
		{
			get { return m_Status[(int)StatusBit.Dirty]; }
			set { m_Status[(int)StatusBit.Dirty] = value; }
		}

		// === GRANULAR STALE FLAGS (3D GPU Cache Sync) ===
		// These track when GPU/JavaScript cached rendering data is stale (C# has fresh data)
		// Independent from IsDirty (general-purpose flag) - used for optimized 3D rendering updates
		
		public bool IsTransformStale
		{
			get { return !m_Status[(int)StatusBit.NotTransformStale]; }
			set { m_Status[(int)StatusBit.NotTransformStale] = !value; }
		}

		public bool IsMaterialStale
		{
			get { return !m_Status[(int)StatusBit.NotMaterialStale]; }
			set { m_Status[(int)StatusBit.NotMaterialStale] = !value; }
		}

		public bool IsGeometryStale
		{
			get { return !m_Status[(int)StatusBit.NotGeometryStale]; }
			set { m_Status[(int)StatusBit.NotGeometryStale] = !value; }
		}

		public bool IsStructureStale
		{
			get { return !m_Status[(int)StatusBit.NotStructureStale]; }
			set { m_Status[(int)StatusBit.NotStructureStale] = !value; }
		}

		public bool IsDataStale
		{
			get { return !m_Status[(int)StatusBit.NotDataStale]; }
			set { m_Status[(int)StatusBit.NotDataStale] = !value; }
		}

		/// <summary>
		/// Returns true if ANY stale flag is set.
		/// </summary>
		public bool IsStale
		{
			get
			{
				return IsTransformStale || IsMaterialStale || IsGeometryStale || 
				       IsStructureStale || IsDataStale;
			}
		}

		/// <summary>
		/// Mark transform as stale (position, rotation, scale changed).
		/// </summary>
		public void SetTransformStale()
		{
			IsTransformStale = true;
		}

		/// <summary>
		/// Mark material as stale (color, texture, opacity changed).
		/// </summary>
		public void SetMaterialStale()
		{
			IsMaterialStale = true;
		}

		/// <summary>
		/// Mark geometry as stale (shape, vertices, mesh changed).
		/// </summary>
		public void SetGeometryStale()
		{
			IsGeometryStale = true;
		}

		/// <summary>
		/// Mark structure as stale (children added/removed).
		/// </summary>
		public void SetStructureStale()
		{
			IsStructureStale = true;
		}

		/// <summary>
		/// Mark data as stale (custom properties, labels changed).
		/// </summary>
		public void SetDataStale()
		{
			IsDataStale = true;
		}

	/// <summary>
	/// Clear all stale flags after GPU/JavaScript synchronization.
	/// </summary>
	public void ClearAllStaleFlags()
	{
		IsTransformStale = false;
		IsMaterialStale = false;
		IsGeometryStale = false;
		IsStructureStale = false;
		IsDataStale = false;
	}

	// === EFFICIENT BITWISE STALE FLAG OPERATIONS ===
	// Stale flags are bits 3-7 (inverted: false = stale, true = fresh)
	// NotTransformStale=3, NotMaterialStale=4, NotGeometryStale=5, NotStructureStale=6, NotDataStale=7
	
	private const int STALE_MASK = 0b11111000; // Bits 3-7
	private const int TRANSFORM_BIT = 1 << 3;
	private const int MATERIAL_BIT = 1 << 4;
	private const int GEOMETRY_BIT = 1 << 5;
	private const int STRUCTURE_BIT = 1 << 6;
	private const int DATA_BIT = 1 << 7;
	
	/// <summary>
	/// Get raw stale bits as integer. Returns 0 if no stale flags.
	/// Each bit position represents a stale flag (inverted - 0 means stale).
	/// Use GetStaleCount() to count how many flags are set.
	/// </summary>
	public int GetStaleBits()
	{
		int result = 0;
		for (int i = 3; i <= 7; i++)
		{
			if (m_Status[i])
				result |= (1 << i);
		}
		return result;
	}
	
	/// <summary>
	/// Count how many stale flags are set (0-5).
	/// Ultra-fast bitwise operation.
	/// </summary>
	public int GetStaleCount()
	{
		int staleBits = GetStaleBits();
		// Invert because stale flags are negative logic (0 = stale)
		int staleFlags = (~staleBits) & STALE_MASK;
		
		// Brian Kernighan's algorithm - counts set bits
		int count = 0;
		while (staleFlags != 0)
		{
			staleFlags &= (staleFlags - 1);
			count++;
		}
		return count;
	}
	
	/// <summary>
	/// Check if ONLY transform is stale (no other flags).
	/// Ultra-fast single bitwise comparison.
	/// </summary>
	public bool IsOnlyTransformStale()
	{
		int staleBits = GetStaleBits();
		int expected = STALE_MASK & ~TRANSFORM_BIT; // All fresh except transform
		return staleBits == expected;
	}
	
	/// <summary>
	/// Check if ONLY material is stale (no other flags).
	/// </summary>
	public bool IsOnlyMaterialStale()
	{
		int staleBits = GetStaleBits();
		int expected = STALE_MASK & ~MATERIAL_BIT;
		return staleBits == expected;
	}
	
	/// <summary>
	/// Check if ONLY geometry is stale (no other flags).
	/// </summary>
	public bool IsOnlyGeometryStale()
	{
		int staleBits = GetStaleBits();
		int expected = STALE_MASK & ~GEOMETRY_BIT;
		return staleBits == expected;
	}

	public bool IsNew
	{
		get { return !m_Status[(int)StatusBit.NotNew]; }
		set { m_Status[(int)StatusBit.NotNew] = !value; }
	}		
		public bool ShouldDelete
		{
			get
			{
				return m_Status[(int)StatusBit.ShouldDelete];
			}
			set
			{
				m_Status[(int)StatusBit.ShouldDelete] = value;
			}
		}
	}
}
