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

namespace Ballov
{
	[Activity (Label = "PreTestActivity")]			
	public class PreTestActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.PreTest);

			TestTypeEnum testType = TestTypeEnum.No;
			Enum.TryParse<TestTypeEnum> (Intent.GetStringExtra ("TestType"), out testType);
			if (testType != TestTypeEnum.No) 
			{
				SetAnnotation (testType);
				var test = GetTest (testType);
				var startButton = FindViewById (Resource.Id.StartButton);
				startButton.Click += (object sender, EventArgs e) => 
				{
					StartActivity(typeof(TestActivity));
				};
			} 
			else
			{
				StartActivity (typeof(MainActivity));
			}
		}

		private Test GetTest(TestTypeEnum testType)
		{
			return new Test (testType, Assets);
		}

		private void SetAnnotation(TestTypeEnum testType)
		{
			var annoText = FindViewById<TextView> (Resource.Id.AnnoText);
			switch (testType) 
			{
			case TestTypeEnum.Belarussian:
				annoText.Text = "Вы выбралі тэст па беларускай мове. Вам будзе дадзена дзве гадзіны на рашэнне заданняу часткі А і Б. Жадаем поспехаў!";
				break;
			case TestTypeEnum.Math:
				annoText.Text = "Вы выбрали тест по математике. У Вас есть два часа на решение заданий части А и Б. Желаем удачи!";
				break;
			case TestTypeEnum.Russian:
				annoText.Text = "Вы выбрали тест по русскому языку. У Вас есть два часа на решение заданий части А и Б. Желаем удачи!";
				break;
			}
		}
	}
}

