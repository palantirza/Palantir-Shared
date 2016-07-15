namespace Palantir.Features
{
	using Configuration;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Threading.Tasks;

	/// <summary>
	/// An expression parser for features.
	/// </summary>
	internal class ExpressionParser
	{
		/// <summary>
		/// Fetches the qualified name (Class.Member) for the feature.
		/// </summary>
		/// <typeparam name="TFeatures">The features.</typeparam>
		/// <param name="featureSelector">The feature selector.</param>
		/// <returns>The qualified name.</returns>
		public static string GetQualifiedName<TFeatures>(Expression<Func<TFeatures, bool>> featureSelector) where TFeatures : class
		{
			var lambda = (LambdaExpression)featureSelector;
			MemberExpression memberExpression;

			if (lambda.Body is UnaryExpression)
			{
				var unaryExpression = (UnaryExpression)(lambda.Body);
				memberExpression = (MemberExpression)(unaryExpression.Operand);
			}
			else
			{
				memberExpression = (MemberExpression)(lambda.Body);
			}

			var nameAttribute = ((PropertyInfo)memberExpression.Member).GetCustomAttribute<FeatureNameAttribute>();
			if (nameAttribute == null)
				return memberExpression.Member.DeclaringType.Name + "." + ((PropertyInfo)memberExpression.Member).Name;
			else
				return nameAttribute.Name;
		}
	}
}
