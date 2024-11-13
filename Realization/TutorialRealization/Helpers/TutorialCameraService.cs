using UnityEngine;

namespace Realization.TutorialRealization.Helpers
{
    public class TutorialCameraService
    {
        private Camera _camera;

        public TutorialCameraService(Camera camera)
        {
            _camera = camera;
        }
        
        public void ChangeTargetTexture(RenderTexture texture)
        {
            _camera.targetTexture = texture;
        }
    }
}