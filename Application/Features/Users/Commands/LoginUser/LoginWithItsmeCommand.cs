using Application.Common.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Users.Commands.LoginUser;

public sealed record LoginWithItsmeCommand(string Code) : IRequest<LoginResult>;

