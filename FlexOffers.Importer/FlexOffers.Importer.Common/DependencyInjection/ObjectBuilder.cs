using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection.Emit;

namespace FlexOffers.Importer.Common.DependencyInjection
{
    /// <summary>
    /// Builds an object dynamically, using a compiled delegates. This method has better performance than invoking the <see cref="Activator" />
    /// class.
    /// </summary>
    public static class ObjectBuilder
    {
        private delegate object Functor();
        private delegate object Functor<T1>(T1 arg1);
        private delegate object Functor<T1, T2>(T1 arg1, T2 arg2);
        private delegate object Functor<T1, T2, T3>(T1 arg1, T2 args, T3 arg3);
        private delegate object Functor<T1, T2, T3, T4>(T1 arg1, T2 args, T3 arg3, T4 arg4);
        private delegate object Functor<T1, T2, T3, T4, T5>(T1 arg1, T2 args, T3 arg3, T4 arg4, T5 arg5);

        private static Dictionary<string, object> DelegateDictionary = new Dictionary<string, object>();
        private static Dictionary<Type, Dictionary<Type, string>> KeyDictionary = new Dictionary<Type, Dictionary<Type, string>>();

        private static object SyncLock = new object();

        /// <summary>
        /// Builds an instance of an type with a parameter-less constructor.
        /// </summary>
        /// <typeparam name="T">The type of the object to build.</typeparam>
        /// <returns>An instance of the object.</returns>
        public static T Build<T>()
        {
            return (T)Build(typeof(T));
        }
        /// <summary>
        /// Builds an instance of an type with a constructor accepting one argument.
        /// </summary>
        /// <typeparam name="T">The type of the object to build.</typeparam>
        /// <typeparam name="T1">The type of the first and only argument.</typeparam>
        /// <param name="arg">The first and only argument.</param>
        /// <returns>An instance of the object.</returns>
        public static T Build<T, T1>(T1 arg)
        {
            return (T)Build<T1>(typeof(T), arg);
        }
        /// <summary>
        /// Builds an instance of an type with a constructor accepting two arguments
        /// </summary>
        /// <typeparam name="T">The type of the object to build.</typeparam>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <param name="arg1">The first argument value.</param>
        /// <param name="arg2">The second argument value.</param>
        /// <returns>An instance of the object.</returns>
        public static T Build<T, T1, T2>(T1 arg1, T2 arg2)
        {
            return (T)Build<T1, T2>(typeof(T), arg1, arg2);
        }
        /// <summary>
        /// Builds an instance of an type with a constructor accepting three arguments.
        /// </summary>
        /// <typeparam name="T">The type of the object to build.</typeparam>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <param name="arg1">The first argument value.</param>
        /// <param name="arg2">The second argument value.</param>
        /// <param name="arg3">The third argument value.</param>
        /// <returns>An instance of the object.</returns>
        public static T Build<T, T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
        {
            return (T)Build<T1, T2, T3>(typeof(T), arg1, arg2, arg3);
        }
        /// <summary>
        /// Builds an instance of an type with a constructor accepting four arguments.
        /// </summary>
        /// <typeparam name="T">The type of the object to build.</typeparam>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <param name="arg1">The first argument value.</param>
        /// <param name="arg2">The second argument value.</param>
        /// <param name="arg3">The third argument value.</param>
        /// <param name="arg4">The fourth argument value.</param>
        /// <returns>An instance of the object.</returns>
        public static T Build<T, T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return (T)Build<T1, T2, T3, T4>(typeof(T), arg1, arg2, arg3, arg4);
        }
        /// <summary>
        /// Builds an instance of an type with a constructor accepting five arguments.
        /// </summary>
        /// <typeparam name="T">The type of the object to build.</typeparam>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <param name="arg1">The first argument value.</param>
        /// <param name="arg2">The second argument value.</param>
        /// <param name="arg3">The third argument value.</param>
        /// <param name="arg4">The fourth argument value.</param>
        /// <param name="arg5">The fifth argument value.</param>
        /// <returns>An instance of the object.</returns>
        public static T Build<T, T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return (T)Build<T1, T2, T3, T4, T5>(typeof(T), arg1, arg2, arg3, arg4, arg5);
        }
        /// <summary>
        /// Builds an instance of an type with a parameter-less constructor.
        /// </summary>
        /// <param name="type">The type of the object to build.</param>
        /// <returns>An instance of the object.</returns>
        public static object Build(Type type)
        {
            return CreateDelegate<Functor>(type)();
        }
        /// <summary>
        /// Builds an instance of an type with a constructor accepting a single parameter.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <param name="type">The type of the object to build.</param>
        /// <param name="arg">The value of the argument.</param>
        /// <returns>An instance of the object.</returns>
        public static object Build<T1>(Type type, T1 arg)
        {
            return CreateDelegate<Functor<T1>>(type)(arg);
        }
        /// <summary>
        /// Builds an instance of an type with a constructor accepting two parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <param name="type">The type of the object to build.</param>
        /// <param name="arg1">The value of the first argument.</param>
        /// <param name="arg2">The value of the second argument.</param>
        /// <returns>An instance of the object.</returns>
        public static object Build<T1, T2>(Type type, T1 arg1, T2 arg2)
        {
            return CreateDelegate<Functor<T1, T2>>(type)(arg1, arg2);
        }
        /// <summary>
        /// Builds an instance of an type with a constructor accepting three parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <param name="type">The type of the object to build.</param>
        /// <param name="arg1">The value of the first argument.</param>
        /// <param name="arg2">The value of the second argument.</param>
        /// <param name="arg3">The value of the third argument.</param>
        /// <returns>An instance of the object.</returns>
        public static object Build<T1, T2, T3>(Type type, T1 arg1, T2 arg2, T3 arg3)
        {
            return CreateDelegate<Functor<T1, T2, T3>>(type)(arg1, arg2, arg3);
        }
        /// <summary>
        /// Builds an instance of an type with a constructor accepting four parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <param name="type">The type of the object to build.</param>
        /// <param name="arg1">The value of the first argument.</param>
        /// <param name="arg2">The value of the second argument.</param>
        /// <param name="arg3">The value of the third argument.</param>
        /// <param name="arg4">The value of the fourth argument.</param>
        /// <returns>An instance of the object.</returns>
        public static object Build<T1, T2, T3, T4>(Type type, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return CreateDelegate<Functor<T1, T2, T3, T4>>(type)(arg1, arg2, arg3, arg4);
        }
        /// <summary>
        /// Builds an instance of an type with a constructor accepting four parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <param name="type">The type of the object to build.</param>
        /// <param name="arg1">The value of the first argument.</param>
        /// <param name="arg2">The value of the second argument.</param>
        /// <param name="arg3">The value of the third argument.</param>
        /// <param name="arg4">The value of the fourth argument.</param>
        /// <param name="arg5">The value of the fifth argument.</param>
        /// <returns>An instance of the object.</returns>
        public static object Build<T1, T2, T3, T4, T5>(Type type, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return CreateDelegate<Functor<T1, T2, T3, T4, T5>>(type)(arg1, arg2, arg3, arg4, arg5);
        }

        // HWK-EYE-72622 (jabreu). Changing the skipVisibility flag to true, to allow dynamic methods
        // to invoke methods in internal classes.
        /// <summary>
        /// Creates the delegate to a constructor.
        /// </summary>
        /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
        /// <param name="type">The type.</param>
        /// <returns>An instance of a delegate to a constructor, for the <paramref name="type"/> supplied.</returns>
        /// <remarks>
        /// Delegates are cached in a static dictionary, so they are only created once per type+constructor combination and reused.
        /// </remarks>
        private static TDelegate CreateDelegate<TDelegate>(Type type)
        {
            TDelegate del = default(TDelegate);
            string key = GetKey(type, typeof(TDelegate));
            if (DelegateDictionary.ContainsKey(key) == false)
            {
                lock (SyncLock)
                {
                    if (DelegateDictionary.ContainsKey(key) == false)
                    {
                        Type[] argumentTypes = typeof(TDelegate).GetGenericArguments();
                        DynamicMethod dm = new DynamicMethod(string.Empty, type, argumentTypes, typeof(ObjectBuilder).Module, true);
                        ILGenerator generator = dm.GetILGenerator();
                        for (int i = 0; i < argumentTypes.Length; i++)
                        {
                            generator.Emit(OpCodes.Ldarg, i);
                        }
                        generator.Emit(OpCodes.Newobj, type.GetConstructor(argumentTypes));
                        generator.Emit(OpCodes.Ret);
                        del = (TDelegate)(object)dm.CreateDelegate(typeof(TDelegate));
                        DelegateDictionary.Add(key, del);
                    }
                }
            }
            else
            {
                del = (TDelegate)DelegateDictionary[key];
            }
            return del;
        }
        /// <summary>
        /// Gets the key identifying an type + constructor combination.
        /// </summary>
        /// <param name="sourceType">The type.</param>
        /// <param name="delegateType">Type of the delegate representing the constructor.</param>
        /// <returns>A string value uniquely representing the combination of type + constructor delegate</returns>
        private static string GetKey(Type sourceType, Type delegateType)
        {
            string result = null;
            Dictionary<Type, string> innerDictionary = null;
            if (KeyDictionary.TryGetValue(sourceType, out innerDictionary) == false)
            {
                lock (SyncLock)
                {
                    if (KeyDictionary.TryGetValue(sourceType, out innerDictionary) == false)
                    {
                        innerDictionary = new Dictionary<Type, string>();
                        KeyDictionary.Add(sourceType, innerDictionary);
                        result = GetKey(innerDictionary, sourceType, delegateType, true);
                    }
                }
            }
            else
            {
                result = GetKey(innerDictionary, sourceType, delegateType, false);
            }
            return result;
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="dictionary">The internal dictionary of keys for a given type, each entry a given constructor.</param>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="delegateType">Type of the delegate.</param>
        /// <param name="skipTest">if set to <c>true</c> skip test for the existence of the entry.</param>
        /// <returns>A string value representing the constructor delegate for given source type</returns>
        private static string GetKey(Dictionary<Type, string> dictionary, Type sourceType, Type delegateType, bool skipTest)
        {
            string result = null;
            if (skipTest)
            {
                result = BuildKey(dictionary, sourceType, delegateType);
            }
            else
            {
                if (dictionary.TryGetValue(delegateType, out result) == false)
                {
                    lock (SyncLock)
                    {
                        if (dictionary.TryGetValue(delegateType, out result) == false)
                        {
                            result = BuildKey(dictionary, sourceType, delegateType);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Builds the key.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="delegateType">Type of the delegate.</param>
        /// <returns>A string value representing the constructor delegate for given source type</returns>
        private static string BuildKey(Dictionary<Type, string> dictionary, Type sourceType, Type delegateType)
        {
            StringBuilder builder = new StringBuilder();
            BuildKey(builder, sourceType);
            BuildKey(builder, delegateType);
            string key = builder.ToString();
            dictionary.Add(delegateType, key);
            return key;
        }
        /// <summary>
        /// Builds a string key representing the type, and add its value to the buffer supplied.
        /// This function is call recursively to process generic types.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="type">The type.</param>
        private static void BuildKey(StringBuilder builder, Type type)
        {
            builder.AppendFormat("{0}.{1}+", type.Namespace, type.Name);
            if (type.IsGenericType)
            {
                Type[] argumentTypeList = type.GetGenericArguments();
                foreach (var argumentType in argumentTypeList)
                {
                    BuildKey(builder, argumentType);
                }
            }
        }
    }
}
