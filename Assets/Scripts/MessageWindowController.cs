using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageWindowController : MonoBehaviour
{
    [SerializeField] TextAsset m_textAsset = default;

    [SerializeField] TextMeshProUGUI m_captionView = default;
    [SerializeField] TextMeshProUGUI m_messageView = default;
    [SerializeField] GameObject m_choicesPrefab = default;
    [SerializeField] Transform m_choicesView = default;
    //[SerializeField] string[] m_messageData = default;
    //[SerializeField] float m_charSecond = 1;
    //bool m_requestNext = false;

    //IEnumerator[] m_ScenarioData = default;
    //[SerializeField] float m_anumeSecond = 1;
    [SerializeField] Image m_actor1 = default;
    [SerializeField] Image m_actor2 = default;
    [SerializeField] Image m_background1 = default;
    [SerializeField] Image m_background2 = default;

    //CancellationTokenSource m_cs = default;
    //bool m_isMessageNext = false;
    Process m_process = default; //�����o�ϐ��ɂ������Ȃ�///////////////////////////////
    //event Action m_nextEvent = null;

    public class Scenario
    {
        List<Process> m_scenarioData = new List<Process>();
        public List<Process> ScenarioData => m_scenarioData;
        //public Scenario(Process[] Scenario) => scenarioData = Scenario;
    }
    public class Process
    {
        List<IEnumerator> m_processData = new List<IEnumerator>();
        //CancellationTokenSource cs = new CancellationTokenSource();
        event Action m_skipEvent = null;

        public List<IEnumerator> ProcessData => m_processData;
        //public CancellationTokenSource CS => cs;
        //public Process(IEnumerator[] process) { processData = process };

        public void AddSkipEvent(Action act)
        {
            m_skipEvent += act;
        }
        public void Skip()
        {
            m_skipEvent?.Invoke();
            m_skipEvent = null;
        }
    }

    private void Start()
    {
        //m_cs = new CancellationTokenSource();
        //Scenario scenario = new Scenario(new Process[]{
        //    new Process(new IEnumerator[]
        //    {

        //        FadeImage(m_actor1, FadeType.FadeIn, 1, m_cs.Token),
        //        FadeImage(m_actor2, FadeType.FadeIn, 1, m_cs.Token), 
        //    }), 
        //    new Process(new IEnumerator[]
        //    {
        //        ShowMessagesAsync(new string[]{ "AAAAAAA","BBBBBBBBBBBBB","CCCCCCCCCC", } ,m_messageView, 0.2f, m_cs.Token),
        //    }),
        //});
        //Scenario scenario = CreateScenario(m_textAsset.text);
        StartCoroutine(PlayScenario(m_textAsset.text));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))//���N���b�N�ŃX�L�b�v�t���O��true��
        {
            //m_cs?.Cancel();
            //m_isMessageNext = true;

            //�e�R���[�`���J�n����m_skipEvent�Ɋe�t���O��ON�ɂ��鏈����ǉ����āA�N���b�N���ꂽ����s���A�S�Ĕj������////////////////////////////
            m_process?.Skip();
        }
        //else if (m_cs.IsCancellationRequested)
        //{
        //    m_cs.Dispose();
        //    Debug.Log(0);
        //}
    }



    //private Scenario CreateScenario(string scenarioText)//�R���X�g���N�^�ɂ���H////////////////////////////////////////////
    //{
    //    Scenario scenario = new Scenario();
    //    string[] scenarioData = scenarioText.Split('\n');
    //    foreach (var processText in scenarioData)
    //    {
    //        Process process = new Process();
    //        string[] eData = processText.Split('+');
    //        foreach (var eText in eData)
    //        {
    //            string[] data = eText.Split(',');
    //            IEnumerator e = default;

    //            if (data[0] == "FadeImage")//FadeImage,m_actor1,FadeIn,1
    //            {
    //                e = FadeImage(ConvertImage(data[1]), ConvertFadeType(data[2]), int.Parse(data[3]), process);
    //            }
    //            else if (data[0] == "ShowMessagesAsync")//ShowMessagesAsync,�咹���͂�,AAAAAAA/BBBBBBBBBBBBB/CCCCCCCCCC",0.2
    //            {
    //                e = ShowMessagesAsync(data[1], m_captionView, ConvertStrings(data[2]), m_messageView, float.Parse(data[3]), process);
    //            }
    //            else
    //            {
    //                Debug.Log("�w�肵���֐���������܂���");
    //            }

    //            process.ProcessData.Add(e);
    //        }
    //        scenario.ScenarioData.Add(process);
    //    }
    //    return scenario;
    //}
    private Image ConvertImage(string data)
    {
        if (data == "m_actor1")
        {
            return m_actor1;
        }
        else if (data == "m_actor2")
        {
            return m_actor2;
        }
        else if (data == "m_background1")
        {
            return m_background1;
        }
        else if (data == "m_background2")
        {
            return m_background2;
        }
        else
        {
            Debug.Log("�w�肵��Image��������܂���");
            return null;
        }
    }
    private FadeType ConvertFadeType(string data)
    {
        return (FadeType)Enum.Parse(typeof(FadeType), data);
    }
    private string[] ConvertTexts(string data)
    {
        return data.Split('/');
    }
    private int[] ConvertNumbers(string data)
    {
        return data.Split('/').Select(int.Parse).ToArray();
    }
    enum FadeType
    {
        FadeFromBlack,
        FadeIn,
        FadeOut,
    }

    //�V�i���I�f�[�^�̎��s
    private IEnumerator PlayScenario(string scenarioText)
    {
        //Scenario scenario = new Scenario();
        string[] scenarioData = scenarioText.Split('\n');
        for (int i = 0; i < scenarioData.Length; i++)
        //foreach (var processText in scenarioData)
        {
            Process process = new Process();
            string[] eData = scenarioData[i].Split('+');
            foreach (var eText in eData)
            {
                string[] data = eText.Split(',');
                IEnumerator e = default;

                if (data[0] == "FadeImage")//FadeImage,m_actor1,FadeIn,1
                {
                    e = FadeImage(ConvertImage(data[1]), ConvertFadeType(data[2]), float.Parse(data[3]), process);
                }
                else if (data[0] == "ShowMessagesAsync")//ShowMessagesAsync,�咹���͂�,AAAAAAA/BBBBBBBBBBBBB/CCCCCCCCCC",0.2
                {
                    e = ShowMessagesAsync(data[1], m_captionView, ConvertTexts(data[2]), m_messageView, float.Parse(data[3]), process);
                }
                else if (data[0] == "ChoicesMessage")
                {
                    e = ChoicesMessage(ConvertTexts(data[1]), ConvertNumbers(data[2]), m_choicesPrefab, m_choicesView, (int x) => LineChange(out i, x));
                }
                else if (data[0] == "LineChange")
                {
                    LineChange(out i, int.Parse(data[1]));
                    continue;
                }
                else
                {
                    Debug.Log("�w�肵���֐���������܂���");
                }
                process.ProcessData.Add(e);
            }
            m_process = process;
            if (process.ProcessData.Count == 1)
            {
                yield return StartCoroutine(process.ProcessData.First());
            }
            else if (process.ProcessData.Count > 1)
            {
                yield return WaitAllAsync(process);//�������s
            }
            yield return null;

            //scenario.ScenarioData.Add(process);
        }
        //return scenario;



        //foreach (var process in scenario.ScenarioData)
        //{
        //    m_process = process;//�N���b�N�Ď��p�̃R���[�`����Update(){if (Input.GetMouseButtonDown(0))�����悤�ɂ�����/////////////
        //    if (process.ProcessData.Count > 1)
        //    {
        //        yield return WaitAllAsync(process);//�������s
        //    }
        //    else
        //    {
        //        yield return StartCoroutine(process.ProcessData.First());
        //    }
        //    yield return null;
        //}
        //yield return null;
    }

    // �����ꂩ���I���܂�
    private IEnumerator WaitAnyAsync(Process process)
    {
        process.ProcessData.ForEach(x => StartCoroutine(Await(x, process.Skip)));
        yield return null;
    }

    //�������s
    private IEnumerator WaitAllAsync(Process process)
    {
        //�������s������
        int flagNum = 0;
        process.ProcessData.ForEach(x => StartCoroutine(Await(x, () => flagNum++)));
        yield return null;

        while (flagNum < process.ProcessData.Count)//�������s�������̂��S�ďI���iflagNum��e�����傫���Ȃ�j�܂�
        {
            yield return null;
        }
    }

    private IEnumerator Await(IEnumerator e, Action completed)
    {
        //yield return e;
        //�����ǂ���
        yield return StartCoroutine(e);

        completed();
    }

    //���� if(Input.GetButtonDown(0)) yield break; �ł����̂ł́H���ǂꂩ�I������瑼���L�����Z���̏������ł��Ȃ�
    //��UniTask�łȂ��̂ł���Εʂ�bool�t���O�ł����̂ł́H
    //���Ȃ��Ƃ��L�����Z�����������郁�b�Z�[�W�����ł͖���new���Ȃ��Ǝg���Ȃ��̂�bool�ł����͂�
    //�I�[�g�Ői�ނ��N���b�N�ł����i�܂Ȃ����̃t���O�K�v�H
    static IEnumerator FadeImage(Image image, FadeType stateName, float second, Process process)
    {
        bool isCancel = false;
        process.AddSkipEvent(() => isCancel = true);

        Animator anim = image.GetComponent<Animator>();
        anim.speed = 1 / second;
        anim.Play(stateName.ToString());
        yield return null;

        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            //ct.WaitHandle//Register�Ȃǂ��g����

            //if (ct.IsCancellationRequested)
            if (isCancel)
            {
                anim.Play(stateName.ToString(), 0, 1);
                yield break;
            }
            yield return null;
        }
        yield return null;
    }

    static IEnumerator ShowMessagesAsync(string caption, TextMeshProUGUI captionView, string[] messages, TextMeshProUGUI messageView, float charSecond, Process process)
    {
        captionView.text = caption;
        foreach (var line in messages)
        {
            ////CancellationTokenSource��new���܂��邩�@bool isCancel = false;nextEvent += () => isCancel = true;���܂��邩
            yield return ShowMessageAsync(line, messageView, charSecond, process);// �R���[�`������A�ʂ̃R���[�`���Ăяo��

            bool isCancel = false;
            process.AddSkipEvent(() => isCancel = true);

            while (!isCancel) { yield return null; }
            yield return null;
        }
        yield return null;
    }

    static IEnumerator ShowMessageAsync(string message, TextMeshProUGUI messageView, float charSecond, Process process)
    {
        bool isCancel = false;
        process.AddSkipEvent(() => isCancel = true);

        messageView.text = "";
        foreach (var ch in message)
        {
            messageView.text += ch;

            float time = 0;
            while (time < charSecond)
            {
                time += Time.deltaTime;
                if (isCancel)
                {
                    messageView.text = message;
                    yield break;
                }
                yield return null;
            }
            yield return null;
        }
    }

    static IEnumerator ChoicesMessage(string[] texts, int[] lineNums, GameObject choicesPrefab, Transform choicesView, Action<int> action)
    {
        bool isCancel = false;

        choicesView.gameObject.SetActive(true);
        for (int i = 0; i < texts.Length; i++)
        {
            GameObject go = Instantiate(choicesPrefab, choicesView);
            go.GetComponentInChildren<Text>().text = texts[i];
            int x = lineNums[i];
            go.GetComponent<Button>().onClick.AddListener(() =>
            {
                isCancel = true;
                action(x);
            });
        }

        while (!isCancel) { yield return null; }

        choicesView.gameObject.SetActive(false);
        foreach (Transform child in choicesView) Destroy(child.gameObject);
        yield return null;
    }

    void LineChange(out int counter, int newLineNum)
    {
        counter = newLineNum - 2;
    }


    //�L�����N�^�[�̕\��ύX
    //�L�����N�^�[�̋����\��
    //BGM�𗬂��A�~�߂�
    //���ʉ��𗬂�
    //�{�C�X�𗬂�
    //�I�[�g�Đ��@�\��ON�AOFF



    //static IEnumerator FadeImage(Image image, FadeType stateName, float second, CancellationToken ct)
    //{
    //    Animator anim = image.GetComponent<Animator>();
    //    anim.speed = 1 / second;
    //    anim.Play(stateName.ToString());
    //    yield return null;

    //    while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
    //    {
    //        //ct.WaitHandle//Register�Ȃǂ��g����

    //        if (ct.IsCancellationRequested)
    //        {
    //            anim.Play(stateName.ToString(), 0, 1);
    //            yield break;
    //        }
    //        yield return null;
    //    }
    //    yield return null;
    //}

    //IEnumerator[] processData1 = new IEnumerator[]
    //{
    //    FadeImage(m_actor1, FadeStateName.FadeIn, 1, m_cs.Token), //�z��ɂ��ĕ����t�F�[�h�H�@//������ł͂Ȃ�enum�ŕ�����H
    //    FadeImage(m_actor2, FadeStateName.FadeIn, 1, m_cs.Token), //�z��ɂ��ĕ����t�F�[�h�H�@//������ł͂Ȃ�enum�ŕ�����H
    //};
    //IEnumerator[] processData2 = new IEnumerator[]
    //{
    //    ShowMessagesAsync(new string[]
    //    {
    //        "AAAAAAAAAAAAA",
    //        "BBBBBBBBBBBBBBBBBBBBB",
    //        "CCCCCCCCCCCCCCCCCC",
    //    }
    //    ,m_messageView, 0.2f, m_cs.Token),
    //};
    //IEnumerator[][] scenarioData = new IEnumerator[][] { processData1, processData2 };


    //Func<IEnumerator> m_func = default;
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


    //    static IEnumerator WaitAnyAsync(IEnumerator e1, IEnumerator e2, CancellationTokenSource c)
    //    {
    //        var r1 = e1.MoveNext();
    //        var r2 = e2.MoveNext();
    //        yield return null;

    //        while (r1 && r2) // �����ꂩ���I���܂ŌJ��Ԃ�
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

    //        while (r1 || r2) // �������I���܂ŌJ��Ԃ�
    //        {
    //            r1 = e1.MoveNext();
    //            r2 = e2.MoveNext();
    //            yield return null;
    //        }
    //    }



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

    //private IEnumerator FadeImage(Image image, string animName)
    //{
    //    Animator anim = image.GetComponent<Animator>();
    //    anim.speed = 1 / m_anumeSecond;
    //    anim.Play(animName);
    //    yield return null;

    //    while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
    //    {
    //        if (m_requestNext) //�����Ƃ���m_requestNext�̎Q�Ƃ�n���H
    //        {
    //            m_requestNext = false;
    //            anim.Play(animName, 0, 1);
    //            yield break;
    //        }
    //        yield return null;
    //    }
    //    yield return null;
    //}
    //private IEnumerator CrossFadeImage(Image inImage, Image outImage)
    //{
    //    Animator inAnime = inImage.GetComponent<Animator>();
    //    Animator outAnime = outImage.GetComponent<Animator>();
    //    inAnime.speed = 1 / m_anumeSecond;
    //    outAnime.speed = 1 / m_anumeSecond;
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

    //private IEnumerator ShowMessagesAsync(string[] messages)
    //{
    //    foreach (var line in messages)
    //    {
    //        // �R���[�`������A�ʂ̃R���[�`���Ăяo��
    //        yield return ShowMessageAsync(line);

    //        while (!m_requestNext) { yield return null; }
    //        m_requestNext = false;
    //        yield return null;
    //    }
    //    yield return null;
    //}

    //private IEnumerator ShowMessageAsync(string message)
    //{
    //    m_messageView.text = "";
    //    foreach (var ch in message)
    //    {
    //        //yield return null;
    //        //if (Input.GetMouseButtonDown(0)) { break; }

    //        m_messageView.text += ch;
    //        //yield return new WaitForSeconds(m_charSecond); // �����ő҂��Ă���ԁA���͊��m���Ȃ�

    //        float time = 0;
    //        while (time < m_charSecond)
    //        {
    //            time += Time.deltaTime;

    //            if (m_requestNext)
    //            {
    //                m_requestNext = false;
    //                m_messageView.text = message;
    //                yield break;
    //            }

    //            yield return null;
    //        }
    //        yield return null;
    //    }
    //}


    //public class Sample : MonoBehaviour
    //{
    //    void Start()
    //    {
    //        var c = new CancellationTokenSource();
    //        var e1 = RotateAsync(Vector3.up, 2, c.Token);
    //        var e2 = ScaleAsync(new Vector3(0.02F, 0.02F, 0.02F), 3, c.Token);
    //        StartCoroutine(WaitAnyAsync(e1, e2, c));
    //    }

    //    static IEnumerator WaitAnyAsync(IEnumerator e1, IEnumerator e2, CancellationTokenSource c)
    //    {
    //        var r1 = e1.MoveNext();
    //        var r2 = e2.MoveNext();
    //        yield return null;

    //        while (r1 && r2) // �����ꂩ���I���܂ŌJ��Ԃ�
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

    //        while (r1 || r2) // �������I���܂ŌJ��Ԃ�
    //        {
    //            r1 = e1.MoveNext();
    //            r2 = e2.MoveNext();
    //            yield return null;
    //        }
    //    }
    //    IEnumerator RotateAsync(Vector3 axis, float duration, CancellationToken ct)
    //    {
    //        var t = 0F;
    //        ct.���W�X�^�[()=>
    //�@�@�@�@{
    //        �@�@Debug.Log("Rotate Cancel");
    //�@�@�@�@}
    //        while (t < duration)
    //        {
    //            if (ct.IsCancellationRequested)
    //            {
    //                Debug.Log("Rotate Cancel");
    //                break;
    //            }

    //            t += Time.deltaTime;
    //            transform.Rotate(axis);
    //            yield return null;
    //        }

    //        Debug.Log("Rotate Finish");
    //    }

    //    IEnumerator ScaleAsync(Vector3 scale, float duration, CancellationToken ct)
    //    {
    //        var t = 0F;
    //        while (t < duration)
    //        {
    //            if (ct.IsCancellationRequested)
    //            {
    //                Debug.Log("Scale Cancel");
    //                break;
    //            }

    //            t += Time.deltaTime;
    //            transform.localScale += scale;
    //            yield return null;
    //        }

    //        Debug.Log("Scale Finish");
    //    }
    //}




}
