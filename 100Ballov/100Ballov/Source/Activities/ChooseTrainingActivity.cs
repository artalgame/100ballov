using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MR.Android.Data;
using MR.Core.TestEntities;

namespace com.flaxtreme.CT
{
	[Activity (Label = "ChooseTrainingActivity")]			
	public class ChooseTrainingActivity : Activity
	{
		public string subjectName;
		protected SubjectsEnumeration subjectType;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.ChooseTraningLayout);

			Enum.TryParse<SubjectsEnumeration> (Intent.GetStringExtra ("TestType"), out subjectType);
			subjectName = SubjectHelper.GetSubjectName (subjectType, this);

		}
	}
}

