using UnityEngine;

public class DefaultSword : Weapon
{
    public override void UseWeapon(Vector3 lastInputDirection)
    {
        Debug.Log("Using Default Sword!");
        // Implement default sword-specific behavior
    }
}
