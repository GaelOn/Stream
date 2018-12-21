using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Stream;
using Stream.Extension;
using System.Text;

namespace StreamTests
{
    //TODO : implement day ten of advent of code as test
    [TestFixture]
    public class StreamTest {
        [Test]
        public void Should_Output_Stream_With_Same_Data_When_Map_Identity () 
        {
            //given
            var expected = new int[] { 1, 2, 3, 4, 5 };
            var stream = expected.AsStream ();
            //when 
            var mapper = stream.Map (x => x);
            mapper.Start ();
            var result = mapper.Result ();
            //then
            result.Length.Should ().Be (expected.Length);
            for (int it = 0; it < result.Length; it++) {
                result[it].Should ().Be (expected[it]);
            }
        }

        [Test]
        public void Should_Output_Int_Stream_With_Result_Of_Applying_Function_When_Map_Function () 
        {
            //given
            var input = new int[] { 1, 2, 3, 4, 5 };
            var stream = input.AsStream ();
            var expected = input.Select (x => 131 * x % 7).ToArray ();
            //when 
            var mapper = stream.Map (x => 131 * x % 7);
            mapper.Start ();
            var result = mapper.Result ();
            //then
            result.Length.Should ().Be (expected.Length);
            for (int it = 0; it < result.Length; it++) {
                result[it].Should ().Be (expected[it]);
            }
        }

        [Test]
        public void Should_Output_Sum_Of_Int_Array () 
        {
            //given
            var input = new int[] { 1, 2, 3, 4, 5 };
            var stream = input.AsStream ();
            var expected = input.Sum ();
            //when 
            var folder = stream.Fold ((x, acc) => x + acc, 0);
            folder.Start ();
            var result = folder.Result ();
            //then
            result.Should ().Be (expected);
        }

        [Test]
        public void Should_Output_Bool_Stream_With_Result_Of_Applying_String_Function_When_Map_Function () 
        {
            //given
            var input = new string[] { "test", "success", "testifying", "push", "open" };
            var stream = input.AsStream ();
            var expected = input.Select (x => x.Contains ("test")).ToArray ();
            //when 
            var mapper = stream.Map (x => x.Contains ("test"));
            mapper.Start ();
            var result = mapper.Result ();
            //then
            result.Length.Should ().Be (expected.Length);
            for (int it = 0; it < result.Length; it++) {
                result[it].Should ().Be (expected[it]);
            }
        }

        [Test]
        public void Should_String_Stream_With_Data_Filter_On ()
        {
            //given
            string[] input = StringArrProvider();
            var stream = input.AsStream();
            var expected = input.Where(x => x.Contains("test")).ToArray();
            //when 
            var mapper = stream.Filter(x => x.Contains("test"));
            mapper.Start();
            var result = mapper.Result();
            //then
            result.Length.Should().Be(expected.Length);
            for (int it = 0; it < result.Length; it++)
            {
                result[it].Should().Be(expected[it]);
            }
        }

        [Test]
        public void Should_String_Stream_With_Data_Mapped_Then_Filter_On () 
        {
            //given
            var input = StringArrProvider();
            var stream = input.AsStream ();
            var expected = input.Select (x => new Tuple<string, int> (x, x.Length))
                .Where (x => x.Item1.Contains ("test") && x.Item2 > 5)
                .ToArray ();
            //when 
            var mapper = stream.Map (x => new Tuple<string, int> (x, x.Length))
                .Filter (x => x.Item1.Contains ("test") && x.Item2 > 5);
            mapper.Start ();
            var result = mapper.Result ();
            //then
            result.Length.Should ().Be (expected.Length);
            for (int it = 0; it < result.Length; it++) {
                result[it].Should ().Be (expected[it]);
            }
        }

        [Test]
        public void PerfTest ()
        {
            // this test is only meaningfull in release mode due to optimization provide by the compilation
#if !DEBUG
            var tab = IntArrayProvider(1000000);

            var sw = new System.Diagnostics.Stopwatch();

            sw.Start();
            var expectedResult = tab.Select(x => x * 7)
                                    .Where(x => x % 2 == 0)
                                    .Average();
            sw.Stop();
            var maxExpectedTime = sw.ElapsedTicks;

            var nbElem = tab.Select(x => x * 7)
                            .Count(x => x % 2 == 0);

            sw.Reset();

            var stream = tab.AsStream()
                            .Map(x => x * 7)
                            .Filter(x => x % 2 == 0)
                            .Average(x => x);

            sw.Start();
            var result = stream.Result();
            sw.Stop();
            var time = sw.ElapsedTicks;

            (Math.Abs(result - expectedResult)).Should().BeApproximately(0, 0.001);
            time.Should().BeLessThan(maxExpectedTime);
#endif
        }

        [Test]
        public void Should_sum_all_element_from_Distinct_Stream () 
        {
            // GIVEN
            var args = new int[] { 0, 1, 2 };
            var startStream = args.AsStream ();
            var expected = 45;
            // WHEN
            var sumStream = startStream.FlatMap(StreamProvider)
                                        .Sum (x => x);
            var result = sumStream.Result ();
            //THEN
            result.Should().Be(expected);
        }

        [Test]
        public void Should_sum_all_element_with_Iter_method()
        {
            // GIVEN
            var start = 0;
            Action<int> iterableAction = (x => start += x);
            var args = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
            var startStream = args.AsStream();
            var expected = 45;
            // WHEN
            var EOS = startStream.Iter(iterableAction);
            EOS.Start();
            //THEN
            start.Should().Be(expected);
        }

        [Test]
        public void Should_Accumulate_Result_Into_List()
        {
            // GIVEN
            var intArr = IntArrayProvider(1000000);
            var stream = intArr.AsStream();
            bool filter(long x) => x % 3 == 0;
            // WHEN
            var res = stream.Filter(filter)
                            .Collect(() => new List<long>(500),
                                     elem => elem,
                                     (elem, acc) => acc.Add(elem))
                            .Result();
            var expected = res.Where(filter).ToList();
            //THEN
            res.Count.Should().Be(expected.Count);
            for (var it = 0; it < res.Count; it++)
            {
                res[it].Should().Be(expected[it]);
            }
        }

        [Test]
        public void Should_Accumulate_Result_Into_String()
        {
            // GIVEN
            var stringArr = StringArrProvider();
            var stream = stringArr.AsStream();
            string transform(string x) => x.Reverse().Aggregate("", (acc, elem) => acc + elem);
            void stringAppender(string toBeAggregated, StringBuilder acc) => acc.Append(toBeAggregated);
            // WHEN
            var res = stream.Collect(() => new StringBuilder("|"),
                                     elem => " " + (transform(elem) + " |"),
                                     stringAppender)
                            .Result()
                            .ToString();
            var expected = "| " + String.Join(" | ", stringArr.Select(transform)) + " |";
            //THEN
            res.Should().Be(expected);
        }

        private Stream<int,int[]> StreamProvider (int useCase) 
        {
            switch (useCase) 
            {
                case 0:
                    return (new int[] { 1, 2, 3 }).AsStream();
                case 1:
                    return (new int[] { 4, 5, 6 }).AsStream();
                default:
                    return (new int[] { 7, 8, 9 }).AsStream();
            }
        }

        private long[] IntArrayProvider(int size)
        {
            var tab = new long[size];
            for (long it = 0; it < size; it++)
            {
                tab[it] = it;
            }

            return tab;
        }

        private static string[] StringArrProvider()
        {
            return new string[] { "test", "success", "testifying", "push", "open" };
        }
    }
}