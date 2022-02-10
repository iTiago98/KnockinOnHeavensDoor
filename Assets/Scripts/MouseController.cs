using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public static MouseController instance;

    public List<Transform> clickableObjects;
    public GameObject fadeImage;
    public GameObject endImage;

    public bool enable = false;
    public bool sanPedroJudge = false;
    public AudioSource endMusic;
    

    private GameState _state = GameState.SELECTING;

    private ClickableObject _hovering = null;
    private ClickableObject _objectUp = null;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (!enable) return;

        ClickableObject clickableObject = GetRaycastObject();

        CheckHovering(clickableObject);
        CheckClick(clickableObject);

    }

    private ClickableObject GetRaycastObject()
    {
        var ray = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        var rayResult = Physics.Raycast(ray, out hit);

        ClickableObject clickableObject = null;
        if (rayResult)
        {
            var obj = hit.collider.gameObject;
            clickableObject = obj.GetComponent<ClickableObject>();
        }

        return clickableObject;
    }

    private void CheckHovering(ClickableObject clickableObject)
    {
        if (clickableObject != _hovering)
        {
            HoverExit();
            if (clickableObject != null && AvailableObject(clickableObject.gameObject))
            {
                clickableObject.OnMouseHoverEnter();
                _hovering = clickableObject;
            }
        }
    }

    public void CheckObjectUp(ClickableObject clickableObject)
    {
        if (_objectUp != null)
        {
            _objectUp.OnMouseClick();
            _objectUp = null;
        }
        else if (clickableObject != null)
        {
            _objectUp = clickableObject;
            _objectUp.OnMouseClick();
        }
    }

    private void CheckClick(ClickableObject clickableObject)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (clickableObject != null && AvailableObject(clickableObject.gameObject))
            {
                if (_state == GameState.PLAYING && IsGrabbableObject(clickableObject.gameObject))
                    CheckObjectUp(clickableObject);
                else
                    clickableObject.OnMouseClick();
            }
            else
            {
                switch (_state)
                {
                    case GameState.PLAYING:
                        CheckObjectUp(null);
                        break;
                    case GameState.STAMPED:
                        if (SceneManager.instance.IsLastCharacter())
                        {
                            endImage.GetComponent<Animator>().Play("End");
                            enable = false;
                            endMusic.Play();
                        }
                        else
                        {
                            fadeImage.GetComponent<Animator>().Play("FadeOut");
                            enable = false;
                        }
                        break;
                }
            }
        }
    }

    private bool AvailableObject(GameObject o)
    {
        switch (_state)
        {
            case GameState.SELECTING:
                return o.CompareTag("Suspect");
            case GameState.PLAYING:
                if (_objectUp != null)
                {
                    if (_objectUp.CompareTag("InterrogationFile")) return o.CompareTag("InterrogationOption");
                }
                else
                {
                    bool hellStampAvailable = (sanPedroJudge || !SceneManager.instance.IsSanPedro()) && o.CompareTag("HellStamp");
                    return o.CompareTag("InterrogationFile") || o.CompareTag("Fafita") || o.CompareTag("HeavenStamp") || hellStampAvailable;
                }
                break;
        }
        return false;
    }

    private bool IsGrabbableObject(GameObject o)
    {
        return o.CompareTag("InterrogationFile") || o.CompareTag("Fafita");
    }
    public void OnInterrogationFileClick()
    {
        SceneManager.instance.GetCurrentFile().OnMouseClick();
    }

    public void SetState(GameState state)
    {
        _state = state;
    }
    private void HoverExit()
    {
        if (_hovering != null)
        {
            _hovering.OnMouseHoverExit();
            _hovering = null;
        }
    }

    public void SetStateStamped()
    {
        _state = GameState.STAMPED;
    }
}
