using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monads.Extensions
{
    public static partial class Maybe
    {
        public static Maybe<T> SingleOrNone<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            try
            {
                return enumerable.Single(predicate);
            }
            catch (Exception)
            {
                return Maybe<T>.None;
            }
        }

        public static Maybe<int> TryParseInt(this string str) =>
            int.TryParse(str, out var result) ? result : Maybe<int>.None;

        public static IEnumerable<T> ToEnumerable<T>(this Maybe<T> maybe) =>
            maybe.Match(value => Enumerable.Repeat(value, 1), () => Enumerable.Empty<T>());
    }
}
