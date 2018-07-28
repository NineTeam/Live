using UnityEngine;

public class SoundController : MonoBehaviour
{
    public bool musicOpen = false;
    public bool soundOpen = false;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }
}
