using Raylib_CsLo;

namespace RealtimeApp.Tests.RaylibTest;

public unsafe class RaylibCanvas
{
    private int active = -1;
    private int index = 0;
    public RaylibCanvas()
    {
        
    }

    private float slider;
    public void Update()
    {
        if (RayGui.GuiButton(new Rectangle(50, 25, 100, 30), "Click me"))
        {
            Console.WriteLine("Clicked");
        }

        fixed (int* scrollIndex = &index)
        {
            active = RayGui.GuiListView(new Rectangle(50, 50, 100, 135), "One;Two;Three;Four", scrollIndex, active);
        }
        // RayGui.GuiLoadStyle();
        slider = RayGui.GuiSliderBar(new Rectangle(300, 50, 100, 20), 1.ToString(), 100.ToString(), slider, 1, 100);
        Raylib.DrawText($"Value - {slider.ToString("00")}", 300, 40, 10, Raylib.RED);
        if (active != -1)
        {
            
        }
    }
}