using Stream;
using Stream.Extension;

namespace StreamPerfConsole
{
    public class StreamProvider : ITestStreamProvider<int, int[]>
    {
        public Stream<int, int[]> LocalStreamProvider(int useCase)
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
    }
}
