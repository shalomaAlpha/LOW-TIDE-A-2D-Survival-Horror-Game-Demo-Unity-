using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Document 
{
    public string owner;
    public int index;
    public string form;
    public Document(string owner, int index, string form)
    {
        this.owner = owner;
        this.index = index;
        this.form = form;
    }
}
