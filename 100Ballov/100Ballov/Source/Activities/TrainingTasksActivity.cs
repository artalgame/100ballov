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
			PresetLayout ();
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
			isShowAnswer = new List<bool> (tasks.Count);
			for (int i=0; i< tasks.Count; i++) {
				isAnswered.Add(false);
				isRightAnswered.Add(false);
				isShowAnswer.Add(false);
			}
		}

		protected void PresetLayout()
		{
			FindViewById<TextView> (Resource.Id.ThemeName).Text = theme.Name;

			FindViewById<Button> (Resource.Id.PrevButton).Click += PrevClick;
			FindViewById<Button> (Resource.Id.NextButton).Click += NextClick;

			ShowTask ();
		}

		protected void ShowTask()
		{

			Task task = tasks[currentTaskToShow];

			if (task is ATask) {
				FindViewById<TextView> (Resource.Id.TaskName).Text = "A" + GetRightTaskNumber ();
			} else {
				FindViewById<TextView> (Resource.Id.TaskName).Text = "B" + GetRightTaskNumber ();
			}

			DrawTaskCondition (task);
		}
		protected void DrawTaskCondition (Task task)
		{
			SetTask (task);
		}

		protected void SetTask(Task task)
		{
			if (!String.IsNullOrEmpty (tasks [currentTaskToShow].QuestionText)) {
				FindViewById<TextView> (Resource.Id.TaskText).Visibility = ViewStates.Visible;
				FindViewById<TextView> (Resource.Id.TaskText).Text = tasks [currentTaskToShow].QuestionText;
			} else {
				FindViewById<TextView> (Resource.Id.TaskText).Visibility = ViewStates.Gone;
			}


			var taskImage = subjectRetriever.GetImage (themeNum, currentTaskToShow+1, tasks[currentTaskToShow].QuestionImageLink,"a");
			if (taskImage != null) {
				FindViewById<ImageView> (Resource.Id.TaskImage).Visibility = ViewStates.Visible;
				FindViewById<ImageView> (Resource.Id.TaskImage).SetImageDrawable (taskImage);
			} else {
				FindViewById<ImageView> (Resource.Id.TaskImage).Visibility = ViewStates.Gone;
			}

			if (task is ATask) {
				var tsk = (ATask)task;
				var variantGroup = FindViewById<LinearLayout> (Resource.Id.VariantsGroup);
				variantGroup.RemoveAllViews ();
				for (int i = 0; i < tsk.Variants.Count; i++) {
					LinearLayout variantLayout = new LinearLayout (this){ Orientation = Orientation.Horizontal, Id = i, Clickable = true };
					variantLayout.Click += VariantClickEvent;


					if (!String.IsNullOrEmpty (tsk.Variants [i].Text)) {
						TextView text = new TextView (this){ Text = tsk.Variants [i].Text };

						if (tsk.CheckedAnswers [i]) {
							variantLayout.SetBackgroundColor (Android.Graphics.Color.DarkBlue);
							text.SetTextColor(Android.Graphics.Color.WhiteSmoke);
						}

						variantLayout.AddView (text);
					}

					var variantImage = subjectRetriever.GetImage (themeNum, currentTaskToShow + 1, tsk.Variants[i].ImageLink, "a");
					if (variantImage != null) {
						ImageView variantImgView = new ImageView (this);
						variantImgView.SetImageDrawable (variantImage);
						variantLayout.AddView (variantImgView);
					}
					variantGroup.AddView (variantLayout);
				}
			} else {

			}
		}

		protected void VariantClickEvent(object sender, EventArgs args){
			var variantLayout = (LinearLayout)sender;
			var task = (ATask)tasks [currentTaskToShow];
			task.CheckedAnswers [variantLayout.Id] = task.CheckedAnswers [variantLayout.Id] ^ true;
			ShowTask ();
		}

		protected void NextClick(object sender, EventArgs args){
			if (currentTaskToShow + 1 == tasks.Count) {
				currentTaskToShow = 0;
			} else {
				currentTaskToShow++;
			}
			ShowTask ();
		}

		protected void PrevClick(object sender, EventArgs args){
			if (currentTaskToShow - 1 == -1) {
				currentTaskToShow = tasks.Count - 1;
			} else {
				currentTaskToShow--;
			}
			ShowTask ();
		}

		protected int GetRightTaskNumber (){
			int aCount = subjectRetriever.GetATasksCount (themeNum);
			int bCount = subjectRetriever.GetBTasksCount (themeNum);
			if (currentTaskToShow < aCount) {
				return currentTaskToShow+1;
			} else {
				return currentTaskToShow - aCount + 1;
			}
		}
	}
}

