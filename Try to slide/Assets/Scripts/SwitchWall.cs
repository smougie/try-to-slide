using UnityEngine;

public class SwitchWall : MonoBehaviour
{
    [SerializeField] private GameObject switchTrigger = null;
    [SerializeField] private GameObject disappearingWall = null;
    [SerializeField] private ParticleSystem stars = null;

    private bool wallDisappearing;
    private Animation switchAnimation;
    private Renderer wallRenderer;
    private Color wallColor;
    private Vector3 wallScale;
    private Vector3 starsScale;

    void Start()
    {
        switchAnimation = switchTrigger.GetComponent<Animation>();
        wallRenderer = disappearingWall.GetComponent<Renderer>();
        wallColor = wallRenderer.material.color;
        scaleParticleSystem();
    }

    void Update()
    {
        if (wallDisappearing)
        {
            wallColor = new Color(wallColor.r, wallColor.g, wallColor.b, wallColor.a - .01f);
            wallRenderer.material.color = wallColor;
        }
        if (wallDisappearing && wallRenderer.material.color.a <= 0)
        {
            wallDisappearing = false;
            switchTrigger.GetComponent<BoxCollider>().enabled = false;
            Destroy(disappearingWall);
            Destroy(stars);
        }
    }

    public void SwitchTrigger()
    {
        wallDisappearing = true;
        switchAnimation.Play();
        stars.Play();

    }

    private void scaleParticleSystem()
    {
        wallScale = disappearingWall.transform.lossyScale;
        var starsShape = stars.shape;
        starsScale = wallScale;
        starsShape.scale = starsScale;
    }
}
