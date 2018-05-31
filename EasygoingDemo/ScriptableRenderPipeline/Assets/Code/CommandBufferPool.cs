// This came straight from the Unity SRP GitHub Project.

using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering {
    class ObjectPool<T> where T : new() {
        readonly Stack<T> mStack = new Stack<T>();
        readonly Action<T> mOnGetHandle;
        readonly Action<T> mOnReleaseHandle;

        public int CountAll { get; private set; }
        public int CountActive { get { return CountAll - CountInactive; } }
        public int CountInactive { get { return mStack.Count; } }

        public ObjectPool(Action<T> actionOnGet, Action<T> actionOnRelease) {
            mOnGetHandle = actionOnGet;
            mOnReleaseHandle = actionOnRelease;
        }

        public T Get() {
            T element;
            if (mStack.Count == 0) {
                element = new T();
                CountAll++;
            }
            else
                element = mStack.Pop();

            mOnGetHandle?.Invoke(element);
            return element;
        }

        public void Release(T element) {
            if (mStack.Count > 0 && ReferenceEquals(mStack.Peek(), element))
                Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");

            mOnReleaseHandle?.Invoke(element);
            mStack.Push(element);
        }
    }

    public static class CommandBufferPool {
        static ObjectPool<CommandBuffer> s_BufferPool = new ObjectPool<CommandBuffer>(null, x => x.Clear());

        public static CommandBuffer Get() {
            var cmd = s_BufferPool.Get();
            cmd.name = "Unnamed Command Buffer";
            return cmd;
        }

        public static CommandBuffer Get(string name) {
            var cmd = s_BufferPool.Get();
            cmd.name = name;
            return cmd;
        }

        public static void Release(CommandBuffer buffer) {
            s_BufferPool.Release(buffer);
        }
    }
}
