using System;
using System.Collections.Async;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Wallr.ImagePersistence
{
    public static class AsyncEnumerableExtensions
    {
        public static IObservable<T> ToObservable<T>(this IAsyncEnumerable<T> enumerable)
        {
            return Observable.Using(enumerable.GetAsyncEnumeratorAsync, (enumerator, t)
                => Task.FromResult(RemainingElements(enumerator, t)));
        }

        private static IObservable<T> RemainingElements<T>(this IAsyncEnumerator<T> enumerable, CancellationToken token)
        {
            return Observable.FromAsync(() => enumerable.MoveNextAsync(token))
                .Select(moveNext
                    => moveNext
                        ? Observable.Return(enumerable.Current).Concat(RemainingElements(enumerable, token))
                        : Observable.Empty<T>())
                .Concat();
        }
    }
}