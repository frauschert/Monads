﻿using System;

namespace Monads
{
    public struct Either<L, R>
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
}
