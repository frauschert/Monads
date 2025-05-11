using System;

namespace Monads
{
    public class Either<L, R>
    {
        private readonly L left;
        private readonly R right;
        private readonly bool isRight;

        private Either(L left)
        {
            this.left = left;
            this.right = default;
            isRight = false;
        }

        private Either(R right)
        {
            this.left = default;
            this.right = right;
            isRight = true;
        }

        public static Either<L, R> Left(L value) => value;
        public static Either<L, R> Right(R value) => value;

        public static implicit operator Either<L, R>(L value) => new Either<L, R>(value);
        public static implicit operator Either<L, R>(R value) => new Either<L, R>(value);

        public TResult Match<TResult>(Func<R, TResult> right, Func<L, TResult> left) =>
            isRight ? right(this.right) : left(this.left);
    }

    public static partial class Either
    {
        public static Either<L, TResult> Select<L, R, TResult>(
            this Either<L, R> either, 
            Func<R, TResult> selector) =>
                SelectMany<L, R, TResult>(either, right => selector(right)); 

        public static Either<L, TResult> SelectMany<L, R, TResult>(
            this Either<L, R> either, 
            Func<R, Either<L, TResult>> selector) =>
                either.Match(right => selector(right), left => left);

        public static Either<L, TResult> SelectMany<L, R, R2, TResult>(
            this Either<L, R> either, 
            Func<R, Either<L, R2>> selector, 
            Func<R, R2, TResult> resultSelector) =>
                either.Match(right => selector(right).Match(right2 => resultSelector(right, right2), left => Either<L, TResult>.Left(left)), left => Either<L, TResult>.Left(left));
    }
}
