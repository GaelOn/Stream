using Stream;

namespace StreamPerfConsole
{
    public interface ITestStreamProvider<TIn, TOut>
    {
        Stream<TIn, TOut> LocalStreamProvider(int useCase);
    }
}