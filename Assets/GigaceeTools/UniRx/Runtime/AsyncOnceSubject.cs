// Original code from https://qiita.com/toRisouP/items/1bd2f3b07bf868953178

using System;
using UniRx;

namespace GigaceeTools
{
    public class AsyncOnceSubject : IObservable<Unit>, IDisposable
    {
        private readonly AsyncSubject<Unit> _asyncSubject = new AsyncSubject<Unit>();
        private readonly object _lockObject = new object();

        public void Dispose()
        {
            lock (_lockObject)
            {
                _asyncSubject.Dispose();
            }
        }

        public IDisposable Subscribe(IObserver<Unit> observer)
        {
            lock (_lockObject)
            {
                return _asyncSubject.Subscribe(observer);
            }
        }

        public void Completed()
        {
            lock (_lockObject)
            {
                if (_asyncSubject.IsCompleted)
                {
                    return;
                }

                _asyncSubject.OnNext(Unit.Default);
                _asyncSubject.OnCompleted();
            }
        }
    }
}
