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

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.ChooseTrainingLayout);

			Enum.TryParse<SubjectsEnumeration> (Intent.GetStringExtra ("TestType"), out subjectType);
			subjectStringName = SubjectHelper.GetSubjectName (subjectType, this);

			FindViewById<TextView> (Resource.Id.SubjectNameTextView).Text = subjectType.ToString ();

			themeButtonsLayout = FindViewById<LinearLayout> (Resource.Id.ThemesLinearLayout);
			LoadThemesButtons ();
		}

		protected void LoadThemesButtons()
		{
			var themesRetriever = new ThemesRetriever (subjectType);
			foreach (var theme in themesRetriever.GetThemes) {
				Button themeButton = new Button (this);
				themeButton.Id = Int32.Parse(theme.Num);
				themeButton.Click += ThemeButtonClick;
				themeButton.Text = theme.Num + ')' + theme.Name + "(3/5)";
				themeButtonsLayout.AddView (themeButton);
			}
		}
		protected void ThemeButtonClick(object sender, EventArgs e)
		{
			var intent = new Intent (this, typeof(ChooseTrainingTasksActivity));
			intent.PutExtra ("SubjectType", subjectType.ToString());
			intent.PutExtra ("ThemeNum", ((Button)sender).Id);
			StartActivity (intent);
		}
	}
}

