using System;
using Accord.Math.Decompositions;
using MarioGabeKasper.Engine.Components;
using MarioGabeKasper.Engine.Core;
using MarioGabeKasper.Engine.Renderer;
using MarioGabeKasper.Engine.Sound;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace MarioGabeKasper.Engine.Serializers
{
    public class BaseSpecifiedConcreteClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if ((
                    typeof(Component).IsAssignableFrom(objectType) ||
                    typeof(Texture).IsAssignableFrom(objectType) ||
                    typeof(GameObject).IsAssignableFrom(objectType))
                    && !objectType.IsAbstract)
            {
                return null;
            }
            return base.ResolveContractConverter(objectType);
        }
    }

    public class ComponentSerializer : JsonConverter
    {
        static JsonSerializerSettings _specifiedSubclassConversion =
            new JsonSerializerSettings()
            {
                ContractResolver = new BaseSpecifiedConcreteClassConverter()
            };

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Component) || objectType == typeof(Texture);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            switch (jo["ObjType"].Value<int>())
            {
                case -1:
                    return null;
                case 1:
                    return JsonConvert.DeserializeObject<FontRenderer>(jo.ToString(), _specifiedSubclassConversion);
                case 2:
                    return JsonConvert.DeserializeObject<SpriteRenderer>(jo.ToString(), _specifiedSubclassConversion);
                case 3:
                    return JsonConvert.DeserializeObject<Rigidbody>(jo.ToString(), _specifiedSubclassConversion);
                case 4:
                    return JsonConvert.DeserializeObject<Texture>(jo.ToString(), _specifiedSubclassConversion);
                case 5:
                    return JsonConvert.DeserializeObject<GameObject>(jo.ToString(), _specifiedSubclassConversion);
                case 8:
                    return JsonConvert.DeserializeObject<GameEngineSound>(jo.ToString(), _specifiedSubclassConversion);
                default:
                    throw new Exception();
            }
        }


        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }

    }
}