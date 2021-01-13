using System;

namespace Monads
{
    public sealed class Try<T>
    {
        public Either<Exception, T> Result { get; }
        public Try(Func<T> func)
        {
            _ = func ?? throw new ArgumentNullException(nameof(func));

            try
            {
                Result = func();
            }
            catch(Exception ex)
            {
                Result = ex;
            }
        }

        public static implicit operator Try<T>(T value) => new Try<T>(() => value);
    }
}
