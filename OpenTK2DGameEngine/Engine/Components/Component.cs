using MarioGabeKasper.Engine.Core;
using MarioGabeKasper.Engine.GUI;
using MarioGabeKasper.Engine.Serializers;
using Newtonsoft.Json;

namespace MarioGabeKasper.Engine.Components
{
    [JsonConverter(typeof(ComponentSerializer))]
    public abstract class Component : DefaultImGuiFieldWindow
    {
        
        protected static int IdCounter = 0;
        public int Uid = -1;
        
        protected GameObject Parent = null;
        public int ObjType = -1;
        
        public virtual void Start(GameObject gameObject)
        {
            this.Parent = gameObject;
            SetObjectType();
        }

        public virtual void Update(float dt)
        {
            
        }

        public virtual void ImGui_()
        {
            CreateDefaultFieldWindow();
        }
        
        
        public void SetGameObject(GameObject go)
        {
            this.Parent = go;
        }
        
        public GameObject GetGameObject()
        {
            return Parent;
        }

        public abstract void SetObjectType();

        public void GenerateId()
        {
            if (this.Uid == -1)
            {
                this.Uid = IdCounter++;
            }
        }

        public static void Init(int maxId)
        {
            IdCounter = maxId;
        }
        

    }
}