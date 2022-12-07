using MarioGabeKasper.Engine.Core;
using MarioGabeKasper.Engine.GUI;
using MarioGabeKasper.Engine.Serializers;
using Newtonsoft.Json;

namespace MarioGabeKasper.Engine.Components
{
    [JsonConverter(typeof(ComponentSerializer))]
    public abstract class Component : DefaultImGuiFieldWindow
    {
        
        protected static int idCounter = 0;
        public int uid = -1;
        
        protected GameObject parent = null;
        public int objType = -1;
        
        public virtual void Start(GameObject gameObject)
        {
            this.parent = gameObject;
            SetObjectType();
        }

        public virtual void update(float dt)
        {
            
        }

        public virtual void ImGui()
        {
            CreateDefaultFieldWindow();
        }
        
        
        public void SetGameObject(GameObject go)
        {
            this.parent = go;
        }
        
        public GameObject GetGameObject()
        {
            return parent;
        }

        public abstract void SetObjectType();

        public void GenerateID()
        {
            if (this.uid == -1)
            {
                this.uid = idCounter++;
            }
        }

        public int GetUID()
        {
            return uid;
        }

        public static void Init(int maxID)
        {
            idCounter = maxID;
        }
        

    }
}