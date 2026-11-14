using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces;

public interface IPasswordHasher
{
    string Hash(string motDePasse);
    PasswordCheck Verify(string hash, string motDePasse);
}
public enum PasswordCheck { Echec, Ok, OkRehash }