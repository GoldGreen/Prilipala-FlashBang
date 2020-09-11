using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public static class UnityEventExtensions
{
    private class SmartSubscriber : IDisposable
    {
        private UnityAction subscriber;
        private UnityEvent subscribe;

        public SmartSubscriber(UnityEvent unityEvent, UnityAction unityAction)
        {
            subscriber = unityAction;
            subscribe = unityEvent;

            subscribe.AddListener(subscriber);
        }

        public void Dispose()
        {
            subscribe.RemoveListener(subscriber);
            subscribe = null;
            subscriber = null;
        }
    }

    private class SmartSubscriber<T> : IDisposable
    {
        private UnityAction<T> subscriber;
        private UnityEvent<T> subscribe;

        public SmartSubscriber(UnityEvent<T> unityEvent, UnityAction<T> unityAction)
        {
            subscriber = unityAction;
            subscribe = unityEvent;

            subscribe.AddListener(subscriber);
        }

        public void Dispose()
        {
            subscribe.RemoveListener(subscriber);
            subscribe = null;
            subscriber = null;
        }
    }

    public static IDisposable Subscribe(this UnityEvent unityEvent, UnityAction action)
     => new SmartSubscriber(unityEvent, action);

    public static IDisposable Subscribe<TOut>(this UnityEvent unityEvent, Func<TOut> func)
    => new SmartSubscriber(unityEvent, () => func());

    public static IDisposable Subscribe<T>(this UnityEvent<T> unityEvent, UnityAction<T> action)
    => new SmartSubscriber<T>(unityEvent, action);

    public static IDisposable Subscribe<T>(this UnityEvent<T> unityEvent, UnityAction action)
    => new SmartSubscriber<T>(unityEvent, _ => action());
    public static IDisposable Subscribe<T, TOut>(this UnityEvent<T> unityEvent, Func<T, TOut> func)
    => new SmartSubscriber<T>(unityEvent, x => func(x));
    public static IDisposable Subscribe<T, TOut>(this UnityEvent<T> unityEvent, Func<TOut> func)
    => new SmartSubscriber<T>(unityEvent, _ => func());
}