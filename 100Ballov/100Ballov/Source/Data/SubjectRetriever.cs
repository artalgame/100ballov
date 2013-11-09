using System;
using MR.Core.Interfaces;
using MR.Core.TestEntities;
using Android.Content.Res;
using System.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using com.flaxtreme.CT;

namespace MR.Android.Data
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
			string fileName = subject.ToString () + "/" + theme.Num.ToString () + @"/a/" + num.ToString () + "/" + "task.txt"; 
			var assets = MRApplication.GetAssetManager ();

			try
			{
			using (StreamReader reader = new  StreamReader(assets.Open(fileName))) {
				string content = reader.ReadToEnd ();

				var json = JsonObject.Parse (content);
				JsonObject jsonTask = json ["task"] as JsonObject;

				var aTask = (ATask) GetTask(jsonTask, num);
				
				List<AVariant> variants = new List<AVariant> ();
				JsonArray variantsJSON = jsonTask ["vs"] as JsonArray;
				foreach (var variant in variantsJSON) {
					string text = variant ["txt"];
					string png = variant ["png"];
					bool isRight = Boolean.Parse (variant ["right"]);
					AVariant newVariant = new AVariant () { ImageLink = png, IsRight = isRight, Text = text };     
					variants.Add (newVariant);
				}

				aTask.Variants = variants;
				return aTask;
				}
			}
			catch(Exception e) {
				return null;
			}
		}

		private Task GetTask(JsonObject jsonTask, int num)
		{
			Task task = new Task (num);
			task.QuestionText = jsonTask ["q_txt"];
			task.QuestionImageLink = jsonTask ["q_png"];
			task.SolutionTxt = jsonTask ["s_txt"];
			task.SolutionImageLink = jsonTask ["s_png"];
			task.SolutionInetLink = jsonTask ["s_link"];
			task.SolutionLocalLink = jsonTask ["s_id"];
			return task;
		}
		
		public BTask GetBTask(string themeNum, int num)
		{
			return GetBTask (themes.FirstOrDefault (x => x.Num == themeNum), num);
		}

		public BTask GetBTask(SubjectTheme theme, int num)
		{
			string fileName = subject.ToString () + "/" + theme.Num.ToString () + @"/b/" + num.ToString () + "/" + "task.txt"; 
			var assets = MRApplication.GetAssetManager ();

			try
			{
				using (StreamReader reader = new  StreamReader(assets.Open(fileName))) {
					string content = reader.ReadToEnd ();

					var json = JsonObject.Parse (content);
					JsonObject jsonTask = json ["task"] as JsonObject;

					var bTask = (BTask)GetTask (jsonTask, num);


					List<AVariant> variants = new List<AVariant> ();
					bTask.Variant = jsonTask ["variant"];
					return bTask;
				}
			}
			catch(Exception e) {
				return null;
			}
		}
	

		public List<Task> GetTasks (string themeNum)
		{
			int taskNum = 1;
			List<Task> tasks = new List<Task> ();
			Task task = GetATask (themeNum, taskNum);

			while (task != null) {
				tasks.Add (task);
				task = GetATask (themeNum, taskNum);
			}

			taskNum=1;
			task = GetBTask (themeNum, taskNum);
			while (task != null) {
				tasks.Add (task);
				task = GetBTask (themeNum, taskNum);
			}
			return tasks;
		}
	}
}

