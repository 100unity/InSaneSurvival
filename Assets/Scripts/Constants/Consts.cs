using UnityEngine;

namespace Constants
{
    public static class Consts
    {
        public static class Scene
        {
            public const int MAIN_MENU = 0;
            public const int GAME = 1;
            public const int DEATH = 2;
        }

        public static class Utils
        {
            // maxDistance for mapping random points onto the NavMesh
            public const float MAX_NAVMESH_MAPPING_DISTANCE = 1.0f;

            // just a high number to ensure random point generation for wandering
            public const int MAPPING_ITERATIONS = 500;
        }

        public static class Colors
        {
            public static Color White = new Color(1, 1, 1, 1);
            public static Color WhiteFaded = new Color(1, 1, 1, 0.4f);
            public static Color Red = new Color(1f, 0.4f, 0.4f, 1);
            public static Color RedFaded = new Color(1f, 0.4f, 0.4f, 0.4f);
        }

        public static class Tooltip
        {
            // Position of the text shadow
            public static Vector2 TOOLTIP_SHADOW_POSITION = new Vector2(-2f, -2f);

            // Color of the text shadow
            public static Color TOOLTIP_SHADOW_COLOR = new Color(0.1f, 0.1f, 0.1f);
        }
    }
}