
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
			
			// === GRANULAR DIRTY TRACKING - 3D OPTIMIZATION (3-7) ===
			NotTransformDirty = 3,   // Inverted: false = transform dirty
			NotMaterialDirty = 4,    // Inverted: false = material dirty
			NotGeometryDirty = 5,    // Inverted: false = geometry dirty
			NotStructureDirty = 6,   // Inverted: false = structure dirty
			NotDataDirty = 7,        // Inverted: false = data dirty
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

		// === GRANULAR DIRTY FLAGS (3D Optimization) ===
		// These are independent from IsDirty and used for smart update routing
		
		public bool IsTransformDirty
		{
			get { return !m_Status[(int)StatusBit.NotTransformDirty]; }
			set { m_Status[(int)StatusBit.NotTransformDirty] = !value; }
		}

		public bool IsMaterialDirty
		{
			get { return !m_Status[(int)StatusBit.NotMaterialDirty]; }
			set { m_Status[(int)StatusBit.NotMaterialDirty] = !value; }
		}

		public bool IsGeometryDirty
		{
			get { return !m_Status[(int)StatusBit.NotGeometryDirty]; }
			set { m_Status[(int)StatusBit.NotGeometryDirty] = !value; }
		}

		public bool IsStructureDirty
		{
			get { return !m_Status[(int)StatusBit.NotStructureDirty]; }
			set { m_Status[(int)StatusBit.NotStructureDirty] = !value; }
		}

		public bool IsDataDirty
		{
			get { return !m_Status[(int)StatusBit.NotDataDirty]; }
			set { m_Status[(int)StatusBit.NotDataDirty] = !value; }
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
