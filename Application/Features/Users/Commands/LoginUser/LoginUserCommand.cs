using Application.Common.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Users.Commands.LoginUser;

public sealed record LoginUserCommand(string Email, string MotDePasse) : IRequest<LoginResult>;

