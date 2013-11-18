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
using Android.Graphics.Drawables;

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

		Drawable[] taskImages;

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

			taskImages = new Drawable[tasks.Count];

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

			FindViewById<Button> (Resource.Id.PastTask).Click += PastButtonClick;

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
			DrawVariants (task);
			DrawSolution ();
			DrawPastTaskButton (task);
		}

		protected void DrawPastTaskButton(Task task)
		{
			if (isAnswered [currentTaskToShow]) {
				FindViewById<Button> (Resource.Id.PastTask).Text = "Показать правильный ответ?";
			} else {
				FindViewById<Button> (Resource.Id.PastTask).Text = "Сдать задачу";
			}
		}

		protected void DrawTaskCondition (Task task)
		{
			if (!String.IsNullOrEmpty (tasks [currentTaskToShow].QuestionText)) {
				FindViewById<TextView> (Resource.Id.TaskText).Visibility = ViewStates.Visible;
				FindViewById<TextView> (Resource.Id.TaskText).Text = tasks [currentTaskToShow].QuestionText;
			} else {
				FindViewById<TextView> (Resource.Id.TaskText).Visibility = ViewStates.Gone;
			}


			string taskType = "a";
			int taskNum = currentTaskToShow + 1;
			if (task is BTask) {
				taskType = "b";
				taskNum = currentTaskToShow - tasks.Count ((Task x) => x is ATask) + 1;
			} 
			var taskImage = taskImages [currentTaskToShow] ?? subjectRetriever.GetImage (themeNum, taskNum, tasks [currentTaskToShow].QuestionImageLink,taskType);
			if (taskImage != null) {
				taskImages [currentTaskToShow] = taskImage;
				FindViewById<ImageView> (Resource.Id.TaskImage).Visibility = ViewStates.Visible;
				FindViewById<ImageView> (Resource.Id.TaskImage).SetImageDrawable (taskImage);
			} else {
				FindViewById<ImageView> (Resource.Id.TaskImage).Visibility = ViewStates.Gone;
			}
		}

		protected void DrawVariants(Task task)
		{
			if (task is ATask) {

				FindViewById<EditText> (Resource.Id.AnswerTextBox).Visibility=ViewStates.Gone;

				var tsk = (ATask)task;
				var variantGroup = FindViewById<LinearLayout> (Resource.Id.VariantsGroup);
				variantGroup.RemoveAllViews ();
				variantGroup.Enabled = true;
				variantGroup.Visibility = ViewStates.Visible;
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
				FindViewById<LinearLayout> (Resource.Id.VariantsGroup).Enabled = false;
				FindViewById<LinearLayout> (Resource.Id.VariantsGroup).Visibility = ViewStates.Gone;

				FindViewById<EditText> (Resource.Id.AnswerTextBox).Enabled = true;
				FindViewById<EditText> (Resource.Id.AnswerTextBox).Visibility = ViewStates.Visible;
			}
		}

		protected void DrawSolution()
		{
			FindViewById<TextView> (Resource.Id.SolutionText).Visibility = ViewStates.Gone;
			FindViewById<TextView> (Resource.Id.SolutionText).Enabled = false;

			FindViewById<ImageView> (Resource.Id.SolutionImage).Visibility = ViewStates.Gone;
			FindViewById<ImageView> (Resource.Id.SolutionImage).Enabled = false;

			FindViewById<Button> (Resource.Id.SolutionInetLink).Visibility = ViewStates.Gone;
			FindViewById<Button> (Resource.Id.SolutionInetLink).Enabled = false;
		}

		protected void VariantClickEvent(object sender, EventArgs args){
			if (!isAnswered [currentTaskToShow]) {
				var variantLayout = (LinearLayout)sender;
				var task = (ATask)tasks [currentTaskToShow];
				var tmpChecked = task.CheckedAnswers [variantLayout.Id];
				task.ResetCheckedAnswers ();

				task.CheckedAnswers [variantLayout.Id] = tmpChecked ^ true;
				ShowTask ();
			}
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

		protected void PastButtonClick(object sender, EventArgs args){

			if (tasks [currentTaskToShow] is ATask) {
				var task = tasks [currentTaskToShow] as ATask;
				int i = 0;
				bool isChecked = false;
				foreach (var isCheckedAns in task.CheckedAnswers) {
					if (isCheckedAns) {
						isChecked = true;
						var variant = (LinearLayout)FindViewById<LinearLayout> (Resource.Id.VariantsGroup).GetChildAt (i);
						if (task.Variants [i].IsRight) {
							variant.SetBackgroundColor(Android.Graphics.Color.Green);
						} else {
							variant.SetBackgroundColor(Android.Graphics.Color.Red);
						}
					}
							i++;
				}
				if (isChecked) {
					SetIsAnswered (sender);
				}
			} else {
				var task = tasks [currentTaskToShow] as BTask;
				var answerEditText = FindViewById<EditText> (Resource.Id.AnswerTextBox);
				string answer = answerEditText.Text;
				string rightAnswer = task.Variant;
				if (!String.IsNullOrEmpty (rightAnswer)) {
					if (answer == rightAnswer) {
						answerEditText.SetBackgroundColor (Android.Graphics.Color.Green);
					} else {
						answerEditText.SetBackgroundColor (Android.Graphics.Color.Red);
					
					}
					SetIsAnswered (sender);
				}
			}
		}

		protected void SetIsAnswered(object sender)
		{
			var button = (Button)sender;
			isAnswered [currentTaskToShow] = true;
			button.Text = "Показать правильный ответ?";
		}
	}
}

