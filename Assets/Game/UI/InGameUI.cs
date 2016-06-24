using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class InGameUI : SingletonBehavior<InGameUI>
{
    public GameObject DamageTextPrefab;
    public GameObject YouDiedDialog;
    public Text PlayerHealth;

    public RectTransform CutsceneHeader;
    public RectTransform CutsceneFooter;

    //    Canvas canvas;
    //
    void Start()
    {
        //this.canvas = this.GetComponent<Canvas>();
    }

    public void ShowDamageLabel(Vector3 pos, int amount)
    {
        var textGo = (GameObject)Instantiate(DamageTextPrefab, pos, Quaternion.identity);
        textGo.transform.localScale = Vector3.one;
        textGo.transform.position = new Vector3(textGo.transform.position.x, textGo.transform.position.y + 3f, textGo.transform.position.z);

        textGo.transform.GetChild(0).GetComponent<Text>().text = amount.ToString();

        textGo.transform.LookAt(Camera.main.transform.position);

        DOTween.Sequence()
            .Insert(0, textGo.transform.DOMoveY(textGo.transform.position.y + 2f, 1.5f))
            .Insert(0.5f , textGo.GetComponent<CanvasGroup>().DOFade(0f, 1f))
            .InsertCallback(1.5f, () => { Destroy(textGo); });
    }

    public void SetPlayerHealth(int health)
    {
        PlayerHealth.text = health.ToString();
    }

    public void CrewKilled()
    {
        this.YouDiedDialog.SetActive(true);
    }

    public void ActivateCutsceneView()
    {
        CutsceneFooter.DOSizeDelta(new Vector2(0, 80), 1f);
        CutsceneHeader.DOSizeDelta(new Vector2(0, 80), 1f);
    }

    public void DeactivateCutsceneView()
    {
        CutsceneFooter.DOSizeDelta(new Vector2(0, 0), 1f);
        CutsceneHeader.DOSizeDelta(new Vector2(0, 0), 1f);
    }
}
