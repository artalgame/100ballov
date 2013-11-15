using System;
using System.Collections.Generic;

namespace MR.Core.TestEntities
{
	public class ATask:Task
	{
		protected bool[] checkedAnswers;
		public List<AVariant> Variants{ get; set; }

		public ATask (int taskNum):base(taskNum)
		{
		}

		public List<AVariant> RightVariants {
			get {
				List<AVariant> variants = new List<AVariant> ();
				foreach (var variant in Variants) {
					if (variant.IsRight)
						variants.Add (variant);
				}
				return variants;
			}
		}

		public bool[] CheckedAnswers
		{
			get {
				if (checkedAnswers == null) {
					checkedAnswers = new bool[Variants.Count];
				}
				return checkedAnswers;
			}
			set{
				checkedAnswers = value;
			}
		}
	}
}


