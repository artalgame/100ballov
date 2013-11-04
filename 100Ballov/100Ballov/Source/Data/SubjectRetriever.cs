using System;
using MR.Core.Interfaces;
using MR.Core.TestEntities;
using Android.Content.Res;
using System.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace com.flaxtreme.CT
{
	public class SubjectRetriever:ISubjectRetriver
	{
		private SubjectsEnumeration subject;
		private List<SubjectTheme> themes;

		public SubjectRetriever (SubjectsEnumeration subject)
		{
			this.subject = subject;
			themes = new ThemesRetriever (subject).GetThemes;
		}

		public List<SubjectTheme> Themes{
			get{ 
				return themes; 
			}
		}

		public SubjectsEnumeration Subject {
			get {
				return subject;
			}
		}

		public ATask GetATask(string themeNum, int num)
		{
			return GetATask (themes.FirstOrDefault (x => x.Num == themeNum), num);
		}

		public ATask GetATask(SubjectTheme theme, int num)
		{
			string fileName = subject.ToString () + "/" + theme.Num.ToString () + @"/a" + num.ToString () + "/" + "task.txt"; 
			var assets = MRApplication.GetAssetManager ();

			using (StreamReader reader = new  StreamReader(assets.Open(fileName))) {
				string content = reader.ReadToEnd ();

				var json = JsonObject.Parse (content);

				JsonObject task = json ["task"] as JsonObject;

				string questText = task ["q_txt"];
				string questImageLink = task ["q_png"];
				string solutionText = task ["s_txt"];
				string solutionImageLink = task ["s_png"];
				string solutionInetLink = task ["s_link"];
				string solutionLocalLink = task ["s_id"];
				List<AVariant> variants = new List<AVariant> ();
				JsonArray variantsJSON = task ["vs"] as JsonArray;
				foreach (var variant in variantsJSON) {
					string text = variant ["txt"];
					string png = variant ["png"];
					bool isRight = Boolean.Parse (variant ["right"]);
					AVariant newVariant = new AVariant () { ImageLink = png, IsRight = isRight, Text = text };     
					variants.Add (newVariant);
				}
				return new ATask () {
					QuestionImageLink = questImageLink,
					QuestionText = questText,
					SolutionImageLink = solutionImageLink,
					SolutionInetLink = solutionInetLink,
					SolutionLocalLink = solutionLocalLink,
					SolutionTxt = solutionText,
					Variants = variants
				};
			}
		}
	}
}

