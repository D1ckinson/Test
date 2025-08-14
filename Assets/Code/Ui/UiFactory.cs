using Assets.Code.Ui;
using Assets.Scripts.Tools;
using Assets.Scripts.Ui;
using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public class UiFactory
{
    private readonly Dictionary<Type, BaseWindow> _cash = new();
    private readonly Dictionary<Type, Func<BaseWindow>> _createMethods;
    private readonly TestCanvasUiFactory _canvas;
    private readonly UIConfig _uIConfig;

    public UiFactory(UIConfig uIConfig)
    {
        _uIConfig = uIConfig.ThrowIfNull();
        var a = Object.Instantiate(_uIConfig.TestCanvasUiFactory);
        _canvas = a;
        _createMethods = new()
        {
            [typeof(DeathWindow)] = CreateDeathWindow,
        };
    }

    public T Create<T>() where T : BaseWindow
    {
        if (_cash.TryGetValue(typeof(T), out BaseWindow window))
        {
            return (T)window;
        }

        return (T)_createMethods[typeof(T)].Invoke();
    }

    private DeathWindow CreateDeathWindow()
    {
        return Object.Instantiate(_uIConfig.DeathWindow, _canvas.DeathWindowPoint, false).Initialize();
    }
}
