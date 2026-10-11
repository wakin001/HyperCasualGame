using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RopeCtrl : MonoBehaviour
{
    public enum State
    {
        None = 0,
        Running,
        End,
    }

    private static RopeCtrl _instance = null;
    public static RopeCtrl instance
    {
        get { return _instance; }
    }

    public Image fingerImage;

    public Canvas canvas;
    public RectTransform rectTransform;
    public Text successText;

    public GameObject[] ropePrefabs;

    public bool isRunning
    {
        get { return state == State.Running; }
    }
    private int curIndex = -1;
    private GameObject curRope = null;
    private CustomRopeDragger curRopeDragger = null;

    private State state = State.None;

    public GameObject successEffectPrefab = null;
    private GameObject successEffect = null;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        StartNextLevel();
    }


    public void UpdateFingerPos(Vector2 screenPos)
    {
        Vector2 localpoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPos, null, out localpoint);
        fingerImage.rectTransform.localPosition = localpoint;
    }

    public void Success()
    {
        VibrationPlayer.Vibrate(VibrationType.Short);
        state = State.End;
        fingerImage.gameObject.SetActive(false);
        //successText.gameObject.SetActive(true);
        curRopeDragger.enabled = false;
        successEffect = Instantiate<GameObject>(successEffectPrefab);
        StartCoroutine(StartNextLevelCoroutine());
    }

    private IEnumerator StartNextLevelCoroutine()
    {
        yield return new WaitForSeconds(6f);
        StartNextLevel();
    }

    private void StartNextLevel()
    {

        if (successEffect != null)
        {
            GameObject.Destroy(successEffect);
        }
        state = State.None;
        while (true)
        {
            int nextIndex = Random.Range(0, ropePrefabs.Length);
            if (nextIndex == curIndex)
            {
                continue;
            }
            if (curRope != null)
            {
                GameObject.Destroy(curRope);
            }
            curIndex = nextIndex;
            curRope = Instantiate(ropePrefabs[curIndex]);
            curRope.transform.position = Vector3.zero;
            curRopeDragger = curRope.GetComponent<CustomRopeDragger>();
            curRopeDragger.enabled = true;
            //successText.gameObject.SetActive(false);
            
            StartCoroutine(GameStartCoroutine());

            break;
        }
    }
   
    private IEnumerator GameStartCoroutine()
    {
        yield return new WaitForSeconds(2f);
        state = State.Running;
    }
}
