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
	[Activity (Label = "FinishTest")]			
	public class FinishTest : Activity
	{

		List<List<int>> aRightAnswers;
		List<int> aAnswers;
		List<string> bRightAnswers;
		List<string> bAnswers;

		int rating = 0;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.FinishTest);

			ReadExtras ();
			rating = CalcRating ();
			var resultTextView =(TextView) FindViewById (Resource.Id.testResultTextView);
			resultTextView.Text ="Ваш результат " + rating + " баллов";
		}

		private int CalcRating(){
			//all tasks = 100%, aPart = 50% and bPart = 50%, aTask = 50%/aCount; bTask = 50%/bCount;
			float aPart = 50f / aAnswers.Count;
			float bPart = 50f / bAnswers.Count;

			float sum = 0f;
			int i = 0;
			foreach(var ans in aAnswers)
			{
				if(aRightAnswers[i].Contains(ans+1))
				{
					sum += aPart;
				}
				i++;
			}

			for(i=0; i<bAnswers.Count; i++)
			{
				if(bAnswers[i] == bRightAnswers[i])
				{
					sum+= bPart;
				}
			}
			return (int)Math.Round(sum);
		}

		private void ReadExtras()
		{
			aRightAnswers = new List<List<int>> ();
			aAnswers = Intent.GetIntArrayExtra ("aAnswers").ToList();
					//finishTestIntent.PutExtra ("bAnswers", bAnswers);
			bAnswers = Intent.GetStringArrayExtra ("bAnswers").ToList();
			//finishTestIntent.PutExtra ("aRightAnswers", test.GetARightAnswers);
			var aCurRightAnswers = Intent.GetIntArrayExtra ("aRightAnswers0");
			int i=1;
			while (aCurRightAnswers != null) {
				aRightAnswers.Add (aCurRightAnswers.ToList ());
				aCurRightAnswers = Intent.GetIntArrayExtra ("aRightAnswers" + i);
				i++;
			}
			//finishTestIntent.PutExtra ("bRightAnswers", test.GetBRightAnswers);
			bRightAnswers = Intent.GetStringArrayExtra ("bRightAnswers").ToList ();
		}
	}
}

