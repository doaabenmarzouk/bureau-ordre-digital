using Amazon.S3;
using Amazon.S3.Model;

namespace BureauOrdreDigital.Api.Modules.Courrier.Services;

public class StockageService : IStockageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public StockageService(IConfiguration configuration)
    {
        _bucketName = configuration["Minio:BucketName"]
            ?? throw new InvalidOperationException("Minio:BucketName non configuré.");

        var config = new AmazonS3Config
        {
            ServiceURL = configuration["Minio:Endpoint"],
            ForcePathStyle = true // obligatoire pour MinIO
        };

        _s3Client = new AmazonS3Client(
            configuration["Minio:AccessKey"],
            configuration["Minio:SecretKey"],
            config);
    }

    public async Task<string> SauvegarderFichier(IFormFile fichier)
    {
        await CreerBucketSiInexistant();

        var nomUnique = $"{Guid.NewGuid()}_{fichier.FileName}";

        using var stream = fichier.OpenReadStream();

        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = nomUnique,
            InputStream = stream,
            ContentType = fichier.ContentType
        };

        await _s3Client.PutObjectAsync(request);

        return nomUnique;
    }

    private async Task CreerBucketSiInexistant()
    {
        var bucketsExistants = await _s3Client.ListBucketsAsync();
        var existe = bucketsExistants.Buckets.Any(b => b.BucketName == _bucketName);

        if (!existe)
        {
            await _s3Client.PutBucketAsync(_bucketName);
        }
    }
}