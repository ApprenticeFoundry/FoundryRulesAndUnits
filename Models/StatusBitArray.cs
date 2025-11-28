
using System.Text;
using System.Collections;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Models;


public class StatusBitArray
{

	private BitArray m_Status;

	/// <summary>
	/// Serialize all 32 status bits as integer for JavaScript access.
	/// JavaScript can use bitwise operations to check any flag.
	/// Includes lifecycle (0-2), stale flags (3-7), UI (13-16), and all other bits.
	/// </summary>
	[JsonInclude]
	[JsonPropertyName("statusBits")]
	public int StatusBits => GetAllBits();

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
		RecomputeBoundary = 8,   // Request JavaScript to compute world position

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
		m_Status = new BitArray(32);
		m_Status.SetAll(false);
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
	/// Request JavaScript to compute world position (bounding box center).
	/// Set when transform changes or _hitBoundary cache is cleared.
	/// Cleared after JavaScript returns boundary in ProcessHitBoundaries callback.
	/// </summary>
	public bool RecomputeBoundary
	{
		get { return m_Status[(int)StatusBit.RecomputeBoundary]; }
		set { m_Status[(int)StatusBit.RecomputeBoundary] = value; }
	}

	/// <summary>
	/// Check if JavaScript world position calculation is needed.
	/// Used by collector to add shape to BoundaryNeeded bucket.
	/// </summary>
	public bool IsRecomputeBoundaryNeeded => m_Status[(int)StatusBit.RecomputeBoundary];

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
	/// Request JavaScript to compute world position.
	/// Called when transform changes or when _hitBoundary cache is invalidated.
	/// </summary>
	public void SetRecomputeBoundary()
	{
		RecomputeBoundary = true;
	}

	/// <summary>
	/// Clear all stale flags after GPU/JavaScript synchronization.
	/// Also clears RecomputeBoundary flag after boundary is received.
	/// </summary>
	public void ClearAllStaleFlags()
	{
		IsTransformStale = false;
		IsMaterialStale = false;
		IsGeometryStale = false;
		IsStructureStale = false;
		IsDataStale = false;
		RecomputeBoundary = false;  // Clear after JavaScript returns boundary
	}

	/// <summary>
	/// Get all 32 status bits as a single integer for JavaScript serialization.
	/// Each bit position corresponds to a StatusBit enum value.
	/// </summary>
	public int GetAllBits()
	{
		int result = 0;
		for (int i = 0; i < 32; i++)
		{
			if (m_Status[i])
				result |= (1 << i);
		}
		return result;
	}


}

