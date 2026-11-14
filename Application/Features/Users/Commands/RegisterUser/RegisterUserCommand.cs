using Application.Common.Messaging;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand(string Nom, string Prenom, string Email, string MotDePasse)
    : IRequest<RegisterResult>;

public enum RegisterStatus { Ok, EmailPris, Invalide }
public sealed record RegisterResult(RegisterStatus Status, UserDto? User = null, string? Erreur = null);
