using Application.Contracts.Infrastructure;
using Application.Models;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.HashProvider
{
    public class HashProvider : IHashProvider
    {
        private string _hashAlgorithmName;
        private HashAlgorithm _hashFunction;


        public HashProvider(IOptions<HashProviderSettings> options)
        {
            if (options.Value == null || options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _hashFunction =  HashAlgorithm.Create(options.Value.HashAlgorithmName) ?? throw new ArgumentException(nameof(options.Value.HashAlgorithmName));
            _hashAlgorithmName = options.Value.HashAlgorithmName;
            Encoding = Encoding.GetEncoding(options.Value.EncodingName);
        }

        public string HashAlgorithmName 
        { 
            get => _hashAlgorithmName; 
            set
            { 
                _hashFunction =  HashAlgorithm.Create(value) ?? throw new ArgumentException($"Hash algorithm with name {value} not found");
                _hashAlgorithmName = value;
            }
        }

        public HashAlgorithm HashFunction { get => _hashFunction; }

        public Encoding Encoding { get;  set; }
    }
}
