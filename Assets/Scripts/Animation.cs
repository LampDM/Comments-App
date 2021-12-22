using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimationJson
{
    public AnimationClips[] AnimationClips;
}

[Serializable]
public class AnimationClips
{
    
    public string clipname;
    public string[] animated_parts;
    public string instruction;
}
