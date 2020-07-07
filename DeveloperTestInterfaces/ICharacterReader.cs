#region Copyright statement
// --------------------------------------------------------------
// Copyright (C) 1999-2016 Exclaimer Ltd. All Rights Reserved.
// No part of this source file may be copied and/or distributed 
// without the express permission of a director of Exclaimer Ltd
// ---------------------------------------------------------------
#endregion
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperTestInterfaces
{
    public interface ICharacterReader : IDisposable
    {
        char GetNextChar();

        Task<char> GetNextCharAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}