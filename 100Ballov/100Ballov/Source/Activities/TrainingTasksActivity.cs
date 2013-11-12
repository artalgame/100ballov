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
	[Activity (Label = "TrainingTasksActivity")]			
	public class TrainingTasksActivity : Activity
	{
		public string subjectStringName;
		protected SubjectsEnumeration subjectType;
		LinearLayout taskButtonsLayout;
		SubjectTheme theme;
		string themeNum;
		List<Task> tasks;

		List<bool> isAnswered;
		List<bool> isRightAnswered;
		List<bool> isShowAnswer;

		int currentTaskToShow=0;

		SubjectRetriever subjectRetriever;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.TrainingTasksLayout);

			SetParameters ();
			ShowTask ();
		}

		protected void SetParameters()
		{
			Enum.TryParse<SubjectsEnumeration> (Intent.GetStringExtra ("TestType"), out subjectType);
			themeNum = Intent.GetStringExtra ("ThemeNum");
			currentTaskToShow = Intent.GetIntExtra ("TaskNum", 0);
			subjectStringName = SubjectHelper.GetSubjectName (subjectType, this);
			subjectRetriever = new SubjectRetriever (subjectType);
			theme = subjectRetriever.GetThemeByNum(themeNum);
			tasks = subjectRetriever.GetTasks(themeNum);

			isAnswered = new List<bool> (tasks.Count);
			isRightAnswered = new List<bool> (tasks.Count);
			isRightAnswered = new List<bool> (tasks.Count);
			for (int i=0; i< tasks.Count; i++) {
				isAnswered [i] = false;
				isRightAnswered [i] = false;
				isShowAnswer [i] = false;
			}
		}

		protected void ShowTask()
		{
			DrawTaskCondition ();
		}
		protected void DrawTaskCondition ()
		{

		}
	}
}

