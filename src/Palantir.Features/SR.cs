namespace Palantir.Features
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Threading.Tasks;

	internal static class SR
    {
		public static string Err_NotAFeatureRuleType(Type type)
		{
			return string.Format(Resource.Err_NotAFeatureRuleType, type.AssemblyQualifiedName);
		}

		public static string Err_NotARuleType(string type)
		{
			return string.Format(Resource.Err_NotAFeatureRuleType, type);
		}

		internal static string Err_ObsoleteFeatureError(string feature)
		{
			return string.Format(Resource.Err_ObsoleteFeatureError, feature);
		}

		internal static string Err_FeaturePropertyMustBeWritable(string feature)
		{
			return string.Format(Resource.Err_FeaturePropertyMustBeWritable, feature);
		}
	}
}
