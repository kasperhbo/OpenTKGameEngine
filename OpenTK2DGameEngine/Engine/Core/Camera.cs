using MarioGabeKasper.Engine.Components;
using MarioGabeKasper.Engine.GUI;
using OpenTK.Mathematics;

namespace MarioGabeKasper.Engine.Core
{
    public class Camera : GameObject
    {
        public readonly bool IsSceneCamera;      
        
        // Those vectors are directions pointing outwards from the camera to define how it rotated.
        private readonly Vector3 _front = -Vector3.UnitZ;
        private readonly Vector3 _up = Vector3.UnitY;
        
        private Matrix4 inverseProjection, inverseView, projectionMatrix, viewMatrix;

        private System.Numerics.Vector2 projectSize = new System.Numerics.Vector2(32 * 40, 32 * 21);

        
        public Camera(string name, Transform transform, bool isSceneCamera) : base(name, transform, 0)
        {
            this.IsSceneCamera = isSceneCamera;
            projectionMatrix = new Matrix4();
            viewMatrix = new Matrix4();
            inverseProjection = new Matrix4();
            inverseView = new Matrix4();
            AdjustProjection();
        }
        
        // public Camera(Vector3 position, bool isSceneCamera)
        // {
        //     this.Position = new Vector2(position.X, position.Y);
        //     this.IsSceneCamera = isSceneCamera;

        // }

        public void AdjustProjection()
        {
            // projectionMatrix = Matrix4.Identity;
            projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, projectSize.X, 0, projectSize.Y, 0, 100f);
            Matrix4.Invert(projectionMatrix, out inverseProjection);
        }

        public void ImGui(ImGuiController imGuiController)
        {

        }

        public Matrix4 GetViewMatrix()
        {
            viewMatrix = Matrix4.LookAt(new Vector3(Transform.Position.X, Transform.Position.Y, 0), new Vector3(Transform.Position.X, Transform.Position.Y, 0) + _front, _up);
            Matrix4.Invert(viewMatrix, out inverseView);
            return viewMatrix;
        }

        public Matrix4 GetProjectionMatrix()
        {
            return projectionMatrix;
        }

        public Matrix4 GetInverseProjection()
        {
            return inverseProjection;
        }

        public Matrix4 GetInverseView()
        {
            return inverseView;
        }

        public System.Numerics.Vector2 GetProjectSize()
        {
            return projectSize;
        }

    }
}