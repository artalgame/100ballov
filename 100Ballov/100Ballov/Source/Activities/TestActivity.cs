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

namespace Ballov.Activites
{
	[Activity (Label = "TestActivity")]			
	public class TestActivity : Activity
	{
		Test test = null;
		TextView timeLabel = null;
		TextView subjectNameLabel = null;
		TextView taskNameLabel = null;

		TextView taskLabel = null;
		ImageView taskImage = null;
		LinearLayout variantsLayout = null;
		Button passButton = null;

		bool isATask = true;
		int currentTaskNum = 3;

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
				StartTest ();
			}

		}

		private void StartTest()
		{
			if (isATask) 
			{
				var curTask = test.GetATask (currentTaskNum);
				if (curTask != null) {
					taskNameLabel.Text = "A" + (currentTaskNum + 1);
					taskLabel.Text = curTask.Desc;
					if (!String.IsNullOrEmpty(curTask.ImageLink)) {
						taskImage.Enabled = true;
						using (Stream ims = Assets.Open (curTask.ImageLink)) {
							Drawable d = Drawable.CreateFromStream (ims, null);
							taskImage.SetImageDrawable (d);
						}
					}else{
						taskImage.Enabled = false;
					}

					RadioGroup variantGroup = new RadioGroup (this);
					variantGroup.Orientation = Orientation.Vertical;
					foreach (var variant in curTask.Variants) {
						RadioButton variantRadio = new RadioButton (this);
						variantRadio.Text = variant;
						variantGroup.AddView (variantRadio);
					}
					variantsLayout.AddView (variantGroup);
				}
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

