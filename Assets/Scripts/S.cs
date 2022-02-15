using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 使う？
/// </summary>
public class S
{
    P[] scenarioData;

    public class P
    {
        IEnumerator[] processData;
    }

    //public ScenarioEvent(IEnumerator[] process) => this.process = process;


    enum FadeStateName
    {
        FadeFromBlack,
        FadeIn,
        FadeOut,
    }

    //private IEnumerator Entry(Image actor)
    //{
    //    float a = 0;
    //    while (a < 1)
    //    {
    //        a += Time.deltaTime;
    //        actor.color = new Color(1, 1, 1, a);
    //        if (m_requestNext)
    //        {
    //            m_requestNext = false;
    //            actor.color = new Color(1, 1, 1, 1);
    //            yield break;
    //        }
    //        yield return null;
    //    }
    //    yield return null;
    //}

    static IEnumerator FadeImage(Image image, FadeStateName stateName, float second, CancellationToken ct)
    {
        Animator anim = image.GetComponent<Animator>();
        anim.speed = 1 / second;
        anim.Play(stateName.ToString());
        yield return null;

        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            if (ct.IsCancellationRequested) 
            {
                anim.Play(stateName.ToString(), 0, 1);
                yield break;
            }
            yield return null;
        }
        yield return null;
    }
    //private IEnumerator CrossFadeImage(Image inImage, Image outImage, float second)
    //{
    //    Animator inAnime = inImage.GetComponent<Animator>();
    //    Animator outAnime = outImage.GetComponent<Animator>();
    //    inAnime.speed = 1 / second;
    //    outAnime.speed = 1 / second;
    //    inAnime.Play("FadeIn");
    //    outAnime.Play("FadeOut");
    //    yield return null;

    //    while (outAnime.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
    //    {
    //        if (m_requestNext)
    //        {
    //            m_requestNext = false;
    //            inAnime.Play("FadeIn", 0, 1);
    //            outAnime.Play("FadeOut", 0, 1);
    //            yield break;
    //        }
    //        yield return null;
    //    }
    //    yield return null;
    //}

    static IEnumerator ShowMessagesAsync(string[] messages, TextMeshProUGUI messageView, float charSecond, CancellationToken ct)
    {
        foreach (var line in messages)
        {
            // コルーチンから、別のコルーチン呼び出し
            yield return ShowMessageAsync(line, messageView, charSecond, ct);

            while (!ct.IsCancellationRequested) { yield return null; }
            yield return null;
        }
        yield return null;
    }

    static IEnumerator ShowMessageAsync(string message, TextMeshProUGUI messageView, float charSecond, CancellationToken ct)
    {
        messageView.text = "";
        foreach (var ch in message)
        {
            //yield return null;
            //if (Input.GetMouseButtonDown(0)) { break; }

            messageView.text += ch;
            //yield return new WaitForSeconds(m_charSecond); // ここで待っている間、入力感知しない

            float time = 0;
            while (time < charSecond)
            {
                time += Time.deltaTime;

                if (ct.IsCancellationRequested)
                {
                    messageView.text = message;
                    yield break;
                }

                yield return null;
            }
            yield return null;
        }
    }



    //    static IEnumerator WaitAnyAsync(IEnumerator e1, IEnumerator e2, CancellationTokenSource c)
    //    {
    //        var r1 = e1.MoveNext();
    //        var r2 = e2.MoveNext();
    //        yield return null;

    //        while (r1 && r2) // いずれかが終わるまで繰り返す
    //        {
    //            r1 = e1.MoveNext();
    //            r2 = e2.MoveNext();
    //            yield return null;
    //        }

    //        c.Cancel();
    //        e1.MoveNext();
    //        e2.MoveNext();
    //    }

    //    static IEnumerator WaitAllAsync(IEnumerator e1, IEnumerator e2)
    //    {
    //        var r1 = e1.MoveNext();
    //        var r2 = e2.MoveNext();
    //        yield return null;

    //        while (r1 || r2) // 両方が終わるまで繰り返す
    //        {
    //            r1 = e1.MoveNext();
    //            r2 = e2.MoveNext();
    //            yield return null;
    //        }
    //    }
}
