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
using MR.Android.Data;
using MR.Core.TestEntities;

namespace com.flaxtreme.CT
{
	[Activity (Label = "ChooseTrainingActivity")]			
	public class ChooseTrainingActivity : Activity
	{
		public string subjectStringName;
		protected SubjectsEnumeration subjectType;
		LinearLayout themeButtonsLayout;
		SubjectsEnumeration subject;
		SubjectRetriever subjectRetriever;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.ChooseTrainingLayout);

			Enum.TryParse<SubjectsEnumeration> (Intent.GetStringExtra ("TestType"), out subjectType);
			subjectStringName = SubjectHelper.GetSubjectName (subjectType, this);

			FindViewById<TextView> (Resource.Id.SubjectNameTextView).Text = subjectStringName;

			themeButtonsLayout = FindViewById<LinearLayout> (Resource.Id.ThemesLinearLayout);
			subjectRetriever = new SubjectRetriever (subjectType,this.ApplicationContext);
			LoadThemesButtons ();

		}

		protected void LoadThemesButtons()
		{
			var themesRetriever = new ThemesRetriever (subjectType);
			foreach (var theme in themesRetriever.GetThemes) {
				Button themeButton = new Button (this);
				themeButton.Id = Int32.Parse(theme.Num);
				themeButton.Click += ThemeButtonClick;
				//SetButtonParameters (themeButton);
				int level = GetTasksStatistic(theme);
				themeButton.Text = theme.Num + ')' + theme.Name + "(" + level + "%)";
				SetButtonColor (themeButton, level);
				themeButtonsLayout.AddView (themeButton);
			}
		}
		protected override void OnStart()
		{
			base.OnStart ();
			themeButtonsLayout.RemoveAllViews ();
			LoadThemesButtons ();
		}


		void SetButtonColor (Button button, int level)
		{
			if (level <= 30) {
				button.SetTextColor (Android.Graphics.Color.Red);
			} else {
				if (level <= 80) {
					button.SetTextColor (Android.Graphics.Color.Goldenrod);
				} else {
					button.SetTextColor (Android.Graphics.Color.Green);
				}
			}
		}

		void SetButtonParameters (Button themeButton)
		{
			var parameters = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.WrapContent, 1);
			parameters.SetMargins (16, 8, 16, 8);
			themeButton.LayoutParameters = parameters;
		}

		private int GetTasksStatistic(SubjectTheme theme)
		{

			int answered = subjectRetriever.AnsweredTasksForTheme (theme);
			int overall = subjectRetriever.OverallTasksForTheme (theme);
			if (overall == 0) {
				return 0;
			}
			return (int)(((float)answered) / overall * 100);
		}

		protected void ThemeButtonClick(object sender, EventArgs e)
		{
			var intent = new Intent (this, typeof(ChooseTrainingTasksActivity));
			intent.PutExtra ("SubjectType", subjectType.ToString());
			intent.PutExtra ("ThemeNum", ((Button)sender).Id);
			StartActivity (intent);
		}
		protected void OnDestroy()
		{
			base.OnDestroy ();
			subjectRetriever.DB.Close ();
		}
	}
}

