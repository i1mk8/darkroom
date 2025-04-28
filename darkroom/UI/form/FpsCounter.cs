using System.Diagnostics;

namespace darkroom.UI.form;

public class FpsCounter
{
    private readonly Stopwatch _fpsWatch = new();
    private int _frameCount;

    public FpsCounter()
    {
        _fpsWatch.Start();
    }

    public int? Update()
    {
        _frameCount++;
        
        if (_fpsWatch.ElapsedMilliseconds < 1000)
            return null;
        
        var fps = _frameCount * 1000 / (int)_fpsWatch.ElapsedMilliseconds;
        _frameCount = 0;
        _fpsWatch.Restart();
        
        return fps;
    }
}