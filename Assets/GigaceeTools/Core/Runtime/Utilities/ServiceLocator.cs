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
        /// サービスを登録する辞書。
        /// </summary>
        private static readonly Dictionary<Type, object> s_services = new();

        /// <summary>
        /// サービスを登録します。
        /// すでに同じ型のサービスが登録されている場合は登録できませんので、先に Unregister を行ってください。
        /// </summary>
        /// <param name="service">登録するサービス。</param>
        /// <typeparam name="TService">登録するサービスの型。</typeparam>
        public static void Register<TService>(TService service) where TService : class, IService
        {
            Type type = typeof(TService);

            // TODO: ContainsKey を TryGetValue に書き換える
            if (s_services.ContainsKey(type))
            {
                Debug.LogWarning($"すでに同じ型のサービスが登録されています: {type.Name}");
                return;
            }

            s_services[type] = service;
        }

        /// <summary>
        /// サービスの登録を解除します。サービスが登録されていなかった場合は警告が出ます。
        /// </summary>
        /// <param name="service">登録を解除するサービス。</param>
        /// <typeparam name="TService">登録を解除するサービスの型。</typeparam>
        public static void Unregister<TService>(TService service) where TService : class, IService
        {
            Type type = typeof(TService);

            if (!s_services.ContainsKey(type))
            {
                Debug.LogWarning($"要求された型のサービスが登録されていません: {type.Name}");
                return;
            }

            if (!Equals(s_services[type], service))
            {
                Debug.LogWarning($"登録されている要求された型のサービスと渡されたサービスが一致しません: {type.Name}");
                return;
            }

            if (s_services[type] is IDisposable disposable)
            {
                disposable.Dispose();
            }

            s_services.Remove(type);
        }

        /// <summary>
        /// 指定された型のサービスがすでに登録されているかをチェックします。
        /// </summary>
        /// <typeparam name="TService">登録を確認するサービスの型。</typeparam>
        /// <returns>指定された型のサービスがすでに登録されている場合は true を返します。</returns>
        public static bool IsRegistered<TService>() where TService : class, IService
        {
            return s_services.ContainsKey(typeof(TService));
        }

        /// <summary>
        /// 渡されたサービスがすでに登録されているかをチェックします。
        /// </summary>
        /// <param name="service">登録を確認するサービス。</param>
        /// <typeparam name="TService">登録を確認するサービスの型。</typeparam>
        /// <returns>渡されたサービスが既に登録されている場合は true を返します。</returns>
        public static bool IsRegistered<TService>(TService service) where TService : class, IService
        {
            Type type = typeof(TService);

            return s_services.ContainsKey(type) && Equals(s_services[type], service);
        }

        /// <summary>
        /// サービスを取得します。取得できなかった場合はエラーになります。
        /// </summary>
        /// <typeparam name="TService">取得したいサービスの型。</typeparam>
        /// <returns>取得したサービスを返します。取得できなかった場合は null を返します。</returns>
        public static TService Get<TService>() where TService : class, IService
        {
            Type type = typeof(TService);

            if (s_services.ContainsKey(type))
            {
                return s_services[type] as TService;
            }

            Debug.LogError($"要求された型のサービスが登録されていません: {type.Name}");
            return null;
        }

        /// <summary>
        /// サービスを取得し、渡された引数に代入します。取得できなかった場合は null が入ります。
        /// </summary>
        /// <param name="service">取得したサービスを入れる変数。</param>
        /// <typeparam name="TService">取得したいサービスの型。</typeparam>
        /// <returns>取得が成功したら true を返します。</returns>
        public static bool TryGet<TService>(out TService service) where TService : class, IService
        {
            Type type = typeof(TService);
            service = s_services.ContainsKey(type) ? s_services[type] as TService : null;

            return service != null;
        }

        /// <summary>
        /// サービスの登録をすべて解除します。
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ClearServices()
        {
            s_services.Clear();
        }
    }
}
