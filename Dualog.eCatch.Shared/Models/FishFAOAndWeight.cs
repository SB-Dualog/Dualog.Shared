using System;
using Dualog.eCatch.Shared.Enums;
using Dualog.eCatch.Shared.Extensions;

namespace Dualog.eCatch.Shared.Models
{
	public class FishFAOAndWeight : IComparable<FishFAOAndWeight>
	{
		public string FAOCode { get; private set; }
		public int Weight { get ; }

		public FishFAOAndWeight(string faocode, int weight)
		{
			FAOCode = faocode;
			Weight = weight;
		}

	    public void SetFAOCode(string code)
	    {
	        FAOCode = code;
	    }

	    public int CompareTo(FishFAOAndWeight other) => StringComparer.OrdinalIgnoreCase.Compare(FAOCode, other.FAOCode);

	    public override string ToString() => $"{FAOCode} {Weight}";

	    public string ToReadableFormat(EcatchLangauge lang)
	    {
	        return $"{Weight.WithThousandSeparator()} kg {FAOCode.ToFishName(lang)}";
	    }
	}
}