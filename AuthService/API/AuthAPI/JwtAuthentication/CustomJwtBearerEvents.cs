//using Infrastructure.TockenTractker;
//using Microsoft.AspNetCore.Authentication.JwtBearer;

//namespace AuthAPI.JwtAuthentication
//{
//    public class CustomJwtBearerEvents : JwtBearerEvents
//    {
//        private readonly TokenTracker<Guid> _tracker;

//        public CustomJwtBearerEvents(TokenTracker<Guid> tracker)
//        {
//            _tracker = tracker;
//        }

//        public override async Task TokenValidated(TokenValidatedContext context)
//        {
//            var token = context.Request.Headers.Authorization.ToString().Split(' ')[1];

//            if (_tracker.IsValid(_tracker.HashProvider.GetHash(token)) == false)
//            {
//                context.Fail("401");
//            }

//            await base.TokenValidated(context);
//        }
//    }
//}
