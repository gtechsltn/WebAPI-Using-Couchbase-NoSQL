using Couchbase;
using Couchbase.Core.Exceptions.KeyValue;
using CouchbaseWebApi.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CouchbaseWebApi.Implements
{
    public class CouchbaseService
    {
        private readonly IBucket _bucket;
        private readonly IConfiguration _configuration;

        public CouchbaseService(IConfiguration configuration)
        {
            _configuration= configuration;

            var cluster = Cluster.ConnectAsync(_configuration["Couchbase:ConnectionString"],
                                                _configuration["Couchbase:Username"],
                                                _configuration["Couchbase:Password"]).Result;


            //var str = _configuration["Couchbase:BucketName"];
            _bucket = cluster.BucketAsync(_configuration["Couchbase:BucketName"]).Result;
        }

        public async Task<MyModel> GetDocumentAsync(string id)
        {
            try
            {
                var result = await _bucket.DefaultCollection().GetAsync(id);
                var storedDocument = result.ContentAs<MyModel>();

                return storedDocument;
            }
            catch (DocumentNotFoundException)
            {
                return null;
            }
        }

        // Utility method to check if a string is valid JSON
        private bool IsValidJson(string str)
        {
            str = str.Trim();
            return (str.StartsWith("{") && str.EndsWith("}")) ||  // For object
                   (str.StartsWith("[") && str.EndsWith("]"));    // For array
        }

        public async Task<MyModel> UpsertDocumentAsync(string id, MyModel document)
        {
            await _bucket.DefaultCollection().UpsertAsync(id, document);

            return document;
        }
    }
}
