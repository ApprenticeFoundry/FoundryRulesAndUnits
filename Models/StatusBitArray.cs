
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
			Enabled,
			LocalMembersOnly,
			Dynamic,
			BadReference,
			MetaKnowledge,
			QueueTrigger,
			Temporary,
			Purging,
			ValueOverTyped,
			ExistenceSuspect,
			ValueSuspect,
			UserSpecified,
			Private,
			Internal,
			Expanded,
			Calculating,
			Calculated,
			ProtectFormula,
			ProtectValue,
			ForceEvaluation,
			ValueIncorrect,
			IsReadOnly,
			TrackChanges
		}

		public StatusBitArray()
		{
			m_Status = new BitArray( 24 );
			m_Status.SetAll(false);
			Enabled = true;
		}

		public bool Visible
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

		public bool Enabled
		{
			get
			{
				return m_Status[(int)StatusBit.Enabled];
			}
			set
			{
				m_Status[(int)StatusBit.Enabled] = value;
			}
		}
		public bool IsPurging
		{
			get
			{
				return m_Status[(int)StatusBit.Purging];
			}
			set
			{
				m_Status[(int)StatusBit.Purging] = value;
			}
		}
		public bool HasQueueTrigger
		{
			get
			{
				return m_Status[(int)StatusBit.QueueTrigger];
			}
			set
			{
				m_Status[(int)StatusBit.QueueTrigger] = value;
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
		public bool IsWhileFormula
		{
			get
			{
				return m_Status[(int)StatusBit.Temporary];
			}
			set
			{
				m_Status[(int)StatusBit.Temporary] = value;
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
		public bool IsValueOverTyped
		{
			get
			{
				return m_Status[(int)StatusBit.ValueOverTyped];
			}
			set
			{
				m_Status[(int)StatusBit.ValueOverTyped] = value;
			}
		}
		public bool TrackChanges
		{
			get
			{
				return m_Status[(int)StatusBit.TrackChanges];
			}
			set
			{
				m_Status[(int)StatusBit.TrackChanges] = value;
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
		public bool IsValueDetermined
		{
			get
			{
				return !m_Status[(int)StatusBit.ValueSuspect];
			}
			set
			{
				m_Status[(int)StatusBit.ValueSuspect] = !value;
				m_Status[(int)StatusBit.Calculating]= false;
			}
		}
		public bool Recalculate
		{
			get
			{
				return IsValueSuspect || IsReferenceBad; // || IsRecalculatedAlways;
			}
		}
		public bool AllowRecalculation
		{
			get
			{
				return !IsFormulaProtected && (IsCalculated || IsReferenceBad);
			}
		}
		public bool IsValueSuspect
		{
			get
			{
				return m_Status[(int)StatusBit.ValueSuspect];
			}
			set
			{
				m_Status[(int)StatusBit.ValueSuspect] = value;
				m_Status[(int)StatusBit.Calculating] = false;

				if ( value == true && IsReferenceBad == true )
					IsReferenceBad = false;

				if ( value && IsValueIncorrect )
					IsValueIncorrect = false;
			}
		}

		public bool IsExistenceSuspect
		{
			get
			{
				return m_Status[(int)StatusBit.ExistenceSuspect];
			}
			set
			{
				m_Status[(int)StatusBit.ExistenceSuspect] = value;
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
		public bool IsLocalMembers
		{
			get
			{
				return m_Status[(int)StatusBit.LocalMembersOnly];
			}
			set
			{
				m_Status[(int)StatusBit.LocalMembersOnly] = value;
			}
		}
		public bool Dynamic
		{
			get
			{
				return m_Status[(int)StatusBit.Dynamic];
			}
			set
			{
				m_Status[(int)StatusBit.Dynamic] = value;
			}
		}
		public bool IsReferenceBad
		{
			get
			{
				return m_Status[(int)StatusBit.BadReference];
			}
			set
			{
				m_Status[(int)StatusBit.BadReference] = value;
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
		public bool IsInternal
		{
			get
			{
				return m_Status[(int)StatusBit.Internal];
			}
			set
			{
				m_Status[(int)StatusBit.Internal] = value;
			}
		}
		public bool IsTemporary
		{
			get
			{
				return m_Status[(int)StatusBit.Temporary];
			}
			set
			{
				m_Status[(int)StatusBit.Temporary] = value;
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
		public bool IsPublic
		{
			get
			{
				return !m_Status[(int)StatusBit.Private];
			}
			set
			{
				m_Status[(int)StatusBit.Private] = !value;
			}
		}
	}
}
