

namespace DotnetAndroid.MapboxQs;

using Com.Mapbox.Maps.Plugins.Viewport;
using Com.Mapbox.Maps.Plugins.Viewport.State;
using Com.Mapbox.Maps.Plugins.Viewport.Transition;

public static class ViewPortPluginExtensions
{
    internal class CompletionListenerAction : Java.Lang.Object, ICompletionListener
    {
        Action<bool> action;

        public CompletionListenerAction(Action<bool> action)
        {
            this.action = action;
        }

        public void OnComplete(bool isFinished) => this.action?.Invoke(isFinished);
    }

    public static void TransitionTo(this IViewportPlugin viewportPlugin, IViewportState targetState, IViewportTransition transition, Action<bool> completion)
        => viewportPlugin.TransitionTo(targetState, transition, completion != null ? new CompletionListenerAction(completion) : null);

    public static Task<bool> TransitionToAsync(this IViewportPlugin viewportPlugin, IViewportState targetState, IViewportTransition transition, Action<bool> completion)
    {
        var tcs = new TaskCompletionSource<bool>();

        viewportPlugin.TransitionTo(targetState, transition, isFinished => tcs.TrySetResult(isFinished));

        return tcs.Task;
    }
}