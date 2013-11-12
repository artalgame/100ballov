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
using MR.Android;

namespace com.flaxtreme.CT
{
	[Activity (Label = "ChooseTrainingTasksActivity")]			
	public class ChooseTrainingTasksActivity : Activity
	{
		public string subjectStringName;
		protected SubjectsEnumeration subjectType;
		LinearLayout taskButtonsLayout;
		SubjectTheme theme;
		string themeNum;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
		
			SetContentView (Resource.Layout.ChooseTrainingTasksLayout);

			Enum.TryParse<SubjectsEnumeration> (Intent.GetStringExtra ("TestType"), out subjectType);
			themeNum = Intent.GetIntExtra ("ThemeNum", 1).ToString ();
			subjectStringName = SubjectHelper.GetSubjectName (subjectType, this);
			theme = (new ThemesRetriever (subjectType)).GetThemes.FirstOrDefault (x => x.Num == themeNum);

			taskButtonsLayout = FindViewById<LinearLayout> (Resource.Id.TasksLinearLayout);
			(FindViewById<TextView> (Resource.Id.ThemeName)).Text = theme.Name;
			LoadThemesButtons ();
		}

		protected void LoadThemesButtons()
		{
			var tasks = new SubjectRetriever (subjectType).GetTasksInfo (theme.Num);
			int ai = 1;
			int bi = 1;
			int i = 0;

			foreach (var task in tasks) {
				Button taskButton = new Button (this);
				taskButton.Id = i;
				taskButton.Click += TaskButtonClick;
				if (task == typeof(ATask)) {
					taskButton.Text = "A" + ai;
					ai++;
				} else {
					taskButton.Text = "B" + bi;
					bi++;
				}
				taskButtonsLayout.AddView (taskButton);
				i++;
			}
		}

		protected void TaskButtonClick(object sender, EventArgs e)
		{
			var intent = new Intent (this, typeof(TrainingTasksActivity));
			intent.PutExtra ("SubjectType", subjectType.ToString());
			intent.PutExtra ("ThemeNum", themeNum);
			intent.PutExtra ("TaskNum", ((Button)sender).Id);
			StartActivity (intent);
		}
	}
}

