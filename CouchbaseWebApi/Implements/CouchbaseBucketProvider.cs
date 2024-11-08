using Couchbase;
using CouchbaseWebApi.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CouchbaseWebApi.Implements
{
    public class CouchbaseBucketProvider : ICouchbaseBucketProvider
    {
        private readonly ICluster _cluster;
        private readonly IConfiguration _configuration;

        // Constructor expects ICluster and IConfiguration to be injected by DI
        public CouchbaseBucketProvider(ICluster cluster, IConfiguration configuration)
        {
            _cluster = cluster;
            _configuration = configuration;
        }

        public async Task<IBucket> GetBucketAsync()
        {
            // Get the bucket name from configuration
            var bucketName = _configuration["Couchbase:BucketName"];

            // Use the ICluster (which is already connected) to get the bucket asynchronously
            var bucket = await _cluster.BucketAsync(bucketName);
            return bucket;
        }
    }

}
