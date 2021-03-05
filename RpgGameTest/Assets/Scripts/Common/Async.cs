using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Async
{
    public static void Waterfall(AsyncTasks tasks, UnityAction callback = null)
    {
        if (tasks.Count() == 0)
        {
            if (callback != null) { callback(); }
            return;
        }
        tasks.Dequeue()(() => { Waterfall(tasks, callback); });
    }
}

public class AsyncTasks
{
    private List<UnityAction<UnityAction>> tasks = new List<UnityAction<UnityAction>>();
    public int Count() { return tasks.Count; }
    public void Add(UnityAction<UnityAction> callback) { tasks.Add(callback); }
    public UnityAction<UnityAction> Dequeue() { return tasks.Dequeue(); }
}

// IList 型の拡張メソッドを管理するクラス
public static class IListExtensions
{
    // 先頭にあるオブジェクトを削除し、返す
    public static T Dequeue<T>(this IList<T> self)
    {
        var result = self[0];
        self.RemoveAt(0);
        return result;
    }
}