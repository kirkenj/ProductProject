using Application.Contracts.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Cryptography;

namespace ProductAPI.JwtAuthentication
{
    public class CustomJwtBearerEvents : JwtBearerEvents
    {
        private readonly IAuthClientService _authClientService;

        private static HashAlgorithm? _hashAlgorithm;
        private static System.Text.Encoding? _hashEncoding;

        
        public CustomJwtBearerEvents(IAuthClientService authClientService)
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

            var task = _authClientService.GetHashAlgorithmName();
            task.Wait();

            if (!task.Result.Success)
            {
                throw new ApplicationException("Couldn't get hash alforithm name from auth service. " + task.Result.Message);
            }

            _hashAlgorithm = HashAlgorithm.Create(task.Result.Result.HashAlgorithmName) ?? throw new ApplicationException();
            _hashEncoding = System.Text.Encoding.GetEncoding(task.Result.Result.EncodingName);
            Console.WriteLine("Success.");
        }


        public override async Task TokenValidated(TokenValidatedContext context)
        {
            HashAlgorithm.Initialize();

            var token = context.Request.Headers.Authorization.ToString().Split(' ')[1];

            var tokenHashBytes = HashAlgorithm.ComputeHash(HashEncoding.GetBytes(token));

            var result = await _authClientService.IsTokenValid(HashEncoding.GetString(tokenHashBytes));

            if (!result.Success)
            {
                throw new ApplicationException("Couldn't verify the authorization token");
            }

            if (!result.Result)
            {
                context.Fail("Token is banned by auth service");
            }

            await base.TokenValidated(context);
        }
    }
}
