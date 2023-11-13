using System.Collections.Generic;
using System.Linq;
using _Workspace.CodeBase.GamePlay.Logic;
using _Workspace.CodeBase.GamePlay.Logic.Service.Color.Data;
using _Workspace.CodeBase.GamePlay.Progress;
using Newtonsoft.Json;
using UnityEngine;

namespace _Workspace.CodeBase.Extensions
{
    public static class DataExtension
    {
        public static T ToDeserialized<T>(this string json)
            => JsonConvert.DeserializeObject<T>(json);

        public static string ToJson(this object obj)
            => JsonConvert.SerializeObject(obj);

        public static Color ToUnityColor(this ColorData colorData)
            => new(colorData.R, colorData.G, colorData.B);


        private static ColorData ToColorData(this Color color)
            => new(color.r, color.g, color.b);

        public static TileItemData ToItemData(this TileItem item)
        {
            return new TileItemData()
            {
                ColorData = item.GetColor().ToColorData(),
                X = item.GetX(),
                Y = item.GetY()
            };
        }

        public static Dictionary<Color, int> ToMap(this List<ColorValueData> colorValuesData)
        {
            Dictionary<Color, int> map = new Dictionary<Color, int>();

            foreach (ColorValueData colorValue in colorValuesData)
                map.Add(colorValue.Color.ToUnityColor(), colorValue.Value);

            return map;
        }

        public static List<ColorValueData> ToDataList(this Dictionary<Color, int> map)
        {
            List<KeyValuePair<Color, int>> list = map.ToList();
            List<ColorValueData> dataList = new List<ColorValueData>();

            foreach (KeyValuePair<Color, int> keyValuePair in list)
                dataList.Add(new ColorValueData(keyValuePair.Value, keyValuePair.Key.ToColorData()));

            return dataList;
        }
    }
}