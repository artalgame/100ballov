using System;

namespace MR.Core.TestEntities
{
	/// <summary>
	/// Task theme. Описывает тему в предмете - ее номер, название, ссылку на ресурсы в интеренете и внутри приложения.
	/// </summary>
	public class SubjectTheme
	{
		public string Num {
			get;
			private set;
		}

		public string Name {
			get;
			private set;
		}

		public string InternetURL {
			get;
			private set;
		}

		public string LocalURL {
			get;
			private set;
		}

		public SubjectTheme (string num, string name, string inetURL, string localURL)
		{
			Num = num;
			Name = name;
			InternetURL = inetURL;
			LocalURL = localURL;
		}
	}
}

