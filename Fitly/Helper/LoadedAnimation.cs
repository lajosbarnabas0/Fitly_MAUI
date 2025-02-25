using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitly.Helper
{
    public static class LoadedAnimation
    {
        public static async Task AnimateElementsOnPage(Page page)
        {
            var elements = page.GetVisualTreeDescendants().OfType<VisualElement>();

            var animationTasks = new List<Task>();

            foreach (var element in elements)
            {
                if (!string.IsNullOrEmpty(element.StyleId))
                {
                    element.Opacity = 0;
                    element.TranslationY = -50;

                    var translationTask = element.TranslateTo(0, 0, 800, Easing.SinInOut);
                    var fadeTask = element.FadeTo(1, 800, Easing.SinInOut);

                    animationTasks.Add(translationTask);
                    animationTasks.Add(fadeTask);
                }
            }
            await Task.WhenAll(animationTasks);
        }
    }
}
