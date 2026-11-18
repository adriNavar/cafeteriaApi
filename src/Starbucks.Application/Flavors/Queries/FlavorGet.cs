using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Core.mediatOR.Contracts;
using Starbucks.Application.Abstractions;
using Starbucks.Application.Flavors.DTOs;
using Starbucks.Domain.Abstractions;

namespace Starbucks.Application.Flavors.Queries;
public class FlavorGet
{
    public class Query : IRequest<Result<List<FlavorResponse>>>
    {
    }

    public class Handler(
     TableServiceClient tableServiceClient,
     BlobServiceClient flavorBlobServiceClient //lo inyyecto
    ) : IRequestHandler<Query, Result<List<FlavorResponse>>>
    {
        private readonly TableServiceClient _tableServiceClient = tableServiceClient;
        private readonly BlobServiceClient _flavorBlobServiceClient = flavorBlobServiceClient;
        public Task<Result<List<FlavorResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
         )
        {
            var flavorCategoriesTableClient = _tableServiceClient
                                                .GetTableClient("CafeCategories");

            //var filter = TableClient.CreateQueryFilter($"Id eq 1");
            var filter = "";
            var quantity = 100;

            var flavors = flavorCategoriesTableClient.Query<TableEntity>(
                filter,
                quantity,
                ["Id", "Name", "Description"],
                cancellationToken
                );

            var results = flavors.AsPages()
                                .SelectMany(page => page.Values)
                                .Select(entity => new FlavorResponse
                                {
                                    Id = entity.GetInt64("Id") ?? -1,
                                    Name = entity.GetString("Name"),
                                    Description = entity.GetString("Description")
                                }).ToList();
            if (results.Count == 0)
            {
                return Task.FromResult(Result<List<FlavorResponse>>
                            .Failure(
                                        new Error("NO_ELEMENTS", "No category flavors")
                                    )
                            );
            }

            var flavorImagesContainerCient = _flavorBlobServiceClient
                    .GetBlobContainerClient("flavor-images");

            foreach (var flavor in results)
            {
                var blobs = flavorImagesContainerCient.FindBlobsByTags(
                    $"FlavorId='{flavor.Id}'"
                );

                foreach (var blob in blobs)
                {
                    var blobClient = flavorImagesContainerCient
                        .GetBlobClient(blob.BlobName);
                    flavor.Images.Add(blobClient.Uri.AbsoluteUri);
                }
            }


            return Task.FromResult(Result<List<FlavorResponse>>.Success(results));
        }
    }

}
