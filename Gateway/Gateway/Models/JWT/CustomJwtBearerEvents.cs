using Clients.AuthApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Cryptography;

namespace CustomGateway.Models.JWT
{
    public class CustomJwtBearerEvents : JwtBearerEvents
    {
        private readonly IAuthApiClient _authClientService;

        private static HashAlgorithm? _hashAlgorithm;
        private static System.Text.Encoding? _hashEncoding;


        public CustomJwtBearerEvents(IAuthApiClient authClientService)
        {
            _authClientService = authClientService;
        }

        private HashAlgorithm HashAlgorithm
        {
            get
            {
                if (_hashAlgorithm == null)
                {
                    UpdateEncodingAndHashAlgoritm();
                }

                return _hashAlgorithm ?? throw new ApplicationException($"{nameof(HashEncoding)} is null");
            }
        }

        private System.Text.Encoding HashEncoding
        {
            get
            {
                if (_hashEncoding == null)
                {
                    UpdateEncodingAndHashAlgoritm();
                }

                return _hashEncoding ?? throw new ApplicationException($"{nameof(HashEncoding)} is null");
            }
        }

        private void UpdateEncodingAndHashAlgoritm()
        {
            Console.Write("Sending request to auth client for hashDefaults. ");

            var task = _authClientService.GetHashDefaultsAsync();
            task.Wait();

            _hashAlgorithm = HashAlgorithm.Create(task.Result.HashAlgorithmName) ?? throw new ApplicationException();
            _hashEncoding = System.Text.Encoding.GetEncoding(task.Result.EncodingName);
            Console.WriteLine("Success.");
        }


        public override async Task TokenValidated(TokenValidatedContext context)
        {
            HashAlgorithm.Initialize();

            var token = context.Request.Headers.Authorization.ToString().Split(' ')[1];

            var tokenHashBytes = HashAlgorithm.ComputeHash(HashEncoding.GetBytes(token));

            var result = await _authClientService.IsTokenValidPOSTAsync(HashEncoding.GetString(tokenHashBytes));

            if (!result)
            {
                context.Fail("Token is banned by auth service");
            }

            await base.TokenValidated(context);
        }
    }
}
