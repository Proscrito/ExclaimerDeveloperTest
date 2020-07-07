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
