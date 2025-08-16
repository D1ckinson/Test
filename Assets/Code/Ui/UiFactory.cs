using Assets.Code.Tools;
using Assets.Code.Ui;
using Assets.Code.Ui.Windows;
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
            [typeof(FadeWindow)] = CreateFadeWindow,
            [typeof(FPSView)] = CreateFPSView,
        };
    }

    public T Create<T>() where T : BaseWindow
    {
        if (_cash.TryGetValue(typeof(T), out BaseWindow window))
        {
            window.SetActive(true);

            return (T)window;
        }

        window = _createMethods[typeof(T)].Invoke();
        _cash.Add(typeof(T), window);

        return (T)window;
    }

    private BaseWindow CreateFadeWindow()
    {
        return Object.Instantiate(_uIConfig.FadeWindow, _canvas.transform, false);
    }

    private BaseWindow CreateDeathWindow()
    {
        return Object.Instantiate(_uIConfig.DeathWindow, _canvas.DeathWindowPoint, false).Initialize();
    }

    private BaseWindow CreateFPSView()
    {
        return Object.Instantiate(_uIConfig.FPSView, _canvas.transform, false);
    }
}
