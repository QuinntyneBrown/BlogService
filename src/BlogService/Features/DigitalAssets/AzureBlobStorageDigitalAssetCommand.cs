using MediatR;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using BlogService.Data;
using BlogService.Features.Core;
using BlogService.Features.DigitalAssets.UploadHandlers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace BlogService.Features.DigitalAssets
{
    public class AzureBlobStorageDigitalAssetCommand
    {
        public class AzureBlobStorageDigitalAssetRequest : IRequest<AzureBlobStorageDigitalAssetResponse>
        {
            public Guid TenantUniqueId { get; set; }
            public InMemoryMultipartFormDataStreamProvider Provider { get; set; }
            public string Folder { get; set; } = "azure-blob-storage-upload";
        }

        public class AzureBlobStorageDigitalAssetResponse {
            public ICollection<DigitalAssetApiModel> DigitalAssets { get; set; } = new HashSet<DigitalAssetApiModel>();
        }

        public class AzureBlobStorageDigitalAssetHandler : IAsyncRequestHandler<AzureBlobStorageDigitalAssetRequest, AzureBlobStorageDigitalAssetResponse>
        {
            public AzureBlobStorageDigitalAssetHandler(BlogServiceContext context, ICache cache, Lazy<IAzureBlobStorageConfiguration> lazyConfiguration)
            {
                _context = context;
                _cache = cache;
                _configuration = lazyConfiguration.Value;
                _storageAccount = new CloudStorageAccount(new StorageCredentials(_configuration.AccountName,_configuration.KeyValue),true);
            }

            public async Task<AzureBlobStorageDigitalAssetResponse> Handle(AzureBlobStorageDigitalAssetRequest request)
            {
                _blobClient = _storageAccount.CreateCloudBlobClient();

                var container = _blobClient.GetContainerReference($"{request.TenantUniqueId}");

                await container.CreateIfNotExistsAsync();

                BlobContainerPermissions permissions = container.GetPermissions();

                permissions.PublicAccess = BlobContainerPublicAccessType.Blob;

                container.SetPermissions(permissions);

                var response = new AzureBlobStorageDigitalAssetResponse();

                foreach (var file in request.Provider.Files)
                {
                    var filename = new FileInfo(file.Headers.ContentDisposition.FileName.Trim(new char[] { '"' })
                        .Replace("&", "and")).Name;

                    var stream = await file.ReadAsStreamAsync();
                    
                    var blockBlob = container.GetBlockBlobReference(filename);

                    blockBlob.Properties.ContentType = System.Convert.ToString(file.Headers.ContentType);

                    await blockBlob.UploadFromStreamAsync(stream);

                    response.DigitalAssets.Add(new DigitalAssetApiModel()
                    {
                        Url = $"{blockBlob.StorageUri.PrimaryUri}"
                    });
                }
                return response;
            }

            private readonly BlogServiceContext _context;

            private readonly ICache _cache;

            private readonly CloudStorageAccount _storageAccount;

            private CloudBlobClient _blobClient;

            private readonly IAzureBlobStorageConfiguration _configuration;
        }
    }
}