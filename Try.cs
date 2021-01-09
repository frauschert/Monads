using System;

namespace Monads
{
    public struct Try<T>
    {
        private readonly Lazy<T> func;
        public Try(Func<T> func)
        {
            this.func = new Lazy<T>(func);
        }

        public Either<Exception, T> Invoke()
        {
            try
            {
                return func.Value;
            }
            catch(Exception ex)
            {
                return ex;
            }
        }

        public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Exception, TResult> onError) =>
            Invoke().Match(onSuccess, onError);
    }
}
