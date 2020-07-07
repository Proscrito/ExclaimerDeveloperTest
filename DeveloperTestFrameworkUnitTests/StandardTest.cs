#region Copyright statement
// --------------------------------------------------------------
// Copyright (C) 1999-2016 Exclaimer Ltd. All Rights Reserved.
// No part of this source file may be copied and/or distributed 
// without the express permission of a director of Exclaimer Ltd
// ---------------------------------------------------------------
#endregion

using DeveloperTest;

using DeveloperTestInterfaces;

using DeveloperTestSupport;

using NUnit.Framework;

namespace DeveloperTestFramework
{
    [TestFixture]
    public sealed class StandardTest : StandardTestBase<IDeveloperTest>
    {
        [Timeout(1000), Test]
        public void TestQuestionOne()
        {
            var output = new Question1TestOutput();
            using (var simpleReader = new SimpleCharacterReader())
            {
                DeveloperImplementation.RunQuestionOne(simpleReader, output);
                VerifyQuestionOne(output);
            }
        }

        [Test, Timeout(120000)]
        public void TestQuestionTwoMultiple()
        {
            var output = new Question2TestOutput();
            using (var slowReader1 = new SlowCharacterReader())
            using (var slowReader2 = new SlowCharacterReader())
            using (var slowReader3 = new SlowCharacterReader())
            {
                DeveloperImplementation.RunQuestionTwo(new ICharacterReader[] { slowReader1, slowReader2, slowReader3 }, output);
                VerifyQuestionTwoMultiple(output);
            }
        }

        [Test, Timeout(120000)]
        public void TestQuestionTwoSingle()
        {
            var output = new Question2TestOutput();
            using (var slowReader = new SlowCharacterReader())
            {
                DeveloperImplementation.RunQuestionTwo(new ICharacterReader[] { slowReader }, output);
                VerifyQuestionTwoSingle(output);
            }
        }

        protected override IDeveloperTest CreateDeveloperTest()
        {
            return new DeveloperTestImplementation();
        }
    }
}