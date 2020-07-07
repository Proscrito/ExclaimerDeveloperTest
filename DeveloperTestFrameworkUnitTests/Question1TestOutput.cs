#region Copyright statement
// --------------------------------------------------------------
// Copyright (C) 1999-2016 Exclaimer Ltd. All Rights Reserved.
// No part of this source file may be copied and/or distributed 
// without the express permission of a director of Exclaimer Ltd
// ---------------------------------------------------------------
#endregion
using System;
using System.Collections;
using System.Collections.Generic;

using DeveloperTestInterfaces;

namespace DeveloperTestFramework
{
    public sealed class Question1TestOutput : IOutputResult, IEnumerable<string>
    {
        private readonly IList<string> _results = new List<string>(128);

        public IEnumerator<string> GetEnumerator() => _results.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void AddResult(string text)
        {
            Console.WriteLine(text);
            _results.Add(text);
        }

        public int Count => _results.Count;
    }
}