using Godot;
using System;
using System.IO;

namespace CardinalEngine {

    public static class CardinalUtils {

        public static void VerifyPackedScenesType<T>(PackedScene[] scenes) {
            for (int i = scenes.Length - 1; i >= 0; i--) {
                PackedScene scene = scenes[i];
                if (scene != null) {
                    try {
                        String sceneType = Path.GetFileNameWithoutExtension(((Script)scene.Instantiate().GetScript()).GetBaseScript().ResourcePath);
                        if (Type.GetType(sceneType) is T) {
                            scenes[i] = null;
                        }
                    } catch {
                        scenes[i] = null;
                    }
                }
            }
        }
    }
}