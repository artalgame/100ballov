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
		}

		protected void LoadTasksButtons()
		{
			var subjectRetriever = new SubjectRetriever (subjectType, this.ApplicationContext);
			var tasks = subjectRetriever.GetTasksInfo (theme.Num);
			int ai = 1;
			int bi = 1;
			int i = 0;

			foreach (var task in tasks) {
				Button taskButton = new Button (this);
				taskButton.Id = i;
//				var parameters = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.WrapContent, 1);
//				parameters.SetMargins (16, 8, 16, 8);
//				taskButton.LayoutParameters = parameters;
				int level = 0;
				taskButton.Click += TaskButtonClick;
				if (task == typeof(ATask)) {
					level = GetTaskStatistic(subjectRetriever, theme, subjectRetriever.GetATask(theme, ai));
					taskButton.Text = "A" + ai + "(" + level + "%)";
					ai++;
				} else {
					level = GetTaskStatistic(subjectRetriever, theme, subjectRetriever.GetBTask(theme, bi));
					taskButton.Text = "B" + bi+"(" + level + "%)";
					bi++;
				}
				if (level <= 30) {
					taskButton.SetTextColor(Android.Graphics.Color.Red);
				} else {
					if (level <= 80) {
						taskButton.SetTextColor (Android.Graphics.Color.Goldenrod);
					} else {
						taskButton.SetTextColor (Android.Graphics.Color.Green);
					}
				}
				taskButtonsLayout.AddView (taskButton);
				i++;
			}
			subjectRetriever.DB.Close ();
		}

		protected override void OnStart()
		{
			base.OnStart ();
			taskButtonsLayout.RemoveAllViews ();
			LoadTasksButtons ();
		}

		private int GetTaskStatistic(SubjectRetriever retriever, SubjectTheme theme, Task task)
		{
			var taskData = retriever.GetTaskDBData (task, theme);
			int answered = taskData.RightAttempts;
			int overall = taskData.OverallAttempts;
			if (overall == 0) {
				return 0;
			}
			return (int)(((float)answered) / overall * 100);
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

