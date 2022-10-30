using PaintIn3D;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PaintController : MonoBehaviour
    {
        [SerializeField] private float brushSize;
        [SerializeField] private GameObject newColor,yellowTool, redTool, blueTool;

        [SerializeField] private Scrollbar brushScroll;

        private void Awake()
        {
            SwitchToYellow();
            brushSize = 1;
        }

        public void SwitchToRed()
        {
            newColor = redTool;
            ChangeBrushSize();
            yellowTool.SetActive(false);
            redTool.SetActive(true);
            blueTool.SetActive(false);
        }

        public void SwitchToBlue()
        {
            newColor = blueTool;
            ChangeBrushSize();
            yellowTool.SetActive(false);
            redTool.SetActive(false);
            blueTool.SetActive(true);
        }

        public void SwitchToYellow()
        {
            newColor = yellowTool;
            ChangeBrushSize();
            yellowTool.SetActive(true);
            redTool.SetActive(false);
            blueTool.SetActive(false);
            
        }

        public void ChangeBrushSize()
        {
            brushSize = brushScroll.value;
            var scale = newColor.GetComponent<P3dPaintSphere>().Scale;
            
            newColor.GetComponent<P3dPaintSphere>().Scale = new Vector3(brushSize*4,brushSize*4,brushSize*4);
        }
        
    }
}