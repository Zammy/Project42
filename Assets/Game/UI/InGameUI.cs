using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class InGameUI : SingletonBehavior<InGameUI>
{
    public GameObject DamageTextPrefab;
    public GameObject YouDiedDialog;

//    Canvas canvas;
//
//    void Start()
//    {
//        this.canvas = this.GetComponent<Canvas>();
//    }

    public void ShowDamageLabel(Vector3 pos, int amount)
    {
        var textGo = (GameObject)Instantiate(DamageTextPrefab, pos, Quaternion.identity);
        textGo.transform.localScale = Vector3.one;

        textGo.transform.GetChild(0).GetComponent<Text>().text = amount.ToString();

        DOTween.Sequence()
            .Insert(0, textGo.transform.DOMoveY(pos.y + 1f, 1f))
            .Insert(0, textGo.GetComponent<CanvasGroup>().DOFade(0f, 1f))
            .InsertCallback(1f, () => { Destroy(textGo); });
    }

    public void CrewKilled()
    {
        this.YouDiedDialog.SetActive(true);
    }
}
