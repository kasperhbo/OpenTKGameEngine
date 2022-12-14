using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MarioGabeKasper.Engine.GUI
{
    public class DefaultImGuiFieldWindow
    {

        public void CreateDefaultFieldWindow()
        {
            string[] empty = new string[0];
            CreateDefaultFieldWindow(empty);
        }

        public void CreateDefaultFieldWindow(string[] fieldsToIgnore)
        {
            FieldInfo[] fields = this.GetType().GetFields(
                                                          BindingFlags.DeclaredOnly
                                                        | BindingFlags.Public
                                                        | BindingFlags.NonPublic
                                                        | BindingFlags.Instance); foreach (var field in fields)
            {
                Type type = field.FieldType;
                var value = field.GetValue(this);
                string name = field.Name;

                if (fieldsToIgnore.Contains(name))
                    return;
                
                if (type == typeof(int))
                {
                    int val = (int)value;

                    if (ImGuiNET.ImGui.DragInt(name + ": ", ref val))
                    {
                        field.SetValue(this, val);
                    }
                }

                if (type == typeof(float))
                {
                    float val = (float)value;
                    
                    
                    if (ImGuiNET.ImGui.DragFloat(name + ": ", ref val))
                    {
                        field.SetValue(this, val);
                    }
                }

                if (type == typeof(bool))
                {
                    bool val = (bool)value;

                    if (ImGuiNET.ImGui.Checkbox(name + ": ", ref val))
                    {
                        field.SetValue(this, !val);
                    }
                }

                if (type == typeof(Vector2))
                {
                    Vector2 val = (Vector2)value;

                    if (ImGuiNET.ImGui.DragFloat2(name + ": ", ref val))
                    {
                        field.SetValue(this, val);
                    }
                }

                if (type == typeof(Vector3))
                {
                    Vector3 val = (Vector3)value;

                    if (ImGuiNET.ImGui.DragFloat3(name + ": ", ref val))
                    {
                        field.SetValue(this, val);
                    }
                }

                if (type == typeof(Vector4))
                {
                    Vector4 val = (Vector4)value;

                    if (ImGuiNET.ImGui.DragFloat4(name + ": ", ref val))
                    {
                        field.SetValue(this, val);
                    }
                }
            }
        }
    }
}
