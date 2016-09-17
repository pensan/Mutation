// REDOX Game Labs 2016

#region INCLUDES
using UnityEngine;
#endregion

[System.Serializable]
public class TimeManager : MonoBehaviour 
{
    private float originalFixedPhysicsTime;
    private float originalTimeScale;

    private void Awake()
    {
        originalFixedPhysicsTime = Time.fixedDeltaTime;
        originalTimeScale = Time.timeScale;
    }

    [SerializeField]
    private float _customTime;
    public float CustomTime
    {
        get
        {
            return _customTime;
        }

        set
        {
            _customTime = value;

            if (Application.isPlaying)
            {
                Time.timeScale = _customTime;

                if (_customTime > 0.0f)
                {
                    Time.fixedDeltaTime = originalFixedPhysicsTime / _customTime;
                }
                else
                {
                    Time.fixedDeltaTime = 0.0f;
                }
            }
   
        }
    }

    void OnDestroy()
    {
        Time.fixedDeltaTime = originalFixedPhysicsTime;
        Time.timeScale = originalTimeScale;
    }
}
