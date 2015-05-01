﻿using System.Linq;
using Swashbuckle.Swagger;

namespace VersionedApi.Swagger
{
    public class AddVersionToBasePath : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.BasePath = "/" + swaggerDoc.Info.Version;

            swaggerDoc.Paths = swaggerDoc.Paths.ToDictionary(
                entry => entry.Key.Replace("/{version}", ""),
                entry =>
                {
                    var pathItem = entry.Value;
                    RemoveVersionParamFrom(pathItem.Get);
                    RemoveVersionParamFrom(pathItem.Put);
                    RemoveVersionParamFrom(pathItem.Post);
                    RemoveVersionParamFrom(pathItem.Delete);
                    RemoveVersionParamFrom(pathItem.Options);
                    RemoveVersionParamFrom(pathItem.Head);
                    RemoveVersionParamFrom(pathItem.Patch);
                    return pathItem;
                });
        }

        private void RemoveVersionParamFrom(Operation operation)
        {
            if (operation == null || operation.Parameters == null) return;

            var versionParam = operation.Parameters.FirstOrDefault(param => param.Name == "version");
            if (versionParam == null) return;

            operation.Parameters.Remove(versionParam) ;
        }
    }
}