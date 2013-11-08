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
using MR.Core.TestEntities;
using MR.Android.Data;

namespace com.flaxtreme.CT
{
	[Activity (Label = "Subject")]			
	public class SubjectActivity : Activity
	{
		protected SubjectsEnumeration subjectType;
		protected string subjectName;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.SubjectActivity);

			Enum.TryParse<SubjectsEnumeration> (Intent.GetStringExtra ("TestType"), out subjectType);

			subjectName = SubjectHelper.GetSubjectName(subjectType, this);
			SetSubjectNameTextView ();
			LoadStatistics ();

			(FindViewById<Button> (Resource.Id.TraningButton)).Click += TrainingButtonClick;


		}

		private void TrainingButtonClick(Object sender, EventArgs args)
		{
			var intent = new Intent (this, typeof(ChooseTrainingActivity));
			intent.PutExtra ("SubjectType", subjectType.ToString());
			StartActivity (intent);
		}

		private void TestButtonClick(Object sender, EventArgs args)
		{
		}

		protected void SetSubjectNameTextView()
		{
			(FindViewById<TextView> (Resource.Id.SubjectNameTextView)).SetText(subjectName, TextView.BufferType.Normal);
		}

		protected void LoadStatistics()
		{
			//TO DO: Implement statistics
		}
	}
}

