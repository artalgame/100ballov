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
			var retriever = new SubjectRetriever (subjectType, this);
			var statistic = retriever.GetStatistic ();
			int overall = statistic.X;
			int right = statistic.Y;

			FindViewById<TextView> (Resource.Id.OverallTextView).Text = "Всего попыток: " + overall;
			FindViewById<TextView> (Resource.Id.RightTextView).Text = "Успешных попыток: " + right;
			int percent = overall == 0 ? 0 : (int)(((float)right / (float)overall) * 100);
			FindViewById<TextView> (Resource.Id.ProcentTextView).Text = "Процент правильных ответов: " + percent;
		}
	}
}

