using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;
using System.Reflection.Metadata;

namespace TestSwaggerGen.Swaggers;

public class CustomDocumentFilter : IDocumentFilter
{
    //DI repos maybe or file config

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        //Demo modify document
        var defaultPathItem = new DataDictionaryPathItem($"/Custom/Endpoint1/{{id}}");

        //Add Get
        defaultPathItem.AddOperation(OperationType.Get, ExampleGetApiOperation());

        //Add Delete

        defaultPathItem.AddOperation(OperationType.Delete, ExampleDeleteApiOperation());


        swaggerDoc.Paths.AddDataDictionaryPath(defaultPathItem);
    }

    private OpenApiOperation ExampleGetApiOperation()
    {
        var defaultModelName = "ModelName";
        var operation = new OpenApiOperation()
        {
            Summary = "Get a specific record",
            Description = defaultModelName + "<br><b>Accept-Encoding</b>: gzip, deflate ou utf8 (opcional)",
            OperationId = defaultModelName + "_Get",
            Tags = new List<OpenApiTag>
            {
                new (){Name = defaultModelName, Description = "Des1"}
            },
            Responses = new OpenApiResponses
            {
                {
                    "200",
                    new OpenApiResponse
                    {
                        Description = "Success",
                        Content = new Dictionary<string, OpenApiMediaType>
                        {
                            {
                                "application/json", new OpenApiMediaType
                                {
                                    Encoding = new Dictionary<string, OpenApiEncoding>
                                    {
                                        {"utf-8", new OpenApiEncoding{ContentType = "application/json"} }
                                    },
                                    Schema = GetResponseSchema(defaultModelName)
                                }
                            }
                        }
                    }
                }
            },
            Parameters = new List<OpenApiParameter>
            {
                new()
                {
                    Name = "Id".ToLower(),
                    Description = "Primary Key Value.<br>" + "Id",
                    In = ParameterLocation.Path,
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "string"
                    }
                }
            }
        }; 
        return operation;
    }

    private OpenApiOperation ExampleDeleteApiOperation()
    {
        var defaultModelName = "ModelName";
        var operation = new OpenApiOperation()
        {
            Summary = "Delete a specific record",
            Description = defaultModelName + "<br><b>Accept-Encoding</b>: gzip, deflate ou utf8 (opcional)",
            OperationId = defaultModelName + "_Get",
            Tags = new List<OpenApiTag>
            {
                new (){Name = defaultModelName, Description = "Des1"}
            },
            Responses = new OpenApiResponses
            {
                {
                    "200",
                    new OpenApiResponse
                    {
                        Description = "Success",
                        Content = new Dictionary<string, OpenApiMediaType>
                        {
                            {
                                "application/json", new OpenApiMediaType
                                {
                                    Encoding = new Dictionary<string, OpenApiEncoding>
                                    {
                                        {"utf-8", new OpenApiEncoding{ContentType = "application/json"} }
                                    },
                                    Schema = GetResponseSchema(defaultModelName)
                                }
                            }
                        }
                    }
                }
            },
            Parameters = new List<OpenApiParameter>
            {
                new()
                {
                    Name = "Id".ToLower(),
                    Description = "Primary Key Value.<br>" + "Id",
                    In = ParameterLocation.Path,
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "string"
                    }
                }
            }
        };
        return operation;
    }
    private  OpenApiSchema GetResponseSchema(string modelName)
    {
        return new OpenApiSchema
        {
            Title = modelName + "Status",
            Type = "array",
            Items = GetValidationLetterSchema(true),
            Description = "List with status and validations"
        };
    }

    private OpenApiSchema GetValidationLetterSchema(bool enableDataField = false)
    {
        var modelSchema = new OpenApiSchema
        {
            Title = "validationLetter",
            Type = "object",
            Properties = new Dictionary<string, OpenApiSchema>()
        };
        modelSchema.Properties.Add("status", new OpenApiSchema
        {
            Description = "Http Response Code",
            Type = "integer",
            Format = "int32"
        });

        modelSchema.Properties.Add("message", new OpenApiSchema
        {
            Description = "Error Message",
            Type = "string"
        });

        modelSchema.Properties.Add("validationList", new OpenApiSchema
        {
            Description = "Detailed error list",
            Type = "array",
            Items = new OpenApiSchema
            {
                Type = "object",
                Description = "Field, Value"
            },
            Properties = new Dictionary<string, OpenApiSchema>
            {
                {
                    "errorList",
                    new OpenApiSchema
                    {
                        Type = "object",
                        Description = "Field, Value"
                    }
                }
            }
        });

        if (enableDataField)
        {
            modelSchema.Properties.Add("data", new OpenApiSchema
            {
                Description = "Return of fields, identity for example",
                Type = "array",
                Items = new OpenApiSchema
                {
                    Description = "Field, Value",
                    Type = "object"
                }
            });
        }

        modelSchema.Required.Add("status");
        modelSchema.Required.Add("message");

        return modelSchema;
    }
}
