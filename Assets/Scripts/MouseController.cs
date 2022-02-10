using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    private static MouseController _instance;

    public static MouseController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindGameObjectWithTag("MouseController").GetComponent<MouseController>();
            }

            return _instance;
        }
    }


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

    void Update()
    {
        if (!enable) return;

        var ray = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        var rayResult = Physics.Raycast(ray, out hit);

        ClickableObject clickableObject = null;
        if (rayResult)
        {
            var obj = hit.collider.gameObject;
            clickableObject = obj.GetComponent<ClickableObject>();
        }

        var mouseInput = Input.GetMouseButtonDown(0);
        OnMouseHoverExit();
        if (clickableObject != null && AvailableObject(clickableObject.gameObject))
        {
            if (_state == State.SELECTING)
            {
                OnMouseHoverEnter(clickableObject);
                if (mouseInput && clickableObject.CompareTag("Suspect"))
                {
                    clickableObject.OnMouseClick();
                }
            }
            else if (_state == State.PLAYING)
            {
                OnMouseHoverExit();
                OnMouseHoverEnter(clickableObject);
                if (mouseInput)
                {
                    switch (clickableObject.tag)
                    {
                        case "InterrogationFile":
                            OnInterrogationFileClick();
                            break;
                        case "InterrogationOption":
                        case "HeavenStamp":
                        case "HellStamp":
                            OnMouseClick(clickableObject);
                            break;
                        case "Fafita":
                            OnFafitaClick();
                            break;
                    }
                }
            }
        }
        else
        {
            if (mouseInput)
            {
                switch (_state)
                {
                    case State.PLAYING:
                        if (_fileUp)
                        {
                            OnInterrogationFileClick();
                            return;
                        }
                        if (_fafitaUp)
                        {
                            OnFafitaClick();
                            return;
                        }
                        break;
                    case State.STAMPED:
                        if (SceneManager.Instance.IsRosa() || SceneManager.Instance.IsSanPedro())
                        {
                            endImage.GetComponent<Animator>().Play("End");
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

    private void OnMouseClick(ClickableObject o)
    {
        if (AvailableObject(o.gameObject)) o.OnMouseClick();
    }

    private void OnMouseHoverExit()
    {
        foreach (Transform transform in clickableObjects)
        {
            transform.gameObject.GetComponent<ClickableObject>().OnMouseHoverExit();
        }

        foreach (Transform transform in SceneManager.Instance.GetInterrogationOptions())
        {
            transform.gameObject.GetComponent<ClickableObject>().OnMouseHoverExit();
        }


        SceneManager.Instance.GetCharacters().gameObject.GetComponent<ClickableObject>().OnMouseHoverExit();

        SceneManager.Instance.GetSanPedro().gameObject.GetComponent<ClickableObject>().OnMouseHoverExit();
    }

    private void OnMouseHoverEnter(ClickableObject o)
    {
        if (AvailableObject(o.gameObject)) o.OnMouseHoverEnter();
    }

    private bool AvailableObject(GameObject o)
    {
        return (_state == State.PLAYING && _fileUp && o.CompareTag("InterrogationOption"))
            || (_state == State.PLAYING && !_fileUp && !_fafitaUp && (o.CompareTag("InterrogationFile") || o.CompareTag("HeavenStamp") || o.CompareTag("Fafita")))
            || (_state == State.PLAYING && (sanPedroJudge || !SceneManager.Instance.IsSanPedro()) && !_fileUp && !_fafitaUp && (o.CompareTag("HellStamp")))
        || (_state == State.SELECTING && (o.CompareTag("Suspect")));
    }

    public void OnInterrogationFileClick()
    {
        SceneManager.Instance.GetCurrentFile().OnMouseClick();
        _fileUp = !_fileUp;
    }

    private void OnFafitaClick()
    {
        SceneManager.Instance.GetFafita().GetComponent<ClickableObject>().OnMouseClick();
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
        OnMouseHoverExit();
    }
    public void SetStateStamped()
    {
        _state = State.STAMPED;
    }
}
