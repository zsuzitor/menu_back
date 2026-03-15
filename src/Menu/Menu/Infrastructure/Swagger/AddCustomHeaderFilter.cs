using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Menu.Host.Infrastructure.Swagger
{
    public class AddCustomHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<IOpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Authorization_Access_Token",
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = JsonSchemaType.String
                }
            });
        }
    }
}
