using System.Collections.Generic;
using amulware.Graphics;
using Bearded.Photones.Performance;
using Bearded.Photones.Rendering;
using Bearded.Photones.UI;

namespace Bearded.Photones.Screens {
    class ScreenLayerCollection {
        private readonly List<IScreenLayer> _screenLayers = new List<IScreenLayer>();
        private readonly HashSet<IScreenLayer> _screenLayersToRemove = new HashSet<IScreenLayer>();
        private ViewportSize _viewportSize;

        protected bool PropagateInput(UpdateEventArgs args, InputState inputState) {
            for (var i = _screenLayers.Count - 1; i >= 0; i--) {
                if (!_screenLayers[i].HandleInput(args, inputState)) {
                    return false;
                }
            }
            return true;
        }

        protected void UpdateAll(BeardedUpdateEventArgs args) {
            _screenLayers.ForEach((layer) => layer.Update(args));
            removeScreenLayersQueuedForRemoval();
        }

        public void OnResize(ViewportSize newSize) {
            _viewportSize = newSize;
            _screenLayers.ForEach((layer) => layer.OnResize(newSize));
        }

        public void Render(RenderContext context) {
            _screenLayers.ForEach((layer) => layer.Render(context));
        }

        public void AddScreenLayerOnTop(IScreenLayer screenLayer) {
            _screenLayers.Add(screenLayer);
            initializeScreenLayer(screenLayer);
        }

        private void addScreenLayerAtIndex(int index, IScreenLayer screenLayer) {
            _screenLayers.Insert(index, screenLayer);
            initializeScreenLayer(screenLayer);
        }

        public void AddScreenLayerOnTopOf(IScreenLayer reference, IScreenLayer toAdd) {
            // Equivalent to adding screen layer _after_ reference.
            var indexOfRef = _screenLayers.IndexOf(reference);
            if (indexOfRef == _screenLayers.Count)
                AddScreenLayerOnTop(toAdd);
            else
                addScreenLayerAtIndex(indexOfRef + 1, toAdd);
        }

        public void AddScreenLayerBehind(IScreenLayer reference, IScreenLayer toAdd) {
            // Equivalent to adding screen layer _before_ reference.
            addScreenLayerAtIndex(_screenLayers.IndexOf(reference), toAdd);
        }

        private void initializeScreenLayer(IScreenLayer screenLayer) {
            screenLayer.OnResize(_viewportSize);
        }

        public void RemoveScreenLayer(IScreenLayer screenLayer) {
            // If you delete a screen layer during the HandleInput method or outside of the main loop entirely, another
            // update may take place before the screen is actually deleted.
            _screenLayersToRemove.Add(screenLayer);
        }

        private void removeScreenLayersQueuedForRemoval() {
            if (_screenLayersToRemove.Count == 0) {
                return;
            }
            _screenLayers.RemoveAll(layer => _screenLayersToRemove.Contains(layer));
            _screenLayersToRemove.Clear();
        }
    }
}