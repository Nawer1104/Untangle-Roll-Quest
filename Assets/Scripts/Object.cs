using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Object : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private Material hitMat;
    private Material originalMat;

    public GameObject vfxDestroy;

    public int index;

    private bool isCompleted;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
        isCompleted = false;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;

        yield return new WaitForSeconds(.2f);

        sr.material = originalMat;
    }

    private void OnMouseDown()
    {
        if (isCompleted) return;

        if (GameManager.Instance.levels[GameManager.Instance.GetCurrentIndex()].index != this.index)
        {
            StartCoroutine("FlashFX");
            return;
        }
        else
        {
            isCompleted = true;
            DestroyObject();
        }
    }

    private void DestroyObject()
    {
        GameObject vfx = Instantiate(vfxDestroy, transform.position, Quaternion.identity) as GameObject;
        Destroy(vfx, 1f);

        GameManager.Instance.levels[GameManager.Instance.GetCurrentIndex()].index += 1;

        transform.DOScale(0, 1f).OnComplete(() => {
            gameObject.SetActive(false);
            GameManager.Instance.levels[GameManager.Instance.GetCurrentIndex()].gameObjects.Remove(gameObject);
            GameManager.Instance.CheckLevelUp();
        });

    }
}
