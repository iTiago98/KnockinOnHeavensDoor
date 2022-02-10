using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;

    public List<GameObject> characters;
    public GameObject sanPedroCharacter;

    private int _highlighted;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateCharacters(int characterIndex)
    {
        ShowCharacter(characterIndex - 1, false);
        ShowCharacter(characterIndex, true);

        if (characterIndex == 1 || characterIndex == 3) ShowSanPedro(true);
        else ShowSanPedro(false);
    }

    public void UpdateCharactersSprites(string speakerName)
    {
        UpdateSprite(_highlighted, false);

        if (speakerName == "San Pedro")
        {
            UpdateSprite(-1, true);
        }
        else
        {
            UpdateSprite(GetCharacterIndexFromName(speakerName), true);
        }
    }

    public void AllCharactersDefault()
    {
        for (int i = -1; i < characters.Count; i++)
        {
            UpdateSprite(i, true);
        }
    }
    
    public void AllCharactersDisabled()
    {
        for (int i = -1; i < characters.Count; i++)
        {
            UpdateSprite(i, false);
        }
    }

    public bool IsLastCharacter(int characterIndex)
    {
        return characterIndex == characters.Count - 1 || characterIndex == characters.Count - 2;
    }

    public bool IsSanPedro(int characterIndex)
    {
        return characterIndex == characters.Count - 1;
    }

    private void ShowCharacter(int characterIndex, bool show)
    {
        if (characterIndex >= 0) characters[characterIndex].SetActive(show);
    }

    public void ShowSanPedro(bool show)
    {
        sanPedroCharacter.SetActive(show);
    }

    private void UpdateSprite(int characterIndex, bool talking)
    {
        if (characterIndex == -2) return;
        _highlighted = (talking) ? characterIndex : -2;

        var character = (characterIndex == -1) ? sanPedroCharacter.GetComponent<Character>() : characters[characterIndex].GetComponent<Character>();
        var sprite = talking ? character.defaultImage : character.disabledImage;
        character.faceImage.sprite = sprite;
    }

    private int GetCharacterIndexFromName(string name)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].GetComponent<Character>().myName == name)
            {
                return i;
            }

        }
        return -2;
    }

}
