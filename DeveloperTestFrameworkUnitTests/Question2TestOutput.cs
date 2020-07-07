#region Copyright statement
// --------------------------------------------------------------
// Copyright (C) 1999-2016 Exclaimer Ltd. All Rights Reserved.
// No part of this source file may be copied and/or distributed 
// without the express permission of a director of Exclaimer Ltd
// ---------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;

using DeveloperTestInterfaces;

namespace DeveloperTestFramework
{
    public sealed class Question2TestOutput : IOutputResult
    {
        private static readonly HashSet<string> ExpectedWords = CreateExpectedWordsSet();
        private readonly object _padlock = new object();
        private readonly IDictionary<string, int> _results = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
        private readonly IList<string> _validationErrors = new List<string>();
        private bool _hasResults;
        private TimeSpan _lastWriteTime;

        public Question2TestOutput()
        {
            _lastWriteTime = new TimeSpan(Environment.TickCount);
        }

        public int Count
        {
            get
            {
                lock (_padlock)
                {
                    return _results.Count;
                }
            }
        }

        public bool HasResults => _hasResults;

        public string ValidationErrors
        {
            get
            {
                lock (_padlock)
                {
                    string result = string.Empty;
                    if (_validationErrors.Count > 0)
                    {
                        result = string.Join(Environment.NewLine, _validationErrors.ToArray());
                    }
                    return result;
                }
            }
        }

        public bool CheckWordCount(string word, int expectedCount)
        {
            lock (_padlock)
            {
                int actualCount;
                return _results.TryGetValue(word, out actualCount) && actualCount == expectedCount;
            }
        }

        private static HashSet<string> CreateExpectedWordsSet()
        {
            return new HashSet<string>(StringComparer.CurrentCultureIgnoreCase)
            {
                "the",
                "her",
                "of",
                "and",
                "a",
                "or",
                "she",
                "was",
                "alice",
                "as",
                "book",
                "by",
                "had",
                "in",
                "it",
                "pictures",
                "sister",
                "to",
                "very",
                "bank",
                "be",
                "beginning",
                "but",
                "close",
                "considering",
                "conversation",
                "conversations",
                "could",
                "daisies",
                "daisy-chain",
                "day",
                "do",
                "eyes",
                "feel",
                "for",
                "get",
                "getting",
                "having",
                "hot",
                "into",
                "is",
                "made",
                "making",
                "mind",
                "no",
                "nothing",
                "on",
                "once",
                "own",
                "peeped",
                "picking",
                "pink",
                "pleasure",
                "rabbit",
                "ran",
                "reading",
                "sitting",
                "sleepy",
                "so",
                "stupid",
                "suddenly",
                "thought",
                "tired",
                "trouble",
                "twice",
                "up",
                "use",
                "well",
                "what",
                "when",
                "whether",
                "white",
                "with",
                "without",
                "worth",
                "would"
            };
        }

        private void AddError(string message)
        {
            lock (_padlock)
            {
                AddErrorUnsafe(message);
            }
        }

        private void AddErrorUnsafe(string message)
        {
            _validationErrors.Add(message);
        }

        private void CheckLastWriteTime()
        {
            const int timerDuration = 10;

            //Allow for some margin 
            const double maxDifference = timerDuration * 1.25;

            lock (_padlock)
            {
                var currentTime = new TimeSpan(Environment.TickCount);
                TimeSpan difference = currentTime - _lastWriteTime;
                if (difference.TotalSeconds > maxDifference)
                {
                    AddErrorUnsafe("Timer should fire every ten seconds");
                }
                _lastWriteTime = currentTime;
            }
        }

        private sealed class WordCount
        {
            private WordCount(string word, int count)
            {
                Word = word;
                Count = count;
            }

            public int Count { get; }

            public string Word { get; }

            public static bool TryParse(string value, out WordCount wordCount)
            {
                bool result = false;
                wordCount = null;
                int separatorIndex = value.LastIndexOf('-');
                if (separatorIndex != -1)
                {
                    string[] parts = { value.Substring(0, separatorIndex), value.Substring(separatorIndex + 1) };
                    if (parts.Length == 2)
                    {
                        int actualCount;
                        if (int.TryParse(parts[1].Trim(), out actualCount) && actualCount > 0)
                        {
                            string word = parts[0].Trim();
                            wordCount = new WordCount(word, actualCount);
                            result = true;
                        }
                    }
                }
                return result;
            }
        }

        public void AddResult(string text)
        {
            _hasResults = true;
            Console.WriteLine(text);
            CheckLastWriteTime();
            WordCount wordCount;
            if (WordCount.TryParse(text, out wordCount))
            {
                if (!ExpectedWords.Contains(wordCount.Word))
                    AddError($"Unexpected word: {wordCount.Word}");

                lock (_padlock)
                {
                    _results[wordCount.Word] = wordCount.Count;
                }
            }
            else
            {
                AddError($"The value '{text}' is not in the expected format");
            }
        }
    }
}