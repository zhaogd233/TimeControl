using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
    public static partial class UniTaskAsyncEnumerable
    {
        public static IUniTaskAsyncEnumerable<TValue> Return<TValue>(TValue value)
        {
            return new Return<TValue>(value);
        }
    }

    internal class Return<TValue> : IUniTaskAsyncEnumerable<TValue>
    {
        private readonly TValue value;

        public Return(TValue value)
        {
            this.value = value;
        }

        public IUniTaskAsyncEnumerator<TValue> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new _Return(value, cancellationToken);
        }

        private class _Return : IUniTaskAsyncEnumerator<TValue>
        {
            private readonly CancellationToken cancellationToken;
            private bool called;

            public _Return(TValue value, CancellationToken cancellationToken)
            {
                Current = value;
                this.cancellationToken = cancellationToken;
                called = false;
            }

            public TValue Current { get; }

            public UniTask<bool> MoveNextAsync()
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (!called)
                {
                    called = true;
                    return CompletedTasks.True;
                }

                return CompletedTasks.False;
            }

            public UniTask DisposeAsync()
            {
                return default;
            }
        }
    }
}