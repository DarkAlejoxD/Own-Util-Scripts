using UnityEngine;
using UtilsComplements;

public class AudioManager : MonoBehaviour, ISingleton<AudioManager>
{
    [Header("References")]
    [SerializeField] private GameObject _prefab;

    [Header("Attributes")]
    [SerializeField] private float _attribute;

    public ISingleton<AudioManager> Instance => this;

    public AudioManager Value => this; //Remove when import to new version

    public void Invalidate() => Destroy(this); //Remove when import to new version

    #region Unity Logic

    private void Awake() => Instance.Instantiate();

    private void OnDestroy() => Instance.RemoveInstance();

    #endregion

    #region Public Methods



    #endregion

    #region Private Methods



    #endregion

}