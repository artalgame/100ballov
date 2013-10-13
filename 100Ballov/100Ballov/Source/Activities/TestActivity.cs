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
using Ballov.TestClasses;
using Android.Graphics.Drawables;
using System.IO;
using System.Xml.Serialization;

namespace Ballov.Activites
{
	[Activity (Label = "TestActivity")]			
	public class TestActivity : Activity
	{
		public const int NoSelectedAnswers = -1;

		Test test = null;
		TextView timeLabel = null;
		TextView subjectNameLabel = null;
		TextView taskNameLabel = null;

		TextView taskLabel = null;
		ImageView taskImage = null;
		LinearLayout variantsLayout = null;

		Button passButton = null;
		Button prevButton = null;
		Button nextButton = null;

		bool isATask = true;
		int currentTaskNum = 0;

		int[] aAnswers;
		string[] bAnswers;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.TestTask);

			TestTypeEnum testType = TestTypeEnum.No;
			Enum.TryParse<TestTypeEnum> (Intent.GetStringExtra ("TestType"), out testType);

			if (testType != TestTypeEnum.No) {
				LoadViews();
				SetSubjectName (testType);
				test = GetTest (testType);

				aAnswers = new int[test.GetATaskCount];
				for (int i = 0; i< aAnswers.Length; i++) {
					aAnswers [i] = NoSelectedAnswers;
				}
				bAnswers = new string[test.GetBTaskCount];

				StartTest ();

			}

		}

		private void StartTest()
		{
			variantsLayout.RemoveAllViews ();
			taskImage.Enabled = false;

			if (isATask) {
				SetCurrentATask ();

			} else {
				SetCurrentBTask ();
		}
	}

		private void SetCurrentATask()
		{
			var curTask = test.GetATask (currentTaskNum);
			if (curTask != null) {
				taskNameLabel.Text = "A" + (currentTaskNum + 1);
				taskLabel.Text = curTask.Desc;
				if (!String.IsNullOrEmpty (curTask.ImageLink)) {
					taskImage.Enabled = true;
					using (Stream ims = Assets.Open (curTask.ImageLink)) {
						Drawable d = Drawable.CreateFromStream (ims, null);
						taskImage.SetImageDrawable (d);
					}
				}
				RadioGroup variantGroup = new RadioGroup (this);
				variantGroup.CheckedChange+= (object sender, RadioGroup.CheckedChangeEventArgs e) => {
					for(int i=0; i<variantGroup.ChildCount; i++)
					{
						RadioButton child = (RadioButton) variantGroup.GetChildAt(i);
						if(child.Checked)
						{
							aAnswers[currentTaskNum] = i;
							return;
						}
					}
				};
				variantGroup.Orientation = Orientation.Vertical;
				int index=0;
				foreach (var variant in curTask.Variants) {
					RadioButton variantRadio = new RadioButton (this);
					variantRadio.Text = variant;

					if((aAnswers[currentTaskNum] != NoSelectedAnswers)&&(index == aAnswers[currentTaskNum]))
					{
						variantRadio.Checked = true;
					}
					variantGroup.AddView (variantRadio);
					index++;
				}

				variantsLayout.AddView (variantGroup);
			}
		}

		private void SetCurrentBTask()
		{
			var curTask = test.GetBTask (currentTaskNum);
			if (curTask != null) {
				taskNameLabel.Text = "B" + (currentTaskNum + 1);
				taskLabel.Text = curTask.Desc;
				if (!String.IsNullOrEmpty (curTask.ImageLink)) {
					taskImage.Enabled = true;
					using (Stream ims = Assets.Open (curTask.ImageLink)) {
						Drawable d = Drawable.CreateFromStream (ims, null);
						taskImage.SetImageDrawable (d);
					}
				}
				EditText answer = new EditText (this) { Hint = "Введите ответ" };
				answer.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
					bAnswers [currentTaskNum] = answer.Text;
				};
				if (!String.IsNullOrEmpty (bAnswers [currentTaskNum])) {
					answer.Text = bAnswers [currentTaskNum];
				}
				variantsLayout.AddView (answer);
			}
		}


		private void LoadViews()
		{
			timeLabel = FindViewById<TextView> (Resource.Id.timeLabel);
			subjectNameLabel = FindViewById<TextView> (Resource.Id.subjectNameLabel);
			taskNameLabel = FindViewById<TextView> (Resource.Id.taskNameLabel);

			taskLabel = FindViewById<TextView> (Resource.Id.taskLabel);
			taskImage = FindViewById<ImageView> (Resource.Id.taskImage);
			variantsLayout = FindViewById<LinearLayout> (Resource.Id.variantsLayout);
			passButton = FindViewById<Button> (Resource.Id.passButton);
			passButton.Click += PassButtonClick;

			prevButton = FindViewById<Button> (Resource.Id.prevButton);
			prevButton.Click += PrevButtonClick;

			nextButton = FindViewById<Button> (Resource.Id.nextButton);
			nextButton.Click += NextButtonClick;
		}

		private void PassButtonClick(Object sender, EventArgs args)
		{
			var finishTestIntent = new Intent (this, typeof(FinishTest));
			finishTestIntent.PutExtra ("aAnswers", aAnswers);
			finishTestIntent.PutExtra ("bAnswers", bAnswers);
			var aRightAns = test.GetARightAnswers();
			for (int i=0; i<aRightAns.Length; i++) {
				finishTestIntent.PutExtra ("aRightAnswers"+i.ToString(),aRightAns[i]);
			}
			finishTestIntent.PutExtra ("bRightAnswers", test.GetBRightAnswers());

			StartActivity (finishTestIntent);
		}

		private void PrevButtonClick(Object sender, EventArgs args)
		{
			if (currentTaskNum == 0) {
				if (isATask) {
					currentTaskNum = test.GetBTaskCount - 1;
				} else {
					currentTaskNum = test.GetATaskCount - 1;
				}
				isATask = !isATask;
			} else {
				currentTaskNum--;
			}
			StartTest ();

		}

		private void NextButtonClick(Object sender, EventArgs args)
		{
			if ((isATask && currentTaskNum == test.GetATaskCount - 1) ||
			    (!isATask && currentTaskNum == test.GetBTaskCount - 1)) {
				isATask = !isATask;
				currentTaskNum = 0;
			} else {
				currentTaskNum++;
			}
			StartTest ();
		}
		private void SetSubjectName(TestTypeEnum testType)
		{
			switch (testType) {
			case TestTypeEnum.Belarussian:
				subjectNameLabel.Text = "Беларуская мова";
				break;
			case TestTypeEnum.Math:
				subjectNameLabel.Text = "Математика";
				break;
			case TestTypeEnum.Russian:
				subjectNameLabel.Text = "Русский язык";
				break;
			}
		}

		private Test GetTest(TestTypeEnum testType)
		{
			return new Test (testType, Assets);
		}
	}
}

