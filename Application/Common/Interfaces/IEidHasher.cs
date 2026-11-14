using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Application.Common.Interfaces;

public interface IEidHasher { string Hash(string itsmeSub); }