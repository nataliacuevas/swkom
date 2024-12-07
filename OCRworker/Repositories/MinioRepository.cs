﻿
using Minio;


namespace OCRworker.Repositories
{
    public class MinioRepository : IMinioRepository
    {
        private readonly IMinioClient _minioClient;
        private const string BucketName = "uploads";
        public MinioRepository()
        {
            _minioClient = new MinioClient()
                .WithEndpoint("minio", 9000)
                .WithCredentials("minioadmin", "minioadmin")
                .WithSSL(false)
                .Build();
        }

        // Testable constructor that accepts an IMinioClient
        public MinioRepository(IMinioClient minioClient)
        {
            _minioClient = minioClient;
        }

        public async Task<MemoryStream> Get(string fileName)
        {
            var memoryStream = new MemoryStream();

            await _minioClient.GetObjectAsync(new GetObjectArgs()
                .WithBucket(BucketName)
                .WithObject(fileName)
                .WithCallbackStream(stream =>
                {
                    stream.CopyTo(memoryStream);
                }));

            memoryStream.Position = 0;
            return memoryStream;
           // return File(memoryStream, "application/octet-stream", fileName);
        }

    }
}
