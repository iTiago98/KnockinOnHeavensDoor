using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialogue
{
    public static char lineSeparator = '/';
    public static char lvlCounter = '$';

    public Sentence current;
    private List<Dialogue> options = new List<Dialogue>();

    public Dialogue(Sentence current) { this.current = current; }

    public Dialogue(Sentence current, params Dialogue[] options)
    {
        this.current = current;
        addOptions(options);
    }

    public Dialogue(string dialogue)
    {
        dialogue = dialogue.Replace("\n", "");
        dialogue = dialogue.Replace("\r", "");
        dialogue = dialogue.Replace("\t", "");

        this.current = null;
        this.options = buildTree(dialogue, 1);
    }

    private List<Dialogue> buildTree(string dialogue, int lvl)
    {
        List<Dialogue> tree = new List<Dialogue>();
        string separator = Lvl(lvl);
        string auxCurrent;
        List<Dialogue> auxOptions = new List<Dialogue>();
        int nextSeparator;

        string iterator = dialogue;
        while(!iterator.Equals("") && separator.Equals(iterator.Substring(0, separator.Length))) // One iteration for each element in the returning list.
        {
            iterator = iterator.Remove(0, separator.Length);
            nextSeparator = iterator.IndexOf(lineSeparator);
            if(nextSeparator == -1) nextSeparator = iterator.Length;
            auxCurrent = iterator.Substring(0,nextSeparator);
            iterator = iterator.Remove(0,nextSeparator);

            tree.Add(new Dialogue(new Sentence(auxCurrent)));
            if(!iterator.Equals(""))
            {
                if(Lvl(lvl+1).Equals(iterator.Substring(0, separator.Length+1)))
                {
                    nextSeparator = iterator.IndexOf(separator);
                    if(nextSeparator == -1) nextSeparator = iterator.Length;
                    tree[tree.Count-1].addOptions(buildTree(iterator.Substring(0, nextSeparator), lvl+1));
                    iterator = iterator.Remove(0, nextSeparator);
                }
            }
        }
        return tree;
    }

    private static string Lvl(int lvl)
    {
        string lvlCounters = "";
        for(int i = 1; i <= lvl; i++) lvlCounters += lvlCounter;
        return lineSeparator+lvlCounters+lineSeparator;
    }

    public void addOptions(List<Dialogue> options) {this.options.AddRange(options);}
    public void addOptions(Dialogue[] options) {this.options.AddRange(options);}

    public void selectOption(int i)
    {
        if(i < options.Count)
        {
            current = options[i].current;
            options = options[i].options;
        }
        else current = null;
    }

    public string speaker => options[0].current.speaker;

    public int numOptions => options.Count;

    public bool nextIsBranched => options[0].options.Count > 1;

    public List<string> getTextPC()
    {
        if(isPC)
        {
            List<string> text = new List<string>();
            foreach(Dialogue dialogue in options) text.Add(dialogue.current.sentence);
            return text;
        }
        else throw new System.Exception("Tried to access an NPC dialogue as a PC.");
    }

    public string getTextNPC()
    {
        if(!isPC) return options[0].current.sentence;
        else throw new System.Exception("Tried to access a PC dialogue as an NPC.");
    }

    public bool isPC => options[0].current.speaker == GameManager.placeholderOptions;

    public bool isEmpty => options.Count == 0;

    public bool isWarp()
    {
        if(current!=null) return current.warp!=-1;
        else return false;
    }

    public int warpIndex => current.warp;

    public void logTree(string hierarchy)
    {
        if(current!=null) Debug.Log("("+hierarchy+")"+current.ToString());
        for(int j = 0; j < options.Count; j++) options[j].logTree(hierarchy+"."+j);
    }
}