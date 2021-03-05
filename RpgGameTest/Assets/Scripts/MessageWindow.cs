using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class MessageWindow : MonoBehaviour
{
    public CanvasGroup messageWindowFrame; // メッセージウィンドウ全体の透明度管理コンポーネント
    public CanvasGroup nameFrame; // 名前表示部分の透明度管理コンポーネント
    public CanvasGroup mainFrame; // メインテキスト表示部分の透明度管理コンポーネント
    public TextMeshProUGUI nameText; // 名前テキスト用コンポーネント
    public TextMeshProUGUI mainText; // メインテキスト用コンポーネント
    public GameObject icon; // アイコン画像
    public float speed; // 一文字が表示されるまでに掛かる秒数
    private float _elapsedTime; // 経過時間。一文字が表示され終わったら0秒にリセットされる
    private int _textNum; // 一文字表示中の添え字
    private string _messageStr; // 表示予定の全ての文字列
    private string _showStr; // 現在表示中の文字列
    private UnityAction _endCallback; // 全て表示後にクリックした際に、実行される終了コールバック変数
    private bool _isShowEnd; // 全ての文字列が表示し終えたか

    // クリックイベント
    // テキストが送信されたら、時間経過で表示される
    // 途中でクリックすると、一気に全て表示される
    // 全て表示されると、アイコンが表示される
    // アイコンが表示されている時に、クリックすると、コールバックを再生する
    // コールバックがないときは、消える

    void Update()
    {
        // もし、メッセージウィンドウの透明度が0ならば
        if (messageWindowFrame.alpha == 0)
        {
            // 処理をせずに終了
            return;
        }

        // 経過時間を加算
        _elapsedTime += Time.deltaTime;

        // 全ての文字列を表示し終えていない場合で、経過時間が一文字が表示されるまでに掛かる秒数を超えたとき
        if (!_isShowEnd && _elapsedTime >= speed)
        {
            // 文字の更新
            UpdateMessage();
        }

        // マウスの左ボタンを押していたら
        if (Input.GetMouseButtonDown(0))
        {
            // 全ての文字を表示し終えた判定がtrueの場合
            if (_isShowEnd)
            {
                // 終了処理を実行する
                EndCallback();
            }
            else
            {
                // 全ての文字を表示する
                ShowAllMessage();
            }
        }
    }

    // 文字の更新
    void UpdateMessage()
    {
        // 経過時間をリセット
        _elapsedTime = 0;
        // 表示される文字列に、一文字追加する
        _showStr += _messageStr[_textNum];
        // メインテキストの文字に、表示文字列を代入する
        mainText.text = _showStr;
        // 一文字表示中の添え字を更新
        _textNum++;
        // 終了判定

        // 添え字の数字が、表示予定の全ての文字列の文字数以上になったとき
        if (_textNum >= _messageStr.Length)
        {
            // アイコンを表示する
            icon.SetActive(true);
            // 全ての文字を表示し終えた判定をtrueにする
            _isShowEnd = true;
        }
    }

    // 全ての文字を表示する
    void ShowAllMessage()
    {
        // メインテキストの文字に、表示予定の全ての文字列を代入する
        mainText.text = _messageStr;
        // アイコンを表示する
        icon.SetActive(true);
        // 全ての文字を表示し終えた判定をtrueにする
        _isShowEnd = true;
    }

    // メッセージウィンドウを非表示にする
    public void HideMessageWindow()
    {
        // 名前テキストの文字をリセット
        nameText.text = "";
        // メインテキストの文字をリセット
        mainText.text = "";
        // メッセージウィンドウの透明度を0にする
        messageWindowFrame.alpha = 0;
        // 名前表示部分の透明度を0にする
        nameFrame.alpha = 0;
        // メインテキスト表示部分の透明度を0にする
        mainFrame.alpha = 0;
        // アイコンを非表示にする
        icon.SetActive(false);
        // 経過時間をリセットする
        _elapsedTime = 0;
        // 一文字表示中の添え字をリセット
        _textNum = 0;
        // 全ての文字を表示し終えた判定をfalseにする
        _isShowEnd = false;
    }

    // メッセージを表示する際の共通処理
    void InitShowMessage(string mainStr, UnityAction callback)
    {
        // アイコンのactiveがtrueな時
        if (icon.activeInHierarchy)
        {
            // アイコンのactiveをfalseにする
            icon.SetActive(false);
        }

        // メッセージウィンドウの透明度を1にする
        messageWindowFrame.alpha = 1;
        // 名前表示部分の透明度を0にする
        nameFrame.alpha = 0;
        // メインテキスト表示部分の透明度を1にする
        mainFrame.alpha = 1;
        // 経過時間をリセットする
        _elapsedTime = 0;
        // 一文字表示中の添え字をリセット
        _textNum = 0;
        // 名前テキスト用コンポーネント
        nameText.text = "";
        // 現在表示中の文字列をリセット
        _showStr = "";
        // 表示予定の全ての文字列
        _messageStr = mainStr;
        // メインテキストの文字に、表示文字列を代入する
        mainText.text = _showStr;
        // 終了コールバックを代入
        _endCallback = callback;
        // 全ての文字を表示し終えた判定をfalseにする
        _isShowEnd = false;
    }

    // 終了処理を実行
    void EndCallback()
    {
        // 終了処理のコールバックがnullじゃなければ
        if (_endCallback != null)
        {
            // 終了処理のコールバックを実行
            _endCallback();
        }
        // そうでなければ
        else
        {
            // メッセージウィンドウを非表示にする
            HideMessageWindow();
        }
    }

    // メッセージウィンドウの初期化処理
    public void InitMessageWindow()
    {
        // メッセージウィンドウを非表示にする
        HideMessageWindow();
    }

    // 名前付きメッセージ表示関数
    public void ShowMessageWindow(string nameStr, string mainStr, UnityAction callback = null)
    {
        // メッセージを表示する際の共通処理
        InitShowMessage(mainStr, callback);
        // 名前表示部分の透明度を1にする
        nameFrame.alpha = 1;
        // 名前テキストの文字に、名前文字列を代入する
        nameText.text = nameStr;
    }

    // 名前無しメッセージ表示関数
    public void ShowMessageWindow(string mainStr, UnityAction callback = null)
    {
        // メッセージを表示する際の共通処理
        InitShowMessage(mainStr, callback);
    }
}
