using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MessageWindow messageWindow;
    // Start is called before the first frame update
    void Start()
    {
        messageWindow.InitMessageWindow();
        AsyncTasks task = new AsyncTasks();
        task.Add(callback => messageWindow.ShowMessageWindow("ななしさん", "テストメッセージ", callback));
        task.Add(callback => messageWindow.ShowMessageWindow("ななしさん", "これはテストメッセージです", callback));
        task.Add(callback => messageWindow.ShowMessageWindow("しゅうりょう します", callback));

        Async.Waterfall(task, () => messageWindow.HideMessageWindow());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
