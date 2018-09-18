using Dualog.eCatch.Shared.Contracts;

namespace Dualog.eCatch.Shared.Models
{
	public class FishSpecies : IHasId
	{
		public string Code { get; }
		public string NorwegianName { get; }
		public string EnglishName { get; }
		public string LatinName { get; }

	    public string Id => Code;
        public string Name => DualogLanguage.SelectedCulture == "en-GB" ? EnglishName : NorwegianName;

        public FishSpecies(string code, string norwegianName, string englishName, string latinName)
		{
			Code = code;
			NorwegianName = norwegianName;
			EnglishName = englishName;
			LatinName = latinName;
		}

	    public override string ToString() => Name;

		public override int GetHashCode()
		{
			return new { Code, NorwegianName, EnglishName, LatinName }.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			var rhs = obj as FishSpecies;
			if (rhs == null) return false;
			return rhs.GetHashCode() == GetHashCode();
		}
	}
}