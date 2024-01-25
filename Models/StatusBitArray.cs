
using System.Text;
using System.Collections;

namespace FoundryRulesAndUnits.Models
{

	public class StatusBitArray
	{

		private BitArray m_Status;

		private enum StatusBit 
		{
			Invisible,
			Private,
			IsReadOnly,
			Unselectable,
			Selected,
			UserSpecified,
			Expanded,
			Calculating,
			Calculated,
			ProtectFormula,
			ProtectValue,
			ForceEvaluation,
			ValueIncorrect,
			MetaKnowledge,
			AllowSubshapes,
			AllowConnections,
			AllowAsParentShape,
			ShouldNotRender,
		}

		public StatusBitArray()
		{
			m_Status = new BitArray( 24 );
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
	}
}
