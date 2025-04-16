using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Npgsql;

namespace Services
{
    public partial class ExceptionMiddlewares(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException postgresEx)
            {
                await ProcessErrorProstgresAsync(context, postgresEx);
            }
            catch (Exception ex)
            {
                await ProcessExceptionGlobalAsync(context, ex);
            }
        }

        private static Task ProcessErrorProstgresAsync(HttpContext context, PostgresException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            string errorMessage = ex.SqlState switch
            {
                "23503" => "El registro no puede ser eliminado o modificado porque está en uso.",
                "23505" => "Duplicado: ya existe un registro con datos únicos similares.",
                "42703" => "Columna no encontrada: revisa la estructura de la base de datos.",
                "42P01" => "Tabla inexistente: verifica si la tabla fue creada.",
                "42P10" => "La columna referenciada no está presente en la tabla.",
                "42P22" => "Falló la validación de datos (CHECK constraint).",
                "42P91" => "Restricción de llave foránea inexistente.",
                "42P99" => "Error de sintaxis query SQL: verifica la consulta.",
                "XX000" => "Error interno desconocido en DB.",
                _ => "Se produjo un error no previsto con la base de datos.",
            };
            var response = new
            {
                errorMessage,
                success = false,
                error = $"PostgresException -> {ex}",
                type = "error",
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }

        private static Task ProcessExceptionGlobalAsync(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new
            {
                success = false,
                message = ex.Message,
                error = $"Exception -> {ex}",
                type = "info",
            };
            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }

        [GeneratedRegex(@"en la table «(\w+)»")]
        private static partial Regex MyRegex();
    }
}
