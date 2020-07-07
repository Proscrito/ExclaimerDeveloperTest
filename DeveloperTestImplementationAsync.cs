#region Copyright statement
// --------------------------------------------------------------
// Copyright (C) 1999-2016 Exclaimer Ltd. All Rights Reserved.
// No part of this source file may be copied and/or distributed 
// without the express permission of a director of Exclaimer Ltd
// ---------------------------------------------------------------
#endregion
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using DeveloperTestInterfaces;

namespace DeveloperTest
{
    public sealed class DeveloperTestImplementationAsync : IDeveloperTestAsync
    {
        private readonly ConcurrentStack<string> _buffer = new ConcurrentStack<string>();
        private Dictionary<string, int> _output = new Dictionary<string, int>();
        private int _waitCount;

        public async Task RunQuestionOne(ICharacterReader reader, IOutputResult output, CancellationToken cancellationToken)
        {
            await QuestionTask(reader, cancellationToken);
            Calculate(output);
            _waitCount = 0; //clear
        }

        public async Task RunQuestionTwo(ICharacterReader[] readers, IOutputResult output, CancellationToken cancellationToken)
        {
            var tasks = readers.Select(x => QuestionTask(x, cancellationToken)).ToList();
            tasks.Add(GetFlushTask(TimeSpan.FromSeconds(10), output, readers.Length));

            await Task.WhenAll(tasks);
        }

        private async Task GetFlushTask(TimeSpan flushPeriod, IOutputResult output, int threadToWait)
        {
            do
            {
                await Task.Delay(flushPeriod);
                Calculate(output);
                Debug.Write("flush");
            } while (threadToWait > _waitCount);

            _waitCount = 0;
        }

        private async Task QuestionTask(ICharacterReader reader, CancellationToken cancellationToken)
        {
            var word = "";
            var notEmpty = true;

            do
            {
                try
                {
                    var nextChar = await reader.GetNextCharAsync(cancellationToken);
                    word = Append(nextChar, word);
                }
                catch (EndOfStreamException)
                {
                    notEmpty = false;
                    //funny stuff, let's put the forgotten point 
                    Append('.', word);
                }

            } while (notEmpty);

            Interlocked.Increment(ref _waitCount);
        }

        private void Calculate(IOutputResult output)
        {
            var buffer = new string[_buffer.Count];
            var count = _buffer.TryPopRange(buffer);
            Debug.Write($"Got: {count} strings");

            //TODO: it makes no sense to flush the output each 10 sec while we still need to keep & update the whole result.
            var chunk = buffer.GroupBy(x => x).Select(g => new KeyValuePair<string, int>(g.Key, g.Count()));
            _output = _output.Concat(chunk).GroupBy(x => x.Key).ToDictionary(c => c.Key, c => c.Sum(x => x.Value));
            var input = _output.OrderByDescending(x => x.Value).ThenBy(x => x.Key).Select(g => $"{g.Key} - {g.Value}");

            foreach (var line in input)
            {
                Debug.Write($"Output: {line}");
                output.AddResult(line);
            }
        }

        private string Append(char nextChar, string word)
        {
            if (char.IsLetterOrDigit(nextChar) || nextChar == '-')
            {
                word += nextChar;
            }
            else
            {
                if (!string.IsNullOrEmpty(word))
                {
                    _buffer.Push(word.ToLower());
                    Debug.Write($"New word: {word.ToLower()}");
                }

                word = "";
            }

            return word;
        }
    }
}