using System;
using MarioGabeKasper.Engine.Core;
using MarioGabeKasper.Engine.GUI;
using MarioGabeKasper.Engine.Serializers;
using Newtonsoft.Json;

namespace MarioGabeKasper.Engine.Components
{
    [JsonConverter(typeof(ComponentSerializer))]
    public abstract class Component : DefaultImGuiFieldWindow
    {
        public int Uid = -1;
        public int ObjType = -1;
        protected static int IdCounter = 0;
        
        [JsonIgnore]public GameObject Parent;
        
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