
using System.Text;
using System.Collections;

namespace FoundryRulesAndUnits.Models
{

	public class StatusBitArray
	{

		private BitArray m_Status;

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
