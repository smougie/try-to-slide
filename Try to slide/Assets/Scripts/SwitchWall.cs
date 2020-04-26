using UnityEngine;

public class SwitchWall : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject switchTrigger = null;  // switch trigger object
    [SerializeField] private GameObject disappearingWall = null;  // disappearing wall object
    [SerializeField] private ParticleSystem stars = null;  // stars effect object

    private bool wallDisappearing;  // flag for wall disappearing
    private Animation switchAnimation;  // switch animation
    private Renderer wallRenderer;  // wall render - setting the alpha color here
    private Color wallColor;  // wall color
    private Vector3 wallScale;  // wall scale and position 
    private Vector3 starsScale;  // starts effect scale and position

    #endregion

    void Start()
    {
        switchAnimation = switchTrigger.GetComponent<Animation>();  // initializing switch animation
        wallRenderer = disappearingWall.GetComponent<Renderer>();  // initializng disappearing wall animation
        wallColor = wallRenderer.material.color;  // initializing disappearing wall color
        scaleParticleSystem();  // calling method responsible for setting stars scale bigger than wall size
    }

    void Update()
    {
        // if falg is raised, turning wall alpha color 
        if (wallDisappearing)
        {
            wallColor = new Color(wallColor.r, wallColor.g, wallColor.b, wallColor.a - .01f);
            wallRenderer.material.color = wallColor;
        }
        // if flag is raised and wall alpha color is equal to 0 (invisible), drop flag, turn off switch object, destroy wall object and starts effect
        if (wallDisappearing && wallRenderer.material.color.a <= 0)
        {
            wallDisappearing = false;
            switchTrigger.GetComponent<BoxCollider>().enabled = false;
            Destroy(disappearingWall);
            Destroy(stars);
        }
    }

    // Method responsible for turning on wallDisappearing flag, palying switch and starts animation  
    public void SwitchTrigger()
    {
        wallDisappearing = true;
        switchAnimation.Play();
        stars.Play();
    }

    // Method responsible for setting starts effect scale to bit bigger than wall object
    private void scaleParticleSystem()
    {
        wallScale = disappearingWall.transform.lossyScale;
        var starsShape = stars.shape;
        starsScale = wallScale;
        starsShape.scale = starsScale;
    }
}
