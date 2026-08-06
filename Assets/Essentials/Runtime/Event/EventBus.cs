using UnityEngine;

namespace SUG.Essentials.Event
{
    // ====================== 普通事件（无返回值） ======================
    public interface IEventBus { }
    public interface IEventBus<T1> { }
    public static class EventBus<EventType> where EventType : IEventBus 
    {
        public delegate void EventHandle();

        private static event EventHandle OnEvent;
        public static void Subscribe(EventHandle handle) => OnEvent += handle;
        public static void UnSubscribe(EventHandle handle) => OnEvent -= handle;
        public static void Publish() => OnEvent?.Invoke();
    }

    public static class EventBus<EventType, T1> where EventType : IEventBus<T1>
    {
        public delegate void EventHandle(T1 arg1);

        public static event EventHandle OnEvent;
        public static void Subscribe(EventHandle handle) => OnEvent += handle;
        public static void UnSubscribe(EventHandle handle) => OnEvent -= handle;
        public static void Publish(T1 arg1) => OnEvent?.Invoke(arg1);
    }

    // ====================== 普通事件（有返回值） ======================
    public interface IEventBusR<TResult> { }
    public interface IEventBusR<T1, TResult> { }

    public static class EventBusR<EventType, TResult> where EventType : IEventBusR<TResult>
    {
        public delegate TResult EventHandle();

        private static event EventHandle OnEvent;

        public static void Subscribe(EventHandle handle) => OnEvent += handle;
        public static void UnSubscribe(EventHandle handle) => OnEvent -= handle;
        public static TResult Publish() => OnEvent != null ? OnEvent.Invoke() : default;
    }

    public static class EventBusR<EventType, T1, TResult> where EventType : IEventBusR<T1, TResult>
    {
        public delegate TResult EventHandle(T1 arg1);

        private static event EventHandle OnEvent;

        public static void Subscribe(EventHandle handle) => OnEvent += handle;
        public static void UnSubscribe(EventHandle handle) => OnEvent -= handle;
        public static TResult Publish(T1 arg1) => OnEvent != null ? OnEvent.Invoke(arg1) : default;
    }
}