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
using Android.Content.PM;

namespace com.flaxtreme.CT
{
	[Activity (Label = "TrainingTasksActivity", ConfigurationChanges = (ConfigChanges.Orientation | ConfigChanges.KeyboardHidden
		| (ConfigChanges)0x0400))]			

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
			//ShowTask ();
		}

		protected void SetParameters()
		{
			Enum.TryParse<SubjectsEnumeration> (Intent.GetStringExtra ("TestType"), out subjectType);
			themeNum = Intent.GetStringExtra ("ThemeNum");
			currentTaskToShow = Intent.GetIntExtra ("TaskNum", 0);
			subjectStringName = SubjectHelper.GetSubjectName (subjectType, this);
			subjectRetriever = new SubjectRetriever (subjectType, ApplicationContext);
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

			FindViewById<EditText> (Resource.Id.AnswerTextBox).TextChanged += AnswerTextBoxChanged;

			//ShowTask ();
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
					LinearLayout variantLayout = new LinearLayout (this){ Orientation = Orientation.Horizontal, Id = i, Clickable = true};
					var parameters = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.WrapContent, 1);
					parameters.SetMargins (4, 4, 4, 4);
					variantLayout.LayoutParameters = parameters;
					variantLayout.Click += VariantClickEvent;
					TextView text = new TextView (this){Text = (i+1)+")" };


						text.Text += tsk.Variants [i].Text;

					if (!isAnswered [currentTaskToShow]) {
						if (tsk.CheckedAnswers [i]) {
							variantLayout.SetBackgroundColor (Android.Graphics.Color.DarkBlue);
							text.SetTextColor (Android.Graphics.Color.WhiteSmoke);
						}
					} else {
						if (!isShowAnswer [currentTaskToShow]) {
							if (tsk.CheckedAnswers [i]) {
								if (tsk.Variants [i].IsRight) {
									variantLayout.SetBackgroundColor (Android.Graphics.Color.Green);
								} else {
									variantLayout.SetBackgroundColor (Android.Graphics.Color.Red);
								}
							}
						} else {
							if (tsk.Variants [i].IsRight) {
								variantLayout.SetBackgroundColor (Android.Graphics.Color.Green);
							} else {
								variantLayout.SetBackgroundColor (Android.Graphics.Color.Red);
							}
						}
					}

					variantLayout.AddView (text);
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

				var answerTextBox = FindViewById<EditText> (Resource.Id.AnswerTextBox);
				answerTextBox.Enabled = true;
				answerTextBox.Visibility = ViewStates.Visible;


				answerTextBox.Text = ((BTask)tasks [currentTaskToShow]).UserAnswer;
//				if (String.IsNullOrEmpty (answerTextBox.Text)) {
//					answerTextBox.Text = "Введите сюда ваш ответ";
//				}
				if (isAnswered [currentTaskToShow]) {
					var tsk = tasks [currentTaskToShow] as BTask;
					string answer = tsk.UserAnswer;
					string rightAnswer = tsk.Variant;
					if (!isShowAnswer [currentTaskToShow]) {
						if (answer == rightAnswer) {
							answerTextBox.SetBackgroundColor (Android.Graphics.Color.Green);
						} else {
							answerTextBox.SetBackgroundColor (Android.Graphics.Color.Red);
						}
					} else {
						answerTextBox.SetBackgroundColor (Android.Graphics.Color.Green);
						answerTextBox.Text = rightAnswer;
						answerTextBox.Enabled = false;
					} 
				}
				else {
					answerTextBox.SetBackgroundColor (Android.Graphics.Color.LightGray);
				}
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

		protected void AnswerTextBoxChanged(object sender, EventArgs args)
		{
			EditText answerTextBox = (EditText)sender;
			if (tasks [currentTaskToShow] is BTask) {
				BTask task = (BTask)tasks [currentTaskToShow];
				task.UserAnswer = answerTextBox.Text;
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
			if (!isAnswered [currentTaskToShow]) {
				TaskDBData taskDBData = subjectRetriever.GetTaskDBData (tasks [currentTaskToShow], theme);
				taskDBData.OverallAttempts++;
				if (tasks [currentTaskToShow] is ATask) {
					var task = tasks [currentTaskToShow] as ATask;
					int i = 0;
					int right = 0;
					bool isChecked = false;
					foreach (var isCheckedAns in task.CheckedAnswers) {
						if (isCheckedAns) {
							isChecked = true;
							if (task.Variants [i].IsRight) {
								right++;
							}
						}
						i++;
					}
					if (right == task.RightVariants.Count ()) {
						taskDBData.RightAttempts++;
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
							taskDBData.RightAttempts++;
						} 
						SetIsAnswered (sender);
					}
				}
				subjectRetriever.UpdateTaskDBData (taskDBData);
				ShowTask ();
			} else {
				if (!isShowAnswer [currentTaskToShow]) {
					isShowAnswer [currentTaskToShow] = true;
					GoToURL ("http://goo.gl/MHtO2W");
				}
			}
		}

		protected override void OnStart()
		{
			base.OnStart ();
			ShowTask ();
		}

		private void GoToURL(string url)
		{
			Android.Net.Uri uri = Android.Net.Uri.Parse (url);
			if (uri != null) {
				Intent launchBrowser = new Intent (Intent.ActionView, uri);
				StartActivity (launchBrowser);
			} 
		}

		protected void SetIsAnswered(object sender)
		{
			var button = (Button)sender;
			isAnswered [currentTaskToShow] = true;
		}

		protected override void OnDestroy()
		{
			try
			{
				base.OnDestroy();
				subjectRetriever.DB.Close ();
			}
			catch(Exception ex) {

			}
		}
	}
}

