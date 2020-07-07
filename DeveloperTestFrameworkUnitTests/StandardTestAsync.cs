using System.Threading;
using System.Threading.Tasks;

using DeveloperTest;

using DeveloperTestInterfaces;

using DeveloperTestSupport;

using NUnit.Framework;

namespace DeveloperTestFramework
{
    [TestFixture]
    public sealed class StandardTestAsync : StandardTestBase<IDeveloperTestAsync>
    {
        [Timeout(1000), Test]
        public async Task TestQuestionOneAsync()
        {
            /* this one fails with the message:

            The seventh element is not correct. Is your word break algorithm effective?
            String lengths are both 2. Strings differ at index 0.
            Expected: "in"
            But was:  "we"
            
            but it's incorrect. 'we' occurs 4 times while 'in' occurs 3 times and actually 'we' should be 7th, while 'in' should be 10th, following the order rules:

            the	18
            of	16
            was	12
            it	11
            a	8
            were	5
    7th     we	4
            with	4
            and	3
            for	3
    10th    in	3
            on	3        

            you could check it here: https://planetcalc.com/3205/ 
              
            I didn't check following tests, because I checked the whole result - it looks correct
             */

            var output = new Question1TestOutput();
            using (var simpleReader = new SimpleCharacterReader())
            {
                await DeveloperImplementation.RunQuestionOne(simpleReader, output, CancellationToken.None);
                VerifyQuestionOne(output);
            }
        }

        [Test, Timeout(220000)] // Timeout parameter value changed from 120000 to 220000 by Kostas.
        public async Task TestQuestionTwoMultipleAsync()
        {
            //passed
            var output = new Question2TestOutput();
            using (var slowReader1 = new SlowCharacterReader())
            using (var slowReader2 = new SlowCharacterReader())
            using (var slowReader3 = new SlowCharacterReader())
            {
                await DeveloperImplementation.RunQuestionTwo(new ICharacterReader[] { slowReader1, slowReader2, slowReader3 }, output, CancellationToken.None);
                VerifyQuestionTwoMultiple(output);
            }
        }

        [Test, Timeout(120000)]
        public async Task TestQuestionTwoSingleAsync()
        {
            //passed
            var output = new Question2TestOutput();
            using (var slowReader = new SlowCharacterReader())
            {
                await DeveloperImplementation.RunQuestionTwo(new ICharacterReader[] { slowReader }, output, CancellationToken.None);
                VerifyQuestionTwoSingle(output);
            }
        }

        protected override IDeveloperTestAsync CreateDeveloperTest()
        {
            return new DeveloperTestImplementationAsync();
        }
    }
}
