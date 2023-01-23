using Raylib_CsLo;
using RealtimeApp.Tests.RaylibTest;

await Runner.Run();
return;

Raylib.InitWindow(900, 480, "TEST");
Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
Raylib.SetTargetFPS(60);
var show = false;
// var canvas = new RaylibCanvas();

// Main game loop
while (!Raylib.WindowShouldClose()) // Detect window close button or ESC key
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Raylib.SKYBLUE);
    Raylib.DrawFPS(10, 10);
    Update();
    Raylib.EndDrawing();
}

Physac.ClosePhysics();

void Update()
{
    // canvas.Update();
}

Raylib.CloseWindow();