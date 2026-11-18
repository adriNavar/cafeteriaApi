using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Core.mediatOR.Contracts;
using Starbucks.Application.Abstractions;
using Starbucks.Application.Flavors.DTOs;
using System.Diagnostics;

namespace Starbucks.Application.Flavors.Commands;
public class FlavorImageUpdate
{
    public class Command(int flavorId, FlavorUpdateRequest flavorUpdateRequest)
        : IRequest<Result<int>>
    {
        public FlavorUpdateRequest FlavorUpdateRequest { get; } = flavorUpdateRequest;
        public int FlavorId { get; } = flavorId;
    }

    public class Handler(BlobServiceClient flavorBlobServiceClient)
        : IRequestHandler<Command, Result<int>>
    {
        private readonly BlobServiceClient _flavorBlobServiceClient = flavorBlobServiceClient;

        public async Task<Result<int>> Handle(Command request, CancellationToken cancellationToken)
        {
            var flavorImagesContainerClient = _flavorBlobServiceClient
                                    .GetBlobContainerClient("flavor-images");

            var blobClient = flavorImagesContainerClient.GetBlobClient(
                request.FlavorUpdateRequest.Name
             );

            // Definimos las opciones comunes de subida, incluyendo el tipo de contenido (Content-Type)
            // para que Azure sepa que es una imagen (ej. image/jpeg, image/png) y no un archivo genérico.
            var uploadOptions = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = "image/jpeg" // O podés extraerlo dinámicamente si tu DTO lo trae, pero 'image/jpeg' o 'image/png' soluciona la visualización
                },
                Tags = new Dictionary<string, string>
                {
                    { "FlavorId", request.FlavorId.ToString() }
                }
            };

            if (!blobClient.Exists(cancellationToken).Value)
            {
                await blobClient.UploadAsync(
                    request.FlavorUpdateRequest.Stream,
                    uploadOptions,
                    cancellationToken
                );
            }
            else
            {
                var tags = blobClient.GetTags(cancellationToken: cancellationToken);
                if (tags.Value.Tags.TryGetValue("FlavorId", out var flavorId)
                            && flavorId == request.FlavorId.ToString())
                {
                    await blobClient.UploadAsync(
                        request.FlavorUpdateRequest.Stream,
                        uploadOptions,
                        cancellationToken
                    );
                }
            }

            return Result<int>.Success(request.FlavorId);
        }
    }
}