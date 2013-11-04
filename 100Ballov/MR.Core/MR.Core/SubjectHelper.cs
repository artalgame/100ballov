using System;
using MR.Core.TestEntities;
using MR.Core.Interfaces;

namespace MR.Core
{
	public class SubjectHelper
	{
		private ISubjectRetriver subjectRetriever;

		public SubjectsEnumeration Subject {
			get;
			private set;
		}

		public SubjectHelper (SubjectsEnumeration subject, ISubjectRetriver subjectRetriever)
		{
			Subject = subject;
			this.subjectRetriever = subjectRetriever;
		}
	}
}

