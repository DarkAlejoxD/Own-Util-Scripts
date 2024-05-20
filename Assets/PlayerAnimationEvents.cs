using UnityEngine;
using AudioController;

[RequireComponent(typeof(Transform))]
public class PlayerAnimationEvents : MonoBehaviour
{
    #region Public Methods

    public void FootStepEvent()
    {
        //Singleton.TryGetInstance(out AudioManager manager);
        AudioManager.GetAudioManager().PlayOneShot(BankType.Enemies, "Step", transform.position);
    }

    #endregion
}