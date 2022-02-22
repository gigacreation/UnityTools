using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace GigaceeTools
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class ServiceLocator
    {
        /// <summary>
        /// インスタンスを登録する辞書。
        /// </summary>
        private static readonly Dictionary<Type, object> s_instances = new Dictionary<Type, object>();

        /// <summary>
        /// インスタンスを登録します。
        /// すでに同じ型のインスタンスが登録されている場合は登録できませんので、先に Unregister を行ってください。
        /// </summary>
        /// <param name="instance">登録するインスタンス。</param>
        /// <typeparam name="T">登録するインスタンスの型。</typeparam>
        public static void Register<T>(T instance) where T : class
        {
            Type type = typeof(T);

            // TODO: ContainsKey を TryGetValue に書き換える
            if (s_instances.ContainsKey(type))
            {
                Debug.LogWarning($"すでに同じ型のインスタンスが登録されています: {type.Name}");
                return;
            }

            s_instances[type] = instance;
        }

        /// <summary>
        /// インスタンスの登録を解除します。インスタンスが登録されていなかった場合は警告が出ます。
        /// </summary>
        /// <param name="instance">登録を解除するインスタンス。</param>
        /// <typeparam name="T">登録を解除するインスタンスの型。</typeparam>
        public static void Unregister<T>(T instance) where T : class
        {
            Type type = typeof(T);

            if (!s_instances.ContainsKey(type))
            {
                Debug.LogWarning($"要求された型のインスタンスが登録されていません: {type.Name}");
                return;
            }

            if (!Equals(s_instances[type], instance))
            {
                Debug.LogWarning($"登録されている要求された型のインスタンスと渡されたインスタンスが一致しません: {type.Name}");
                return;
            }

            if (s_instances[type] is IDisposable disposable)
            {
                disposable.Dispose();
            }

            s_instances.Remove(type);
        }

        /// <summary>
        /// 指定された型のインスタンスがすでに登録されているかをチェックします。
        /// </summary>
        /// <typeparam name="T">登録を確認するインスタンスの型。</typeparam>
        /// <returns>指定された型のインスタンスがすでに登録されている場合は true を返します。</returns>
        public static bool IsRegistered<T>() where T : class
        {
            return s_instances.ContainsKey(typeof(T));
        }

        /// <summary>
        /// 渡されたインスタンスがすでに登録されているかをチェックします。
        /// </summary>
        /// <param name="instance">登録を確認するインスタンス。</param>
        /// <typeparam name="T">登録を確認するインスタンスの型。</typeparam>
        /// <returns>渡されたインスタンスが既に登録されている場合は true を返します。</returns>
        public static bool IsRegistered<T>(T instance) where T : class
        {
            Type type = typeof(T);

            return s_instances.ContainsKey(type) && Equals(s_instances[type], instance);
        }

        /// <summary>
        /// インスタンスを取得します。取得できなかった場合はエラーになります。
        /// </summary>
        /// <typeparam name="T">取得したいインスタンスの型。</typeparam>
        /// <returns>取得したインスタンスを返します。取得できなかった場合は null を返します。</returns>
        public static T Get<T>() where T : class
        {
            Type type = typeof(T);

            if (s_instances.ContainsKey(type))
            {
                return s_instances[type] as T;
            }

            Debug.LogError($"要求された型のインスタンスが登録されていません: {type.Name}");
            return null;
        }

        /// <summary>
        /// インスタンスを取得し、渡された引数に代入します。取得できなかった場合は null が入ります。
        /// </summary>
        /// <param name="instance">取得したインスタンスを入れる変数。</param>
        /// <typeparam name="T">取得したいインスタンスの型。</typeparam>
        /// <returns>取得が成功したら true を返します。</returns>
        public static bool TryGet<T>(out T instance) where T : class
        {
            Type type = typeof(T);
            instance = s_instances.ContainsKey(type) ? s_instances[type] as T : null;

            return instance != null;
        }

        /// <summary>
        /// インスタンスの登録をすべて解除します。
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ClearInstances()
        {
            s_instances.Clear();
        }
    }
}
