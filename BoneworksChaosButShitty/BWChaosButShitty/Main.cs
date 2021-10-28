using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnhollowerRuntimeLib;
using UnityEngine;
using Random = UnityEngine.Random;

namespace YOWC.ShittyBWChaos
{
    public static class BuildInfo
    {
        public const string Name = "BW Chaos But Shitty"; // Name of the Mod.  (MUST BE SET)
        public const string Author = "YOWChap"; // Author of the Mod.  (Set as null if none)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.0.0"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
    }

    /*
     * 
     * I made this mod as a joke because I was bored.
     * The code is bad and I'm probably not going to fix it.
     * No comments because that would mean looking at this code for even longer.
     * 
     */

    public class Main : MelonMod
    {
        private List<MethodInfo> validMethods = new List<MethodInfo>();
        private List<FieldInfo> validFields = new List<FieldInfo>();
        private Dictionary<Type, UnityEngine.Object[]> validMonoBehaviours = new Dictionary<Type, UnityEngine.Object[]>();

        private float minWaitTime = 0.5f;
        private float maxWaitTime = 2f;

        private Func<Type, bool> IsValidVariableType = type => type == typeof(int) || type == typeof(float) || type == typeof(bool) || type == typeof(string);


        public override void OnApplicationLateStart()
        {
            MelonMod[] melonMods = MelonHandler.Mods.Where(x => x != this).ToArray();
            foreach (MelonMod mod in melonMods)
            {
                Assembly assembly = mod.Assembly;
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).Where(x => !x.IsGenericMethod && x.GetParameters().Length == 0).ToArray();
                    validMethods.AddRange(methods);

                    FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Where(x => IsValidVariableType(x.FieldType)).ToArray();
                    validFields.AddRange(fields);
                }
            }

            MelonCoroutines.Start(CallRandomMethod());
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            validMonoBehaviours.Clear();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(MonoBehaviour)))
                    {
                        try
                        {
                            UnityEngine.Object[] objects = GameObject.FindObjectsOfType(Il2CppType.From(type));
                            if (objects.Length > 0)
                                validMonoBehaviours.Add(type, objects);
                        }
                        catch { }
                    }
                }
            }

            MelonLogger.Msg(validMonoBehaviours.Count);
        }

        private object GetRandomVariableValue(Type varType)
        {
            if (varType == typeof(int))
            {
                return Random.Range(int.MinValue, int.MaxValue);
            }
            else if (varType == typeof(float))
            {
                return Random.Range(float.MinValue, float.MaxValue);
            }
            else if (varType == typeof(bool))
            {
                return Random.value > 0.5f;
            }
            else if (varType == typeof(string))
            {
                string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+/?,.<>";
                int length = Random.Range(1, 50);
                return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Range(0, s.Length)]).ToArray());
            }

            return null;
        }

        private IEnumerator CallRandomMethod()
        {
            yield return new WaitForSecondsRealtime(Random.Range(minWaitTime, maxWaitTime));

            float val = Random.value;
            if (val < 1/3f)
            {
                MethodInfo method = validMethods[Random.Range(0, validMethods.Count)];
                MelonLogger.Msg(ConsoleColor.Green, $"Invoking {method.DeclaringType.FullName}.{method.Name}");
                try { method.Invoke(null, null); } catch { }
            }
            else if (val < 2/3f)
            {
                FieldInfo field = validFields[Random.Range(0, validFields.Count)];
                object value = GetRandomVariableValue(field.FieldType);
                MelonLogger.Msg(ConsoleColor.Green, $"Setting {field.DeclaringType.FullName}.{field.Name} to {value}");
                try { field.SetValue(null, value); } catch { }
            }
            else
            {
                try
                {
                    Type type = validMonoBehaviours.Keys.ElementAt(Random.Range(0, validMonoBehaviours.Count));
                    UnityEngine.Object[] objects = validMonoBehaviours[type];
                    UnityEngine.Object obj = objects[Random.Range(0, objects.Length)];
                    if (obj != null)
                    {
                        if (Random.value > 0.5f)
                        {
                            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(x => IsValidVariableType(x.FieldType)).ToArray();
                            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(x => IsValidVariableType(x.PropertyType)).ToArray();

                            int totalVars = fields.Length + properties.Length;
                            int index = Random.Range(0, totalVars);
                            if (index < fields.Length)
                            {
                                FieldInfo field = fields[index];
                                object value = GetRandomVariableValue(field.FieldType);
                                MelonLogger.Msg(ConsoleColor.Green, $"Setting {field.DeclaringType.FullName}.{field.Name} to {value} on {obj.name}");
                                field.SetValue(obj, value);
                            }
                            else
                            {
                                PropertyInfo property = properties[index - fields.Length];
                                object value = GetRandomVariableValue(property.PropertyType);
                                MelonLogger.Msg(ConsoleColor.Green, $"Setting {property.DeclaringType.FullName}.{property.Name} to {value} on {obj.name}");
                                property.SetValue(obj, value);
                            }
                        }
                        else
                        {
                            MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(x => !x.IsGenericMethod && x.GetParameters().Length == 0).ToArray();
                            MethodInfo method = methods[Random.Range(0, methods.Length)];
                            MelonLogger.Msg(ConsoleColor.Green, $"Invoking {method.DeclaringType.FullName}.{method.Name} on {obj.name}");
                            method.Invoke(null, null);
                        }
                    }
                }
                catch { }
            }
            
            MelonCoroutines.Start(CallRandomMethod());
        }
    }
}
