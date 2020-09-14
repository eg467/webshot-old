﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Webshot
{
    public sealed class CancellableTask : IDisposable
    {
        private CancellationTokenSource _canceler;
        public bool Busy => _canceler is object;
        private readonly object _lock = new object();

        private bool Cancel()
        {
            lock (_lock)
            {
                return UnsafeCancel();
            }
        }

        private bool UnsafeCancel()
        {
            var wasBusy = Busy;
            if (wasBusy)
            {
                _canceler.Cancel();
                UnsafeDisposeCanceler();
            }
            return wasBusy;
        }

        private void UnsafeDisposeCanceler()
        {
            _canceler?.Dispose();
            _canceler = null;
        }

        private void DisposeCanceler()
        {
            lock (_lock)
            {
                UnsafeDisposeCanceler();
            }
        }

        private void CreateCanceler(int timeout = -1)
        {
            lock (_lock)
            {
                UnsafeCancel();
                _canceler = new CancellationTokenSource(timeout);
                _canceler.Token.Register(DisposeCanceler);
                UnsafeDisposeCanceler();
            }
        }

        public async Task<TResult> PerformAsync<TResult>(
            Func<CancellationToken, Task<TResult>> fn,
            int timeout = -1)
        {
            CreateCanceler(timeout);
            TResult result = await fn(_canceler.Token);
            DisposeCanceler();
            return result;
        }

        public async Task PerformAsync(Func<CancellationToken, Task> fn, int timeout = -1)
        {
            CreateCanceler(timeout);
            await fn(_canceler.Token);
            DisposeCanceler();
        }

        public void Dispose()
        {
            Cancel();
        }
    }
}