namespace Talabat.APIs.Extensions
{
    public static class AddSwaggerExtension
    {
        public static WebApplication UserSwaggerMiddlewares(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }
}
