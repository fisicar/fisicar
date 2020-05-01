using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Collections/String List")]

public class ContentList : ScriptableObject
{
    public ProblemDefinition[] problems;

    public TopicDefinition[] topics;
}

[Serializable]
public class TopicDefinition
{
    public string topicTitle;
    
    public ProblemDefinition[] problems;
}