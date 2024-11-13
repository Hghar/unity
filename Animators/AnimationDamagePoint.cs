using AssetStore.HeroEditor.Common.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDamagePoint : MonoBehaviour
{
    private void OnEnable()
    {
        if(this.gameObject.transform.parent.parent.parent.GetComponentInChildren<CharacterBodySculptor>())
            this.transform.parent = this.gameObject.transform.parent.parent.parent.GetComponentInChildren<CharacterBodySculptor>().MeleeWeapon[0];
    }
}
