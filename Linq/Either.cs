using System;

namespace Monads.Linq
{
    public static partial class Either
    {
        public static Either<L, TResult> Select<L, R, TResult>(this Either<L, R> either, Func<R, TResult> selector) =>
            SelectMany<L, R, TResult>(either, right => selector(right)); 

        public static Either<L, TResult> SelectMany<L, R, TResult>(this Either<L, R> either, Func<R, Either<L, TResult>> selector) =>
            either.Match(right => selector(right), left => Either<L, TResult>.Left(left));

        public static Either<L, TResult> SelectMany<L, R, R2, TResult>(this Either<L, R> either, Func<R, Either<L, R2>> selector, Func<R, R2,TResult> resultSelector) =>
            either.Match(right => selector(right).Match(right2 => resultSelector(right, right2), left => Either<L, TResult>.Left(left)), left => Either<L, TResult>.Left(left));

        public static Either<L, R> Where<L, R>(this Either<L, R> either, Func<R, bool> predicate) =>
            either.Match(right => predicate(right) ? right : new Either<L, R>(), left => left); 
    }
}
