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
    public enum State
    {
        SELECTING, PLAYING, STAMPING, STAMPED
    }

    private State _state = State.SELECTING;
    private bool _fileUp = false;
    private bool _fafitaUp = false;

    private ClickableObject _hovering = null;
    private ClickableObject _objectUp = null;

    private void Awake()
    {
        instance = this;
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

    private void CheckObjectUp(ClickableObject clickableObject)
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

    void Update()
    {
        if (!enable) return;

        ClickableObject clickableObject = GetRaycastObject();

        CheckHovering(clickableObject);

        if (Input.GetMouseButtonDown(0))
        {
            if (clickableObject != null && AvailableObject(clickableObject.gameObject))
            {
                if (_state == State.PLAYING && IsGrabbableObject(clickableObject.gameObject))
                    CheckObjectUp(clickableObject);
                else
                    clickableObject.OnMouseClick();
                //switch (_state)
                //{
                //    case State.SELECTING:
                //        clickableObject.OnMouseClick();
                //        break;
                //    case State.PLAYING:
                //        switch (clickableObject.tag)
                //        {
                //            case "InterrogationFile":
                //            case "Fafita":
                //                CheckObjectUp(clickableObject);
                //                break;
                //            default:
                //                clickableObject.OnMouseClick();
                //                break;
                //        }
                //        break;
                //}
            }
            else
            {
                switch (_state)
                {
                    case State.PLAYING:
                        CheckObjectUp(null);
                        //if (_fileUp)
                        //{
                        //    OnInterrogationFileClick();
                        //    return;
                        //}
                        //if (_fafitaUp)
                        //{
                        //    OnFafitaClick();
                        //    return;
                        //}
                        break;
                    case State.STAMPED:
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

    //private void OnMouseClick(ClickableObject o)
    //{
    //    if (AvailableObject(o.gameObject)) o.OnMouseClick();
    //}

    //private void OnMouseHoverExit()
    //{
    //    foreach (Transform transform in clickableObjects)
    //    {
    //        transform.gameObject.GetComponent<ClickableObject>().OnMouseHoverExit();
    //    }

    //    foreach (Transform transform in SceneManager.instance.GetInterrogationOptions())
    //    {
    //        transform.gameObject.GetComponent<ClickableObject>().OnMouseHoverExit();
    //    }

    //    CharacterManager.instance.GetCharacters().gameObject.GetComponent<ClickableObject>().OnMouseHoverExit();
    //    CharacterManager.instance.GetSanPedro().gameObject.GetComponent<ClickableObject>().OnMouseHoverExit();
    //}

    //private void OnMouseHoverEnter(ClickableObject o)
    //{
    //    if (AvailableObject(o.gameObject)) o.OnMouseHoverEnter();
    //}

    private bool AvailableObject(GameObject o)
    {
        //return (_state == State.PLAYING && _fileUp && o.CompareTag("InterrogationOption"))
        //    || (_state == State.PLAYING && !_fileUp && !_fafitaUp && (o.CompareTag("InterrogationFile") || o.CompareTag("HeavenStamp") || o.CompareTag("Fafita")))
        //    || (_state == State.PLAYING && !_fileUp && !_fafitaUp && (sanPedroJudge || !SceneManager.instance.IsSanPedro()) &&  (o.CompareTag("HellStamp")))
        //|| (_state == State.SELECTING && (o.CompareTag("Suspect")));

        switch (_state)
        {
            case State.SELECTING:
                return o.CompareTag("Suspect");
            case State.PLAYING:
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
        _fileUp = !_fileUp;
    }

    private void OnFafitaClick()
    {
        SceneManager.instance.GetFafita().GetComponent<ClickableObject>().OnMouseClick();
        _fafitaUp = !_fafitaUp;
    }

    public void SetStateSelecting()
    {
        _state = State.SELECTING;
    }
    public void SetStatePlaying()
    {
        _state = State.PLAYING;
    }
    public void SetStateStamping()
    {
        _state = State.STAMPING;
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
        _state = State.STAMPED;
    }
}
