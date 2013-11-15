using System;
using MR.Core.Interfaces;
using MR.Core.TestEntities;
using Android.Content.Res;
using System.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using com.flaxtreme.CT;
using Android.Graphics.Drawables;

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
			return GetATask (GetThemeByNum(themeNum), num);
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

					var aTask= new ATask(num);
					SetCommonTaskParams(jsonTask, aTask);
				
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

		private void SetCommonTaskParams(JsonObject jsonTask, Task task)
		{
			task.QuestionText = jsonTask ["q_txt"];
			task.QuestionImageLink = jsonTask ["q_png"];
			task.SolutionTxt = jsonTask ["s_txt"];
			task.SolutionImageLink = jsonTask ["s_png"];
			task.SolutionInetLink = jsonTask ["s_link"];
			task.SolutionLocalLink = jsonTask ["s_id"];
		}
		
		public BTask GetBTask(string themeNum, int num)
		{
			return GetBTask (GetThemeByNum(themeNum), num);
		}

		public SubjectTheme GetThemeByNum(string themeNum)
		{
			return themes.FirstOrDefault (x => x.Num == themeNum);
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

					var bTask = new BTask(num);
					SetCommonTaskParams (jsonTask, bTask);


					List<AVariant> variants = new List<AVariant> ();
					bTask.Variant = jsonTask ["variant"];
					return bTask;
				}
			}
			catch(Exception e) {
				return null;
			}
		}

		public bool IsATaskExist(SubjectTheme theme, int num)
		{
			string fileName = subject.ToString () + "/" + theme.Num.ToString () + @"/a/" + num.ToString () + "/" + "task.txt"; 
			return IsTaskExist (fileName);
		}

		public bool IsBTaskExist(SubjectTheme theme, int num)
		{
			string fileName = subject.ToString () + "/" + theme.Num.ToString () + @"/b/" + num.ToString () + "/" + "task.txt"; 
			return IsTaskExist (fileName);
		}

		private bool IsTaskExist(string fileName)
		{
			var assets = MRApplication.GetAssetManager ();
			try
			{
				using (StreamReader reader = new  StreamReader(assets.Open(fileName))) {
					return true;
				}
			}
			catch(Exception)
			{
				return false;
			}
		}
	

		public List<Task> GetTasks (string themeNum)
		{
				int taskNum = 1;
				var tasks = new List<Task> ();
				Task task = GetATask (themeNum, taskNum);

				while (task != null) {
					tasks.Add (task);
					taskNum++;
					task = GetATask (themeNum, taskNum);
				}

				taskNum = 1;
				task = GetBTask (themeNum, taskNum);
				while (task != null) {
					tasks.Add (task);
					taskNum++;
					task = GetBTask (themeNum, taskNum);
				}
			return tasks;
		}
		public List<Type> GetTasksInfo(string themeNum)
		{
				SubjectTheme theme = GetThemeByNum (themeNum);
				int taskNum = 1;
				var tasksInfo = new List<Type> ();
				bool taskExits = IsATaskExist (theme, taskNum);

				while (taskExits) {
					tasksInfo.Add (typeof(ATask));
					taskNum++;
					taskExits = IsATaskExist (theme, taskNum);
				}

				taskNum = 1;
				taskExits = IsBTaskExist (theme, taskNum);
				while (taskExits) {
					tasksInfo.Add (typeof(BTask));
					taskNum++;
					taskExits = IsBTaskExist (theme, taskNum);
				}
			return tasksInfo;
		}
		public int GetATasksCount (string themeNum){
			return GetTasksInfo(themeNum).Count (x => x == typeof(ATask));
		}

		public int GetBTasksCount (string themeNum){
			return GetTasksInfo(themeNum).Count (x => x == typeof(BTask));
		}

		public Drawable GetImage(string themeNum, int taskNum, string imageFileName, string taskType="")
		{
			try {

				var assets = MRApplication.GetAssetManager ();
				string fileName = subject.ToString () + "/" + themeNum + @"/" + taskType + @"/" + taskNum.ToString () + "/" + imageFileName;
				using (Stream ims = assets.Open (fileName)) {
					Drawable d = Drawable.CreateFromStream (ims, null);
					return d;
				}
			} catch (Exception) {
				return null;
			}
		}
	}
}

