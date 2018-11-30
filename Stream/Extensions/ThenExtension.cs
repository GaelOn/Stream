using System;
namespace Stream.Streams.Folder
{
    public static class ThenExtension
    {
        // Func part
        public static Func<TIn, TOut> Then<TIn, TInterm, TOut>(
            this Func<TIn, TInterm> fn1, Func<TInterm, TOut> fn2)
        {
            return (argin => fn2(fn1(argin)));
        }

        public static Func<TIn1, TIn2, TOut> Then<TIn1, TIn2, TInterm, TOut>(
            this Func<TIn1, TIn2, TInterm> fn1, Func<TInterm, TOut> fn2)
        {
            return ((argin1, argin2) => fn2(fn1(argin1, argin2)));
        }

        public static Func<TIn1, TIn2, TIn3, TOut> Then<TIn1, TIn2, TIn3, TInterm, TOut>(
            this Func<TIn1, TIn2, TIn3, TInterm> fn1, Func<TInterm, TOut> fn2)
        {
            return ((argin1, argin2, argin3) => fn2(fn1(argin1, argin2, argin3)));
        }

        public static Func<TIn1, TIn2, TIn3, TIn4, TOut> Then<TIn1, TIn2, TIn3, TIn4, TInterm, TOut>(
            this Func<TIn1, TIn2, TIn3, TIn4, TInterm> fn1, Func<TInterm, TOut> fn2)
        {
            return ((argin1, argin2, argin3, argin4) => fn2(fn1(argin1, argin2, argin3, argin4)));
        }

        public static Func<TIn1, TIn2, TIn3, TIn4, TIn5, TOut> Then<TIn1, TIn2, TIn3, TIn4, TIn5, TInterm, TOut>(
            this Func<TIn1, TIn2, TIn3, TIn4, TIn5, TInterm> fn1, Func<TInterm, TOut> fn2)
        {
            return ((argin1, argin2, argin3, argin4, argin5) => fn2(fn1(argin1, argin2, argin3, argin4, argin5)));
        }

        public static Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TOut> Then<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TInterm, TOut>(
            this Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TInterm> fn1, Func<TInterm, TOut> fn2)
        {
            return ((argin1, argin2, argin3, argin4, argin5, argin6) => fn2(fn1(argin1, argin2, argin3, argin4, argin5, argin6)));
        }

        public static Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TOut> Then<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TInterm, TOut>(
            this Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TInterm> fn1, Func<TInterm, TOut> fn2)
        {
            return ((argin1, argin2, argin3, argin4, argin5, argin6, argin7) => fn2(fn1(argin1, argin2, argin3, argin4, argin5, argin6, argin7)));
        }

        public static Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TOut> Then<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TInterm, TOut>(
            this Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TInterm> fn1, Func<TInterm, TOut> fn2)
        {
            return ((argin1, argin2, argin3, argin4, argin5, argin6, argin7, argin8) => fn2(fn1(argin1, argin2, argin3, argin4, argin5, argin6, argin7, argin8)));
        }

        // Action part
        public static Action<TIn> Then<TIn, TInterm>(
            this Func<TIn, TInterm> fn1, Action<TInterm> fn2)
        {
            return (argin => fn2(fn1(argin)));
        }

        public static Action<TIn1, TIn2> Then<TIn1, TIn2, TInterm>(
            this Func<TIn1, TIn2, TInterm> fn1, Action<TInterm> fn2)
        {
            return ((argin1, argin2) => fn2(fn1(argin1, argin2)));
        }

        public static Action<TIn1, TIn2, TIn3> Then<TIn1, TIn2, TIn3, TInterm>(
            this Func<TIn1, TIn2, TIn3, TInterm> fn1, Action<TInterm> fn2)
        {
            return ((argin1, argin2, argin3) => fn2(fn1(argin1, argin2, argin3)));
        }

        public static Action<TIn1, TIn2, TIn3, TIn4> Then<TIn1, TIn2, TIn3, TIn4, TInterm>(
            this Func<TIn1, TIn2, TIn3, TIn4, TInterm> fn1, Action<TInterm> fn2)
        {
            return ((argin1, argin2, argin3, argin4) => fn2(fn1(argin1, argin2, argin3, argin4)));
        }

        public static Action<TIn1, TIn2, TIn3, TIn4, TIn5> Then<TIn1, TIn2, TIn3, TIn4, TIn5, TInterm>(
            this Func<TIn1, TIn2, TIn3, TIn4, TIn5, TInterm> fn1, Action<TInterm> fn2)
        {
            return ((argin1, argin2, argin3, argin4, argin5) => fn2(fn1(argin1, argin2, argin3, argin4, argin5)));
        }

        public static Action<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6> Then<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TInterm>(
            this Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TInterm> fn1, Action<TInterm> fn2)
        {
            return ((argin1, argin2, argin3, argin4, argin5, argin6) => fn2(fn1(argin1, argin2, argin3, argin4, argin5, argin6)));
        }

        public static Action<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7> Then<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TInterm>(
            this Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TInterm> fn1, Action<TInterm> fn2)
        {
            return ((argin1, argin2, argin3, argin4, argin5, argin6, argin7) => fn2(fn1(argin1, argin2, argin3, argin4, argin5, argin6, argin7)));
        }

        public static Action<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8> Then<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TInterm>(
            this Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TInterm> fn1, Action<TInterm> fn2)
        {
            return ((argin1, argin2, argin3, argin4, argin5, argin6, argin7, argin8) => fn2(fn1(argin1, argin2, argin3, argin4, argin5, argin6, argin7, argin8)));
        }
    }
}
