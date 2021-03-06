﻿using UnityEngine;

namespace Constants
{
    public static class Consts
    {
        public static class Tags
        {
            public const string PLAYER = "Player";
        }

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

            public const string AUDIO_MANAGER_VOLUME = "Volume";
        }

        public static class Colors
        {
            public static Color White = new Color(1, 1, 1, 1);
            public static Color WhiteFaded = new Color(1, 1, 1, 0.4f);
            public static Color Red = new Color(1f, 0.4f, 0.4f, 1);
            public static Color RedFaded = new Color(1f, 0.4f, 0.4f, 0.4f);
            public static Color Green = new Color(0.19f, 0.93f, 0.62f, 1);
            public static Color Grey = new Color(0.41f, 0.41f, 0.41f);
            public static Color Transparent = new Color(1, 1, 1, 0);
        }

        public static class Tooltip
        {
            // Position of the text shadow
            public static Vector2 TOOLTIP_SHADOW_POSITION = new Vector2(-2f, -2f);

            // Color of the text shadow
            public static Color TOOLTIP_SHADOW_COLOR = new Color(0.1f, 0.1f, 0.1f);
        }

        public static class Animation
        {
            public const string ATTACK_TRIGGER = "attack";
            public const string HIT_TRIGGER = "hit";
            public const string DIE_TRIGGER = "die";
            public const string INTERACT_TRIGGER = "interact";

            public const string SLEEP_BOOL = "isSleeping";
            public const string OPEN_BOOL = "open";

            public const string MOVEMENT_SPEED_FLOAT = "movementSpeed";
            public const string ATTACK_SPEED_FLOAT = "attackSpeed";
            public const string INTERACT_SPEED_FLOAT = "interactSpeed";
        }
    }
}