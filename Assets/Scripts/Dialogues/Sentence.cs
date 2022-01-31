using System.Collections;
using System.Collections.Generic;

public class Sentence
{
    public readonly string speaker;
    public readonly string sentence;
    public readonly int warp = -1;

    public Sentence(string speaker, string sentence) {this.speaker = speaker; this.sentence = sentence;}
    public Sentence(string input)
    {
        this.speaker = "";
        this.sentence = input;
        for(int i = 0; input[i] != '|'; i++)
            speaker += input[i];
        input = input.Remove(0,this.speaker.Length+1);
        if(input[input.Length-2] == '@')
        {
            warp = int.Parse(""+input[input.Length-1]);
            input = input.Remove(input.Length-2, 2);
        }
        this.sentence = input;
    }

    override public string ToString() => speaker + ": " + sentence;
}