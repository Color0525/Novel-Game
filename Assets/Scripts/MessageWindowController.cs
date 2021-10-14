using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageWindowController : MonoBehaviour
{
    //[SerializeField] TextMeshProUGUI m_captionText = default;
    [SerializeField] TextMeshProUGUI m_massageText = default;
    [SerializeField] public string[] m_massages = default;
    //[SerializeField] Massage m_massageClass = new Massage();

    //[Serializable]
    //public class Massage : IEnumerable
    //{
    //    public string[] m_massages = default;
    //    int m_currentIndex = 0;

    //    public string NextMassage()
    //    {
    //        if (m_currentIndex >= m_massages.Length)
    //        {

    //        }
    //        string next = m_massages[m_currentIndex];
    //        m_currentIndex++;
    //        return next;
    //    }

    //    public IEnumerator GetEnumerator()
    //    {
    //        return IEnumerator
    //    }
    //}

    void Start()
    {
        StartCoroutine(PlayMassage());
    }

    public IEnumerator PlayMassage()
    {
        foreach (var massage in m_massages)
        {
            m_massageText.text = massage;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            yield return null;
        }
    }
}
