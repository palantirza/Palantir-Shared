namespace Palantir.Features
{
	public interface IFeatureContext
	{
		object this[string key] { get; }
	}
}