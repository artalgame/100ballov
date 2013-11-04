using System;
using System.Collections.Generic;

namespace MR.Core.TestEntities
{
	public class ATask
	{
		public String QuestionText {get;set;}
		public String QuestionImageLink{ get; set; }

		public List<AVariant> Variants{ get; set; }

		public String SolutionTxt{ get; set; }
		public String SolutionImageLink{ get; set; }
		public String SolutionInetLink{ get; set; }
		public String SolutionLocalLink{ get; set; }

		public ATask ()
		{

		}

		public List<AVariant> RightVariants {
			get {
				List<AVariant> variants = new List<AVariant> ();
				foreach (var variant in Variants) {
					if (variant.IsRight)
						variants.Add(variant);
				}
				return variants;
			}
		}
	}
}


