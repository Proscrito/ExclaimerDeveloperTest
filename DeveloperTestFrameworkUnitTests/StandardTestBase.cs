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

using NUnit.Framework;

namespace DeveloperTestFramework
{
    public abstract class StandardTestBase<T>
    {
        private readonly Lazy<T> _developerImplementation;

        protected StandardTestBase()
        {
            _developerImplementation = new Lazy<T>(CreateDeveloperTest);
        }

        protected T DeveloperImplementation => _developerImplementation.Value;

        protected abstract T CreateDeveloperTest();

        protected void VerifyQuestionOne(Question1TestOutput output)
        {
            Assert.AreNotEqual(
                85,
                output.Count,
                "No of results is not correct; should be 86. Have you dealt with the final word left after you broke out of your processing loop?");
            Assert.AreEqual(86, output.Count, "No of results is not correct; should be 86");
            VerifyResult(output, 0, "the", 18, "First element is not correct");
            VerifyResult(output, 3, "it", 11, "Check case sensitivity?");
            VerifyResult(output, 6, "in", 4, "The seventh element is not correct. Is your word break algorithm effective?");
            VerifyResult(output, 49, "fair", 1, "Fiftieth element is not correct");
            VerifyResult(output, 82, "way", 1, "Is way--in strictly a word do you think?");
            VerifyResult(output, 85, "worst", 1, "Last element is not correct");
        }

        protected void VerifyQuestionTwoMultiple(Question2TestOutput output)
        {
            string validationErrors = output.ValidationErrors;
            if (!string.IsNullOrEmpty(validationErrors))
            {
                Assert.Fail(validationErrors);
            }
            Assert.AreEqual(76, output.Count, "Result count is incorrect");
            VerifyWordCount(output, "book", 2, 3);
            VerifyWordCount(output, "and", 4, 3);
            VerifyWordCount(output, "rabbit", 1, 3);
        }

        protected void VerifyQuestionTwoSingle(Question2TestOutput output)
        {
            string validationErrors = output.ValidationErrors;
            if (!string.IsNullOrEmpty(validationErrors))
            {
                Assert.Fail(validationErrors);
            }
            Assert.AreEqual(76, output.Count, "Result count is incorrect");
            VerifyWordCount(output, "book", 2);
            VerifyWordCount(output, "and", 4);
            VerifyWordCount(output, "rabbit", 1);
        }

        private static void VerifyResult(IEnumerable<string> output, int index, string expectedWord, int expectedCount, string message)
        {
            //Get the element at the specified index
            string item = output.ElementAt(index);
            if (string.IsNullOrEmpty(item))
            {
                Assert.Fail($"Empty item at {index}");
            }
            string[] parts = item.Split('-');
            if (parts.Length != 2)
            {
                Assert.Fail($"Invalid number of parts in output '{item}'");
            }

            int actualCount;
            if (!int.TryParse(parts[1].Trim(), out actualCount))
            {
                Assert.Fail($"Unable to parse count '{parts[1]}'");
            }

            Assert.AreEqual(expectedWord, parts[0].Trim(), message);
            Assert.AreEqual(expectedCount, actualCount, message);
        }

        private static void VerifyWordCount(Question2TestOutput output, string word, int expectedCount, int factor = 1)
        {
            int factoredExpectedCount = expectedCount * factor;
            if (!output.CheckWordCount(word, factoredExpectedCount))
            {
                Assert.Fail($"The word '{word}' should appear {factoredExpectedCount} times");
            }
        }
    }
}