using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionController : MonoBehaviour
{
    [SerializeField] Animator transition;

    private IEnumerator TransitionCoroutine;

    public static TransitionController _instance = null;

    public Vector3 PlayerPositionOrigin { get; private set; }
    public Vector3 PlayerPositionDestiny { get; private set; }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    public void FadeIn()
    {
        _instance.transition.SetTrigger("fadeIn");
    }

    public void FadeOut()
    {
        _instance.transition.SetTrigger("fadeOut");
    }

    public void SetPlayerPositionOrigin(Vector3 newPosition)
    {
        this.PlayerPositionOrigin = newPosition;
    }

    public void SetPlayerPositionDestiny(Vector3 newPosition)
    {
        this.PlayerPositionDestiny = newPosition;
    }

    public void TransitionTo(GameObject objToMove, Vector3 newPosition)
    {
        TransitionCoroutine = Transition(objToMove, newPosition);
        StartCoroutine(TransitionCoroutine);
    }

    IEnumerator Transition(GameObject objToMove, Vector3 newPosition)
    {
        FadeOut();
        //Camera.current.enabled = false;
        //Camera.main.enabled = false;
        yield return new WaitForSeconds(1);

        SetPlayerPositionOrigin(objToMove.transform.position);
        objToMove.transform.position = newPosition;

        yield return new WaitForSeconds(1);

        //Camera.current.enabled = true;
        //Camera.main.enabled = true;
        FadeIn();
    }
}
