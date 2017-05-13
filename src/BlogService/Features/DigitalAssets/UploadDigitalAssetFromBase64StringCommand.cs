using BlogService.Data;
using BlogService.Features.Core;
using MediatR;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BlogService.Features.DigitalAssets
{
    public class UploadDigitalAssetFromBase64StringCommand
    {
        public class UploadDigitalAssetFromBase64StringRequest : IRequest<UploadDigitalAssetFromBase64StringResponse>
        {
            public Guid TenantUniqueId { get; set; }
            public string Base64String { get; set; }
        }

        public class UploadDigitalAssetFromBase64StringResponse
        {
            public string Filename { get; set; }
        }

        public class UploadDigitalAssetFromBase64StringHandler : IAsyncRequestHandler<UploadDigitalAssetFromBase64StringRequest, UploadDigitalAssetFromBase64StringResponse>
        {
            public UploadDigitalAssetFromBase64StringHandler(BlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<UploadDigitalAssetFromBase64StringResponse> Handle(UploadDigitalAssetFromBase64StringRequest request)
            {
                _blobClient = _storageAccount.CreateCloudBlobClient();

                var container = _blobClient.GetContainerReference($"{request.TenantUniqueId}");

                await container.CreateIfNotExistsAsync();

                BlobContainerPermissions permissions = container.GetPermissions();

                permissions.PublicAccess = BlobContainerPublicAccessType.Blob;

                container.SetPermissions(permissions);
                
                byte[] data = System.Convert.FromBase64String(request.Base64String);

                var stream = new MemoryStream(data);

                System.Drawing.Image image = System.Drawing.Image.FromStream(stream);

                var mimeType = image.GetMimeType();

                var filename = $"{Guid.NewGuid()}.{GetFileExtensionBasedOnMimeType(mimeType)}";

                var blockBlob = container.GetBlockBlobReference(filename);

                blockBlob.Properties.ContentType = System.Convert.ToString(mimeType);

                await blockBlob.UploadFromStreamAsync(stream);

                return new UploadDigitalAssetFromBase64StringResponse()
                {
                    Filename = $"{blockBlob.StorageUri.PrimaryUri}"
                };
            }

            private string GetFileExtensionBasedOnMimeType(string mimeType) {

                if (mimeType == "image/jpeg")
                    return "jpg";

                if (mimeType == "image/gif")
                    return "gif";

                if (mimeType == "image/png")
                    return "png";

                throw new NotSupportedException();
            }

            private readonly BlogServiceContext _context;

            private readonly ICache _cache;

            private readonly CloudStorageAccount _storageAccount;

            private CloudBlobClient _blobClient;

            private readonly IAzureBlobStorageConfiguration _configuration;
        }
    }
}
