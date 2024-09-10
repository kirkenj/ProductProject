﻿using Application.Contracts.Persistence;
using Application.DTOs.User;
using Application.Features.User.Requests.Queries;
using AutoMapper;
using CustomResponse;
using MediatR;

namespace Application.Features.User.Handlers.Queries
{
    public class GetUserListHandler : IRequestHandler<GetUserListRequest, Response<List<UserListDto>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserListHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Response<List<UserListDto>>> Handle(GetUserListRequest request, CancellationToken cancellationToken)
        {
            IReadOnlyCollection<Domain.Models.User> users = await _userRepository.GetPageContent(request.UserFilter, request.Page, request.PageSize);
            return Response<List<UserListDto>>.OkResponse(_mapper.Map<List<UserListDto>>(users), "Success");
        }
    }
}