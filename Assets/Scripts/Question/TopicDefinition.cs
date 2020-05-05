using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TopicDefinition
{
    public string topicTitle;

    public Sprite topicSprite;

    [Multiline] public string topicDescription;
    
    public ProblemDefinition[] problems;
}