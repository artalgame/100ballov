using System;
using System.Collections.Generic;
using MR.Core.TestEntities;
using Android.Content.Res;
using System.IO;
using System.Json;


namespace com.flaxtreme.CT
{
	public class ThemesRetriever
	{
		private SubjectsEnumeration subject;
		private List<SubjectTheme> themes;
		public ThemesRetriever (SubjectsEnumeration subject)
		{
			this.subject = subject;
			RetrieveThemes ();
		}
		private void RetrieveThemes()
		{
			switch (subject) 
			{
			case SubjectsEnumeration.Math:
				{
					RetrieveSubjectTheme(@"Math/Themes.txt");
					break;
				}
			}
		}

		public List<SubjectTheme> GetThemes {
			get{ return themes; }
		}

		private void RetrieveSubjectTheme(string fileName)
		{
			themes = new List<SubjectTheme> ();
			AssetManager assets = MRApplication.GetAssetManager ();

			using (StreamReader reader = new  StreamReader(assets.Open(fileName))) {
				string content = reader.ReadToEnd ();

				var json = JsonObject.Parse (content);

				JsonArray themesJson = json ["Themes"] as JsonArray;
				foreach (JsonObject theme in themesJson) {
					SubjectTheme newTheme = new SubjectTheme (theme ["Num"], theme ["Name"], theme ["InetLink"], theme ["LocalLink"]);
					themes.Add (newTheme);
				}
			}
		}
	}
}

