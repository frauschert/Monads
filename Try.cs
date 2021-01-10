using System;

namespace Monads
{
    public sealed class Try<T>
    {
        public Either<Exception, T> Result { get; }
        public Try(Func<Either<Exception, T>> func)
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
    }
}
