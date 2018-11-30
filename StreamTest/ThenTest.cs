using System;
using NUnit.Framework;
using Stream.Streams.Folder;
using FluentAssertions;

namespace StreamTests
{
    [TestFixture]
    public class ThenTest
    {
        public readonly Func<int, int> fi1 = (i => i + 2);
        public readonly Func<int, int> fi2 = (i => i * 2);
        public readonly Func<string, string> fs1 = (s => s + ",,test,break");
        public readonly Func<string, string[]> fs2 = (s => s.Split(',', StringSplitOptions.RemoveEmptyEntries));

        [Test]
        public void TestIntFunction() 
        {
            //given
            var input    = 5;
            var expected = 14;
            //when
            var fn = fi1.Then(fi2);
            var result = fn(input);
            //then
            result.Should().Be(expected);
        }

        [Test]
        public void TestStringFunction()
        {
            //given
            var input = "first";
            var expected = new string[] { "first", "test", "break" };
            //when
            var fn = fs1.Then(fs2);
            var result = fn(input);
            //then
            result.Length.Should().Be(expected.Length);
            for (int it = 0; it < 3; it++)
            {
                result[it].Should().Be(expected[it]);
            }
        }
    }
}