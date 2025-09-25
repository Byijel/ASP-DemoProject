namespace Ap.Demo.API.Extensions
{
	public static class Registrator
	{
		public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder app)
		{
			app.UseMiddleware<Ap.Demo.API.Middleware.ExceptionHandlingMiddleware>();
			return app;
		}
	}
}