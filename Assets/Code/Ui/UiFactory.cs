using Assets.Code.Data;
using Assets.Code.Tools;
using Assets.Code.Ui.Windows;
using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Assets.Code.Ui
{
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
                [typeof(FPSWindow)] = CreateFPSView,
                [typeof(MenuWindow1)] = CreateMenuWindow,

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
            return _uIConfig.FadeWindow.Instantiate(_canvas.transform, false);
        }

        private BaseWindow CreateDeathWindow()
        {
            return _uIConfig.DeathWindow.Instantiate(_canvas.DeathWindowPoint, false).Initialize();
        }

        private BaseWindow CreateFPSView()
        {
            return _uIConfig.FPSWindow.Instantiate(_canvas.transform, false);
        }

        private BaseWindow CreateMenuWindow()
        {
            return _uIConfig.MenuWindow.Instantiate(_canvas.transform, false);
        }
    }
}