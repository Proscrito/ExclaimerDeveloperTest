#region Copyright statement
// --------------------------------------------------------------
// Copyright (C) 1999-2016 Exclaimer Ltd. All Rights Reserved.
// No part of this source file may be copied and/or distributed 
// without the express permission of a director of Exclaimer Ltd
// ---------------------------------------------------------------
#endregion
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using DeveloperTestInterfaces;

namespace DeveloperTestSupport
{
    public class SlowCharacterReader : ICharacterReader
    {
        private readonly string content = @"  Alice was beginning to get very tired of sitting by her sister
on the bank, and of having nothing to do:  once or twice she had
peeped into the book her sister was reading, but it had no
pictures or conversations in it, 'and what is the use of a book,'
thought Alice `without pictures or conversation?'

  So she was considering in her own mind (as well as she could,
for the hot day made her feel very sleepy and stupid), whether
the pleasure of making a daisy-chain would be worth the trouble
of getting up and picking the daisies, when suddenly a White
Rabbit with pink eyes ran close by her.";

        readonly Random _rnd = new Random();

        private int _pos;

        public char GetNextChar()
        {
            Thread.Sleep(_rnd.Next(200));

            if (_pos >= content.Length)
            {
                throw new EndOfStreamException();
            }

            return content[_pos++];
        }

        public async Task<char> GetNextCharAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(_rnd.Next(200), cancellationToken);

            if (_pos >= content.Length)
            {
                throw new EndOfStreamException();
            }

            return content[_pos++];
        }

        public void Dispose()
        {
            //do nothing
        }
    }
}