using System;
using MR.Core.TestEntities;
using Android.Content;

namespace MR.Android.Data
{
	public static class SubjectHelper
	{
		public static string GetSubjectName (SubjectsEnumeration subjectType, Context context)
		{
			string subjectName = null;
			switch (subjectType) {
			case SubjectsEnumeration.Math:
				subjectName = context.GetString (com.flaxtreme.CT.Resource.String.Math);
				break;
			}
			return subjectName;
		}
	}
}


