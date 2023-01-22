
//tcpServer.Start();

using Raylib_CsLo;
using RealtimeApp.Tests.RaylibTest;

//await Runner.Run();

Raylib.InitWindow(900, 480, "TEST");
Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
Raylib.SetTargetFPS(60);
var show = false;

// Main game loop
while (!Raylib.WindowShouldClose()) // Detect window close button or ESC key
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Raylib.SKYBLUE);
    Raylib.DrawFPS(10, 10);
    Update();
    Raylib.EndDrawing();
}

void Update()
{
    if (RayGui.GuiButton(new Rectangle(50, 25, 100, 30), "Click me"))
    {
        Console.WriteLine("Clicked");
        show = true;
        
    }

    if (show)
    {
        var clicked = RayGui.GuiMessageBox(new Rectangle(50, 50, 500, 400), "message box", "you clicked", "yes;no");
        if (clicked == 1)
        {
            Console.WriteLine(clicked == 0 ? "yes" : "no");
            show = false;
        }
        else if(clicked == 2 || clicked == 0)
        {
            show = false;
        }
    }

}

Raylib.CloseWindow();