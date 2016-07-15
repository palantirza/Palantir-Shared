namespace Palantir.Mvc
{
    using Microsoft.AspNetCore.Mvc;

	/// <summary>
	/// Controller extension methods for results.
	/// </summary>
	public static class ResultControllerExtensions
	{
		/// <summary>
		/// Return a 500 - Internal Server error.
		/// </summary>
		/// <param name="controller">The controller.</param>
		/// <returns>The result.</returns>
		public static HttpInternalServerErrorResult HttpInternalServerError(this Controller controller)
		{
			return new HttpInternalServerErrorResult();
		}

		/// <summary>
		/// Return a 202 - Accepted result.
		/// </summary>
		/// <param name="controller">The controller.</param>
		/// <returns>The result.</returns>
		public static HttpAcceptedResult HttpAccepted(this Controller controller)
		{
			return new HttpAcceptedResult();
		}

		/// <summary>
		/// Return a 403 - Forbidden result.
		/// </summary>
		/// <param name="controller">The controller.</param>
		/// <returns>The result.</returns>
		public static HttpForbiddenResult HttpForbidden(this Controller controller)
		{
			return new HttpForbiddenResult();
		}

		/// <summary>
		/// Return a 409 - Conflict result.
		/// </summary>
		/// <param name="controller">The controller.</param>
		/// <returns>The result.</returns>
		public static HttpConflictResult HttpConflict(this Controller controller)
		{
			return new HttpConflictResult();
		}

	}
}
